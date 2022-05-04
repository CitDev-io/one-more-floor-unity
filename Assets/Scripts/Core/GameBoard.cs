using System.Collections.Generic;
using System.Linq;

public delegate void TilesDelegate(List<Tile> t);
public delegate void TileDelegate(Tile t);
public delegate void IntDelegate(int i);
public delegate void NoParamDelegate();
public delegate void StatSheetDelegate(StatSheet statSheet);
public delegate void DamageDelegate(DamageResult result);
public delegate void SourcedDamageDelegate(int damage);
public delegate void PhaseDelegate(BoardPhase phase);
public delegate void StringDelegate(string str);

public class GameBoard
{
    public TilesDelegate OnPlayerCollectedTiles, OnSelectionChange;
    public TileDelegate OnTileAddedToSelection;
    public IntDelegate OnCoinCollected, OnPotionsCollected;
    public IntDelegate OnShieldsCollected, OnPoisonCollected, OnSwordsCollected;
    public IntDelegate OnMonsterKillsEarned, OnExperienceGained;
    public NoParamDelegate OnEnemyStunned, OnGoldGoalReached, OnDefenseGoalReached;
    public NoParamDelegate OnExperienceGoalReached;
    public PhaseDelegate OnPhaseChange;
    public DamageDelegate OnMonstersAttack;
    public StatSheetDelegate OnLose;
    public StringDelegate OnDebugLog;

    public List<Tile> Tiles = new List<Tile>();
    public PlayerAvatar Player;
    List<Tile> selection = new List<Tile>();

    public int MovesMade { get; private set; } = 0;
    public BoardPhase Phase { get; private set; } = BoardPhase.READY;
    private bool PlayersTurn = true;

    public PlayerItem[] ItemShopOptions { get; private set; }
    public PlayerItem[] EnchantmentShopOptions { get; private set; }
    TileSelector tileSelector;
    MonsterSelector monsterSelector;
    ItemSelector itemSelector;
    EnchantmentSelector enchantmentSelector;
    ChainValidator chainValidator;

    public GameBoard() {
        Player = new PlayerAvatar(
            StatMatrix.BASE_PLAYER()
        );
        tileSelector = new TileSelector(new List<TileType> {
            TileType.Shield,
            TileType.Sword,
            TileType.Potion,
            TileType.Coin
        });
        monsterSelector = new MonsterSelector();
        itemSelector = new ItemSelector();
        enchantmentSelector = new EnchantmentSelector();

        int COUNT_ROWS = 6;
        int COUNT_COLS = 6;
        // board builder
        for (var rowid = 0; rowid < COUNT_ROWS; rowid++) {
            for (var colid = 0; colid < COUNT_COLS; colid++) {
                Tile t = new Tile(
                    colid,
                    rowid,
                    monsterSelector.NextMonster(Player.Level),
                    tileSelector.GetNextTile()
                );
                Tiles.Add(t);
            }
        }

        chainValidator = new StandardChainValidator(Tiles, selection);
        ItemShopOptions = itemSelector.GetShopItemsForPlayer(Player);
        EnchantmentShopOptions = enchantmentSelector.GetEnchantmentItemsForPlayer(Player);

        Trip();
    }

    void Trip() {
        if (Phase == BoardPhase.GAMEOVER) {
            return;
        }

        if (!Player.isAlive()) {
            OnLose?.Invoke(Player);
            SetPhase(BoardPhase.GAMEOVER);
            return;
        }

        if (Player.HasReachedExperienceGoal()) {
            SetPhase(BoardPhase.LEVELUP);
            OnExperienceGoalReached?.Invoke();
            return;
        }
        if (Player.HasReachedCoinGoal()) {
            SetPhase(BoardPhase.ITEMSHOP);
            ItemShopOptions = itemSelector.GetShopItemsForPlayer(Player);
            OnGoldGoalReached?.Invoke();
            return;
        }
        if (Player.HasReachedDefenseGoal()) {
            SetPhase(BoardPhase.ENCHANTMENTSHOP);
            EnchantmentShopOptions = enchantmentSelector.GetEnchantmentItemsForPlayer(Player);
            OnDefenseGoalReached?.Invoke();
            return;
        }
        
        if (!PlayersTurn) {
            SetPhase(BoardPhase.MONSTERTURN);
            DoPhase_MonstersAttack();
            return;
        }

        DoPhase_Populate();
        SetPhase(BoardPhase.PLAYERTURN);
    }

    public void LevelUpPurchase() {
        if (Phase != BoardPhase.LEVELUP) throw new System.Exception($"NOT RIGHT PHASE {Phase}, SHOULD BE LEVELUP");

        Player.SpendDownExp();
        // TODO: Purchase powerups

        Trip();
    }

    public void ItemShopPurchase(int index) {
        if (Phase != BoardPhase.ITEMSHOP) throw new System.Exception($"NOT RIGHT PHASE {Phase}, SHOULD BE ITEMSHOP");

        PlayerItem ItemPurchased = ItemShopOptions[index];
        Player.AddItemToInventory(ItemPurchased);
        Player.SpendDownCoins();
        ItemShopOptions = null;
        
        Trip();
    }

    public void EnchantmentShopPurchase(int index) {
        if (Phase != BoardPhase.ENCHANTMENTSHOP) throw new System.Exception($"NOT RIGHT PHASE {Phase}, SHOULD BE SHOPPING");

        PlayerItem EnchantmentPurchased = EnchantmentShopOptions[index];
        Player.AddEnchantmentToItemInSameSlot(EnchantmentPurchased);
        Player.SpendDownDefensePoints();
        EnchantmentShopOptions = null;
        
        Trip();
    }

    public void UserStartSelection(Tile tile)
    {
        if (Phase != BoardPhase.PLAYERTURN) return;

        ClearSelection();
        DoUserIndicatingTile(tile);
    }

    public void UserEndSelection()
    {
        if (Phase != BoardPhase.PLAYERTURN) return;

        if (!chainValidator.isSelectionFinishable())
        {
            ClearSelection();
            return;
        }

        DoPhase_Collection();
    }

    public void UserIndicatingTile(Tile tile)
    {
        if (Phase != BoardPhase.PLAYERTURN) return;

        DoUserIndicatingTile(tile);
    }

    /*

        private methods


    */

    void SetPhase(BoardPhase p) {
        Phase = p;
        OnPhaseChange?.Invoke(p);
    }

    void DoPhase_Populate() {
        var tilesToRepop = Tiles.Where((o) => o.IsBeingCollected || !o.isAlive()).OrderByDescending(o => o.row);
        foreach(Tile t in tilesToRepop) {
            RecascadeTile(t);
        }
    }

    void DoPhase_MonstersAttack() {
        var monsters = Tiles.Where((o) => o.tileType == TileType.Monster
            && o.wasAliveBeforeLastUserAction()
            && !o.isStunned()
            && o.isAlive()
        ).ToList();
        MonstersAttack(monsters);
        AgeAllMonsters();
        PlayersTurn = true;

        Trip();
    }

    void DoPhase_Collection() {
        CollectTiles();
        PlayersTurn = false;
        TagTilesForCollection(selection);
        ClearSelection();

        Trip();
    }
    
    void MonstersAttack(List<Tile> monsters) {
        int damageReceived = monsters.Sum((o) => o.CurrentMonster.TotalStats.Strength);
        
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
        }
    
        if (enemiesInSelection.Count > 0 && shieldsCollected > 0) {
            OnEnemyStunned?.Invoke();
        }

        int damageDealt = Player.CalcDamageDone(swordsCollected);

        foreach (Tile monster in enemiesInSelection)
        {
            monster.TakeDamage(damageDealt, Player.TotalStats.ArmorPiercing);
        }

        int killCount = enemiesInSelection.Where((o) => !o.isAlive()).Count();
        if (killCount > 0) {
            OnMonsterKillsEarned?.Invoke(killCount);
            CollectionResult result = Player.CollectKilledMonsters(killCount);
            OnExperienceGained?.Invoke(result.Earned + result.BonusGained);
        }

        OnPlayerCollectedTiles?.Invoke(selection);
    }

    void TagTilesForCollection(List<Tile> clearedTiles)
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

        TileType newTileType = tileSelector.GetNextTile();
        tile.CurrentMonster = monsterSelector.NextMonster(Player.Level);

        tile.ClearAndDropTileAs(newTileType);
    }
}
