using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public delegate void TilesDelegate(List<Tile> t);
public delegate void TileDelegate(Tile t);
public delegate void IntDelegate(int i);
public delegate void NoParamDelegate();
public class GameBoard
{
    public TilesDelegate OnPlayerCollectedTiles;
    public TileDelegate OnTileAddedToSelection;
    public TilesDelegate OnSelectionChange;
    public IntDelegate OnMonstersAttack;
    public IntDelegate OnCoinCollected;
    public IntDelegate OnHeartsCollected;
    public IntDelegate OnShieldsCollected;
    public IntDelegate OnSwordsCollected;
    public NoParamDelegate OnEnemyStunned;
    public NoParamDelegate OnMonsterKillEarned;
    public NoParamDelegate OnWin;
    public NoParamDelegate OnLose;

    public List<Tile> Tiles = new List<Tile>();
    BoardContext ctx;
    public int HitPoints { get; private set; } = 1;
    public int MaxHitPoints { get; private set; } = 1;
    public int Armor { get; private set; } = 0;
    public int MaxArmor { get; private set; } = 0;
    public int Kills { get; private set; } = 0;
    public int KillRequirement = 15;
    public int MovesMade = 0;
    int EnemyHp = 1;
    int EnemyDmg = 1;

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

        switch(bctx.PC.Name.ToLower()) {
            case "warrior":
                chainValidator = new WarriorChainValidator(
                    Tiles,
                    selection
                );
                break;
            case "rogue":
                chainValidator = new RogueChainValidator(
                    Tiles,
                    selection
                );
                break;
            default:
                Debug.Log("NOT ESTABLISHED WHAT CLASS YOU ARE");
                break;
        }

        HitPoints = bctx.PC.HitPoints;
        MaxHitPoints = bctx.PC.MaxHitPoints;
        Armor = bctx.PC.SpecialPoints;
        MaxArmor = bctx.PC.MaxSpecialPoints;
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
            ExecuteUserTurn();
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
        HitPoints = Mathf.Clamp(HitPoints + changeAmount, 0, MaxHitPoints);

        if (HitPoints == 0)
        {
            OnLose?.Invoke();
        }
    }

    void ApplyArmorChange(int changeAmount)
    {
        Armor = Mathf.Clamp(Armor + changeAmount, 0, MaxArmor);
    }

    void SetEnemyStatsByRound(int round)
    {

        EnemyHp = Mathf.Min((int) Mathf.Ceil(round / 3f) + 1, 4);
        EnemyDmg = Mathf.Min((int) Mathf.Ceil(round / 4f), 3);
    }

    void ExecuteUserTurn() {
        CollectTiles(selection);
        var monsters = Tiles.Where((o) => o.tileType == TileType.Monster
            && o.wasAliveBeforeLastUserAction()
            && !o.isStunned()
        ).ToList();
        MonstersAttack(monsters);
        
        AgeAllMonsters();
    }

    void MonstersAttack(List<Tile> monsters) {
        int damageReceived = monsters.Sum((o) => o.Damage);
        
        if (damageReceived == 0) return;
        OnMonstersAttack?.Invoke(damageReceived);
        
        
        if (Armor >= damageReceived)
        {
            ApplyArmorChange(-damageReceived);
            return;
        }

        int remainingDmg = damageReceived - Armor;
        if (Armor > 0)
        {
            ApplyArmorChange(-Armor);
        }

        ApplyHpChange(-remainingDmg);
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
        OnSelectionChange?.Invoke(selection);
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

    void CollectTiles(List<Tile> collected)
    {
        MovesMade += 1;

        int healthGained = collected
            .Where((o) => o.tileType == TileType.Heart)
            .ToList().Count;

        int armorGained = collected
            .Where((o) => o.tileType == TileType.Shield)
            .ToList().Count;

        List<Tile> coinsCollected = collected.Where((o) => o.tileType == TileType.Coin).ToList();

        int coinGained = collected
            .Where((o) => o.tileType == TileType.Coin)
            .ToList().Count * 10;

        if (coinGained > 0)
        {
            OnCoinCollected?.Invoke(coinGained);
        }

        int damageDealt = collected
            .Where((o) => o.tileType == TileType.Sword)
            .ToList().Count;

        List<Tile> enemies = collected
            .Where((o) => o.tileType == TileType.Monster).ToList();

        List<Tile> clearableTiles = collected
            .Where((o) => o.tileType != TileType.Monster).ToList();

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

        if (enemies.Count > 0 && damageDealt > 0)
        {
            OnSwordsCollected?.Invoke(damageDealt);
        }

        if (enemies.Count > 0 && armorGained > 0) {
            OnEnemyStunned?.Invoke();
        }

        foreach (Tile monster in enemies)
        {
            monster.TakeDamage(damageDealt);
            if (!monster.isAlive())
            {
                clearableTiles.Add(monster);
                OnMonsterKill();
            }

            if (armorGained > 0) {
                monster.Stun();
            }
        }

        OnPlayerCollectedTiles?.Invoke(collected);
        ClearTiles(collected);
    }

    void OnMonsterKill()
    {
        Kills += 1;
        OnMonsterKillEarned?.Invoke();
        if (Kills >= KillRequirement)
        {
            OnWin?.Invoke();
        }
    }

    void ClearTiles(List<Tile> clearedTiles)
    {
        clearedTiles.OrderByDescending(o => o.row);
        foreach(Tile t in clearedTiles)
        {
            if (t.tileType == TileType.Monster && t.HitPoints > 0) {
                continue;
            }
            RecascadeTile(t);
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
