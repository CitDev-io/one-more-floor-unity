using System.Collections.Generic;
using System.Linq;

public delegate void TilesDelegate(List<Tile> t);
public delegate void TileDelegate(Tile t);
public delegate void IntDelegate(int i);
public delegate void NoParamDelegate();
public delegate void StatSheetDelegate(StatSheet statSheet);
public delegate void DamageDelegate(DamageResult result);
public delegate void SourcedDamageDelegate(int damage, DamageSource source);

public class GameBoard
{
    public TilesDelegate OnPlayerCollectedTiles, OnSelectionChange;
    public TileDelegate OnTileAddedToSelection;
    public IntDelegate OnCoinCollected, OnPotionsCollected;
    public IntDelegate OnShieldsCollected, OnPoisonCollected, OnSwordsCollected;
    public IntDelegate OnMonsterKillsEarned, OnExperienceGained;
    public NoParamDelegate OnEnemyStunned, OnGoldGoalReached, OnDefenseGoalReached;
    public NoParamDelegate OnExperienceGoalReached, OnReadyForNextTurn;

    public DamageDelegate OnMonstersAttack;
    public List<Tile> Tiles = new List<Tile>();
    public StatSheetDelegate OnLose;
    public StatSheet Player;
    public int MovesMade = 0;
    bool BoardComplete = false;
    bool AwaitingRoundChange = false;

    List<Tile> selection = new List<Tile>();

    TileSelector tileSelector;
    ChainValidator chainValidator;

    public GameBoard() {
        tileSelector = new TileSelector(new List<TileType> {
            TileType.Shield,
            TileType.Sword,
            TileType.Potion,
            TileType.Coin
        });

        int COUNT_ROWS = 6;
        int COUNT_COLS = 6;

        for (var rowid = 0; rowid < COUNT_ROWS; rowid++) {
            for (var colid = 0; colid < COUNT_COLS; colid++) {
                Tile t = new Tile(
                    colid,
                    rowid,
                    new EnemyStatSheet(){
                        Vitality = 3,
                        Strength = 2
                    },
                    tileSelector.GetNextTile()
                );
                Tiles.Add(t);
            }
        }
        chainValidator = new StandardChainValidator(Tiles, selection);
        Player = new StatSheet();
    }

    public void UserStartSelection(Tile tile)
    {
        ClearSelection();
        DoUserIndicatingTile(tile);
    }

    public void UserEndSelection()
    {
        if (chainValidator.isSelectionFinishable())
        {
            DoPhase_Collection();
        }

        ClearSelection();
    }

    public void UserIndicatingTile(Tile tile)
    {
        DoUserIndicatingTile(tile);
    }

    /*

        private methods


    */

    void DoPhase_Collection() {
        CollectTiles();

        DoPhase_Post();
    }

    void DoPhase_Post() {
        CheckXPLevelUp();
        DoPhase_Monsters();
    }

    void CheckXPLevelUp() {
        if (Player.HasReachedExperienceGoal()) {
            OnExperienceGoalReached?.Invoke();
            Player.SpendDownExp();
        }
    }

    public void RoundProceed()
    {
        if (!AwaitingRoundChange) return;
        DoPhase_Populate();
        AwaitingRoundChange = false;
    }

    void DoPhase_Monsters() {

        var monsters = Tiles.Where((o) => o.tileType == TileType.Monster
            && o.wasAliveBeforeLastUserAction()
            && !o.isStunned()
            && o.isAlive()
        ).ToList();
        MonstersAttack(monsters);
        
        AgeAllMonsters();
        DoPhase_Cleanup();
    }

    void DoPhase_Cleanup() {
        if (!Player.isAlive()) {
            OnLose?.Invoke(Player);
            BoardComplete = true;
            return;
        }
        ClearTiles(selection);
        AwaitingRoundChange = true;
        OnReadyForNextTurn?.Invoke();
    }

    void DoPhase_Populate() {
        var tilesToRepop = Tiles.Where((o) => o.IsBeingCollected || !o.isAlive()).OrderByDescending(o => o.row);
        foreach(Tile t in tilesToRepop) {
            RecascadeTile(t);
        }
        DoPhase_PrePhase();
    }

    void DoPhase_PrePhase() {

    }

    void MonstersAttack(List<Tile> monsters) {
        if (BoardComplete) return;

        int damageReceived = monsters.Sum((o) => o.CurrentMonster.Strength);
        
        if (damageReceived == 0) return;

        var badGuyArmorPiercingAgainstMe = 0; // Consider that if we do pass armor piercing, it'll have to be by-source. that changes the signature anyway
        DamageResult result = Player.TakeDamage(damageReceived, badGuyArmorPiercingAgainstMe);
        OnMonstersAttack?.Invoke(result);
        foreach(Tile monster in monsters) {
            monster.DoAttack();
        }

    }

    void AgeAllMonsters() {
        var monsters = Tiles.Where((o) => o.tileType == TileType.Monster);
        foreach(Tile monster in monsters) {
            monster.AgeUp();
        }
    }

    void ClearSelection()
    {
        selection.Clear();
        SelectionChanged();
    }

    void DoUserIndicatingTile(Tile tile)
    {
        if (BoardComplete) return;
    
        bool AlreadySelected = selection.Contains(tile);

        if (AlreadySelected)
        {
            HandleUserReindicatingTile(tile);
        } else {
            AddTileToSelectionIfEligible(tile);
        }
    }

    void AddTileToSelectionIfEligible(Tile tile)
    {
        if (chainValidator.isEligibleToAddToSelection(tile))
        {
            selection.Add(tile);
            foreach(Tile t in selection) { t.selectedAgainstDamage = 1; }
            OnTileAddedToSelection?.Invoke(tile);
            SelectionChanged();
        }
    }

    void HandleUserReindicatingTile(Tile tile)
    {
        int index = selection.IndexOf(tile);
        List<Tile> tilesToUnhighlight = selection.GetRange(index + 1, selection.Count - index - 1);
        foreach (Tile t in tilesToUnhighlight)
        {
            selection.Remove(t);
            SelectionChanged();
        }
    }

    void SelectionChanged() {
        int dmgSelected = Player.CalcDamageDone(selection.Where((t) => t.tileType == TileType.Sword).Count());
        foreach(Tile t in Tiles) {
            t.selectedAgainstDamage = selection.Contains(t) ? dmgSelected : 0;
        }
        OnSelectionChange?.Invoke(selection);
    }

    void CollectTiles()
    {
        MovesMade += 1;

        //  COINS
        List<Tile> coinsCollected = selection.Where((o) => o.tileType == TileType.Coin).ToList();

        int coinGained = selection
            .Where((o) => o.tileType == TileType.Coin)
            .ToList().Count;

        if (coinGained > 0) {
            CollectionResult cr = Player.CollectCoins(coinGained);
            OnCoinCollected?.Invoke(cr.Earned + cr.BonusGained);
            if (Player.HasReachedCoinGoal()) {
                OnGoldGoalReached?.Invoke();
                Player.SpendDownCoins();
            }
        }

        // POTIONS
        int potionsCollected = selection
            .Where((o) => o.tileType == TileType.Potion)
            .ToList().Count;

        if (potionsCollected != 0)
        {
            CollectionResult cr = Player.CollectPotions(potionsCollected);
            OnPotionsCollected?.Invoke(cr.Earned + cr.BonusGained);
        }


        // SWORDS ENEMIES AND ARMOR
        int shieldsCollected = selection
            .Where((o) => o.tileType == TileType.Shield)
            .ToList().Count;
        int swordsCollected = selection
            .Where((o) => o.tileType == TileType.Sword)
            .ToList().Count;
        
        if (swordsCollected > 0) {
            OnSwordsCollected?.Invoke(swordsCollected);
        }
        
        List<Tile> enemiesInSelection = selection
            .Where((o) => o.tileType == TileType.Monster).ToList();

        if (shieldsCollected != 0 && enemiesInSelection.Count == 0)
        {
            CollectionResult result = Player.CollectShields(shieldsCollected);
            OnShieldsCollected?.Invoke(result.Earned + result.BonusGained);
            if (Player.HasReachedDefenseGoal()) {
                OnDefenseGoalReached?.Invoke();
                Player.SpendDownDefensePoints();
            }
        }
    
        if (enemiesInSelection.Count > 0 && shieldsCollected > 0) {
            OnEnemyStunned?.Invoke();
        }

        int damageDealt = Player.CalcDamageDone(swordsCollected);

        foreach (Tile monster in enemiesInSelection)
        {
            monster.TakeDamage(damageDealt, DamageSource.SwordAttack);
            if (shieldsCollected > 0) {
                monster.Stun();
            }
        }

        int killCount = enemiesInSelection.Where((o) => !o.isAlive()).Count();
        if (killCount > 0) {
            OnMonsterKillsEarned?.Invoke(killCount);
            CollectionResult result = Player.CollectKilledMonsters(killCount);
            OnExperienceGained?.Invoke(result.Earned + result.BonusGained);
        }

        OnPlayerCollectedTiles?.Invoke(selection);
    }

    void ClearTiles(List<Tile> clearedTiles)
    {
        foreach(Tile t in clearedTiles)
        {
            if (t.tileType == TileType.Monster && t.isAlive()) {
                continue;
            }
            t.IsBeingCollected = true;
        }
    }

    void RecascadeTile(Tile tile)
    {
        List<Tile> aboveTiles = Tiles.FindAll((o) => o.col == tile.col && o.row > tile.row);

        foreach(Tile t in aboveTiles)
        {
            t.Dropdown();
        }

        tile.ClearAndDropTileAs(tileSelector.GetNextTile());
    }
}
