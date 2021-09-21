using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public delegate void TilesDelegate(List<Tile> t);
public delegate void TileDelegate(Tile t);
public delegate void IntDelegate(int i);
public delegate void NoParamDelegate();
public delegate void StatSheetDelegate(StatSheet statSheet);
public delegate void SourcedDamageDelegate(int damage, DamageSource source);

public class GameBoard
{
    public TilesDelegate OnPlayerCollectedTiles;
    public TileDelegate OnTileAddedToSelection;
    public TilesDelegate OnSelectionChange;
    public IntDelegate OnMonstersAttack;
    public IntDelegate OnCoinCollected;
    public IntDelegate OnHeartsCollected;
    public IntDelegate OnShieldsCollected;
    public IntDelegate OnPoisonCollected;
    public IntDelegate OnSwordsCollected;
    public NoParamDelegate OnEnemyStunned;
    public NoParamDelegate OnMonsterKillEarned;
    public StatSheetDelegate OnWin;
    public StatSheetDelegate OnLose;
    public NoParamDelegate OnReadyForNextTurn;
    public List<Tile> Tiles = new List<Tile>();
    BoardContext ctx;
    public StatSheet Player;
    public int Kills { get; private set; } = 0;
    public int KillRequirement = 15;
    public int MovesMade = 0;
    int EnemyHp = 1;
    int EnemyDmg = 1;
    bool BoardComplete = false;
    bool AwaitingRoundChange = false;

    List<Tile> selection = new List<Tile>();

    TileSelector tileSelector;
    ChainValidator chainValidator;

    public GameBoard(BoardContext bctx) {
        ctx = bctx;
        SetEnemyStatsByRound(bctx.Stage);
        tileSelector = new TileSelector(bctx.PC.TileOptions);

        int COUNT_ROWS = 6;
        int COUNT_COLS = 6;

        for (var rowid = 0; rowid < COUNT_ROWS; rowid++) {
            for (var colid = 0; colid < COUNT_COLS; colid++) {
                Tile t = new Tile(
                    colid,
                    rowid,
                    EnemyHp,
                    EnemyDmg,
                    tileSelector.GetNextTile()
                );
                Tiles.Add(t);
            }
        }

        switch(CurrentCharacterType()) {
            case CharacterType.Warrior:
                chainValidator = new WarriorChainValidator(
                    Tiles,
                    selection
                );
                break;
            case CharacterType.Rogue:
                chainValidator = new RogueChainValidator(
                    Tiles,
                    selection
                );
                break;
            default:
                Debug.Log("NOT ESTABLISHED WHAT CLASS YOU ARE");
                break;
        }
        Player = bctx.PC.GetStatSheet();
    }

    public CharacterType CurrentCharacterType() {
        switch(ctx.PC.Name.ToLower()) {
            case "warrior":
                return CharacterType.Warrior;
            case "rogue":
                return CharacterType.Rogue;
            default:
                return CharacterType.Warrior;
        }
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

    void ApplyHpChange(int changeAmount)
    {
        Player.ApplyHP(changeAmount);

        if (Player.Hp == 0)
        {
            OnLose?.Invoke(Player);
        }
    }

    void ApplyPoisonChanged(int changeAmount) {
        Player.ApplySP(changeAmount);
    }

    void ApplyArmorChange(int changeAmount)
    {
        Player.ApplySP(changeAmount);
    }

    void SetEnemyStatsByRound(int round)
    {

        EnemyHp = Mathf.Min((int) Mathf.Ceil(round / 3f) + 1, 4);
        EnemyDmg = Mathf.Min((int) Mathf.Ceil(round / 4f), 3);
    }

    void DoPhase_Collection() {
        CollectTiles();

        DoPhase_Post();
    }

    void DoPhase_Post() {
        var monsters = Tiles.Where((o) => o.tileType == TileType.Monster && o.isAlive());
        if (ctx.PC.Name.ToLower() == "rogue" && Player.Sp > 0 && monsters.Any()) {
            int sprayCount = Mathf.Min(3, Player.Sp);
            Debug.Log(sprayCount + " sprays");

            var spraysDone = 0;
            for(var i = 0; i < sprayCount; i++) {
                if (monsters.Count() == 0) continue;

                int indexChosen = Random.Range(0, monsters.Count());
                var monster = monsters.ElementAt(indexChosen);

                monster.TakeDamage(1, DamageSource.PoisonDart);
                CheckIfMonsterDied(monster);
                spraysDone++;
            }
            Player.ApplySP(-spraysDone);
        }

        DoPhase_Monsters();
    }

    public void RoundProceed()
    {
        Debug.Log("TRY TO PROCEED");
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
        ClearTiles(selection);
        AwaitingRoundChange = true;
        OnReadyForNextTurn?.Invoke();
    }

    void DoPhase_Populate() {
        Debug.Log("POPULATE");
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

        int damageReceived = monsters.Sum((o) => o.Damage);
        
        if (damageReceived == 0) return;
        OnMonstersAttack?.Invoke(damageReceived);
        foreach(Tile monster in monsters) {
            monster.DoAttack();
        }
        
        if (CurrentCharacterType() == CharacterType.Warrior) {
            if (Player.Sp >= damageReceived)
            {
                ApplyArmorChange(-damageReceived);
                return;
            }

            int remainingDmg = damageReceived - Player.Sp;
            if (Player.Sp > 0)
            {
                ApplyArmorChange(-Player.Sp);
            }

            ApplyHpChange(-remainingDmg);
        } else {
            ApplyHpChange(-damageReceived);
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
        OnSelectionChange?.Invoke(selection);
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
            OnTileAddedToSelection?.Invoke(tile);
            OnSelectionChange?.Invoke(selection);
        }
    }

    void HandleUserReindicatingTile(Tile tile)
    {
        int index = selection.IndexOf(tile);
        List<Tile> tilesToUnhighlight = selection.GetRange(index + 1, selection.Count - index - 1);
        foreach (Tile t in tilesToUnhighlight)
        {
            selection.Remove(t);
            OnSelectionChange?.Invoke(selection);
        }
    }

    void CollectTiles()
    {
        MovesMade += 1;

        int healthGained = selection
            .Where((o) => o.tileType == TileType.Heart)
            .ToList().Count;

        int armorGained = selection
            .Where((o) => o.tileType == TileType.Shield)
            .ToList().Count;

        int poisonGained = selection
            .Where((o) => o.tileType == TileType.Poison)
            .ToList().Count;

        List<Tile> coinsCollected = selection.Where((o) => o.tileType == TileType.Coin).ToList();

        int coinGained = selection
            .Where((o) => o.tileType == TileType.Coin)
            .ToList().Count * 10;

        if (coinGained > 0)
        {
            OnCoinCollected?.Invoke(coinGained);
        }

        int swordsCollected = selection
            .Where((o) => o.tileType == TileType.Sword)
            .ToList().Count;
        
        if (swordsCollected > 0) {
            Player.ApplySwords(swordsCollected);
        }

        List<Tile> enemies = selection
            .Where((o) => o.tileType == TileType.Monster).ToList();

        if (healthGained != 0)
        {
            ApplyHpChange(healthGained);
            OnHeartsCollected?.Invoke(healthGained);
        }
        if (armorGained != 0)
        {
            ApplyArmorChange(armorGained);
            OnShieldsCollected?.Invoke(armorGained);
        }

        if (poisonGained != 0) {
            ApplyPoisonChanged(poisonGained);
            OnPoisonCollected?.Invoke(poisonGained);
        }

        if (swordsCollected > 0)
        {
            OnSwordsCollected?.Invoke(swordsCollected);
        }

        if (enemies.Count > 0 && armorGained > 0) {
            OnEnemyStunned?.Invoke();
        }

        int damageDealt = swordsCollected * Player.Damage;

        foreach (Tile monster in enemies)
        {
            monster.TakeDamage(damageDealt, DamageSource.SwordAttack);
            if (armorGained > 0) {
                monster.Stun();
            }
            CheckIfMonsterDied(monster);
        }

        OnPlayerCollectedTiles?.Invoke(selection);
    }

    void CheckIfMonsterDied(Tile monster) {
        if (!monster.isAlive()) {
            OnMonsterKill();
        }
    }
    void OnMonsterKill()
    {
        Kills += 1;
        OnMonsterKillEarned?.Invoke();
        if (Kills >= KillRequirement)
        {
            BoardComplete = true;
            OnWin?.Invoke(Player);
        }
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
