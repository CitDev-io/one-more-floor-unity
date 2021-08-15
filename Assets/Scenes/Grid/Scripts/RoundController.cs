using citdev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public delegate void NoParamDelegate();

public class RoundController : MonoBehaviour
{
    public NoParamDelegate OnRoundEnd;

    [SerializeField] public int HitPoints;
    [SerializeField] public int Armor;
    [SerializeField] public int Kills = 0;

    GameController_DDOL _gc;

    bool roundEnded = false;

    int enemyHp = 4;
    int enemyDmg = 2;

    int turn = 1;
    int round = 1;
    public int KillRequirement = 15;
    public int tilesCleared = 0;
    public int RoundMoves = 0;
    [SerializeField] GameObject dmgPrefab;
    [SerializeField] GameObject hpPrefab;
    [SerializeField] GameObject apPrefab;
    [SerializeField] GameObject gpPrefab;
    [SerializeField] GameObject floaterParent;

    void FloatDamage(int dmg)
    {
        var go = Instantiate(dmgPrefab, floaterParent.transform);
        go.GetComponent<TextMeshProUGUI>().text = "-" + dmg + " HP";
    }
    void FloatHeal(int dmg)
    {
        var go = Instantiate(hpPrefab, floaterParent.transform);
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " HP";
    }
    void FloatArmor(int dmg)
    {
        var go = Instantiate(apPrefab, floaterParent.transform);
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " AP";
    }
    void FloatGold(int dmg)
    {
        var go = Instantiate(gpPrefab, floaterParent.transform);
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " GP";
    }

    private void Start()
    {
        _gc = FindObjectOfType<GameController_DDOL>();

        _gc.round += 1;
        round = _gc.round;
        SetEnemyStatsByRound();
        SetupCharacterForRound();
    }

    void SetupCharacterForRound()
    {
        HitPoints = 15;
    }

    void SetEnemyStatsByRound()
    {

        enemyHp = Mathf.Min((int) Mathf.Ceil(round / 3f) + 1, 4);
        enemyDmg = Mathf.Min((int) Mathf.Ceil(round / 4f), 3);
    }

    void AssessAttack(int damage)
    {
        if (Armor >= damage)
        {
            Armor -= damage;
            return;
        }

        int remainingDmg = damage - Armor;
        if (Armor > 0)
        {
            ApplyArmorChange(-Armor);
        }

        ApplyHpChange(-remainingDmg);
    }

    void ApplyHpChange(int changeAmount)
    {
        HitPoints = Mathf.Clamp(HitPoints + changeAmount, 0, 15);

        if (HitPoints == 0)
        {
            // (e) PLAYER DIED
            DoLose();
        }
    }

    void ApplyArmorChange(int changeAmount)
    {
        Armor = Mathf.Clamp(Armor + changeAmount, 0, 10);
    }

    public void PlayerCollectedTiles(List<GameTile> collected, BoardController board)
    {
        RoundMoves++;

        int healthGained = collected
            .Where((o) => o.tileType == TileType.Heart)
            .Aggregate(0, (acc, cur) => acc + cur.Power);

        int armorGained = collected
            .Where((o) => o.tileType == TileType.Shield)
            .Aggregate(0, (acc, cur) => acc + cur.Power);

        int coinMultiplier = 1;

        List<GameTile> coinsCollected = collected.Where((o) => o.tileType == TileType.Coin).ToList();

        if (coinsCollected.Count > 0) {
            _gc.PlaySound("Coin_Collect");
        }

        int coinGained = collected
            .Where((o) => o.tileType == TileType.Coin)
            .Aggregate(0, (acc, cur) => acc + cur.Power) * coinMultiplier;

        if (coinsCollected.Count > 0)
        {
            _gc.PlaySound("Coin_Collect");
            FloatGold(coinGained);
        }

        int damageDealt = collected
            .Where((o) => o.tileType == TileType.Sword)
            .Aggregate(0, (acc, cur) => acc + cur.Power);

        List<GameTile> enemies = collected
            .Where((o) => o.tileType == TileType.Monster).ToList();

        List<GameTile> clearableTiles = collected
            .Where((o) => o.tileType != TileType.Monster).ToList();

        if (healthGained != 0)
        {
            ApplyHpChange(healthGained);
            _gc.PlaySound("Heart_Use");
            FloatHeal(healthGained);
        }
        if (armorGained != 0)
        {
            ApplyArmorChange(armorGained);
            _gc.PlaySound("Shield_Use");
            FloatArmor(armorGained);
        }
        
        _gc.CoinBalanceChange(coinGained);

        if (enemies.Count > 0)
        {
            _gc.PlaySound("Sword_Hit");
        }

        foreach (GameTile monster in enemies)
        {
            // (e) : HITTING A MONSTER
            monster.HitPoints -= damageDealt;
            if (monster.HitPoints <= 0)
            {
                // (e) : MONSTER DIED
                clearableTiles.Add(monster);
                OnMonsterKill();
            } else
            {
                // (e) : MONSTER SURVIVED ATTACK
                monster.label1.text = monster.HitPoints + "";
            }
        }

        board.ClearTiles(clearableTiles);
        // (e) : FINISHED RESOLVING USER COLLECTION
        DoEnemiesTurn();
    }

    void OnMonsterKill()
    {
        Kills += 1;
        _gc.OnMonsterKilled();
        if (Kills >= KillRequirement)
        {
            DoVictory();
        }
    }

    void DoLose()
    {
        StartCoroutine("LoseRoutine");
    }

    IEnumerator LoseRoutine()
    {
        FindObjectOfType<BoardController>()?.ToggleTileFreeze(true);
        _gc.PreviousRoundMoves = RoundMoves;
        yield return new WaitForSeconds(0.2f);
        _gc.ChangeScene("GameOver");
    }

    void DoVictory()
    {
        roundEnded = true;
        OnRoundEnd?.Invoke();
        StartCoroutine("RoundVictory");
    }

    IEnumerator RoundVictory()
    {
        _gc.PreviousRoundMoves = RoundMoves;
        yield return new WaitForSeconds(3f);
        _gc.ChangeScene("RoundScore");
    }

    void DoEnemiesTurn()
    {
        if (roundEnded) return;

        // deal damage to player for existing monsters
        var monsters = GameObject.FindObjectOfType<BoardController>().GetMonsters()
            .Where((o) => o.TurnAppeared < turn).ToList();

        int damageReceived = monsters
                .Aggregate(0, (acc, cur) => acc + cur.Power);

        if (monsters.Count > 0)
        {
            int random = Random.Range(1, 4);
            _gc.PlaySound("Monster_Hit_" + random);
            FloatDamage(damageReceived);
        }
        AssessAttack(damageReceived);
        FindObjectOfType<BoardController>()?.EnemyIconsTaunt();
        turn += 1;
    }

    TileType GetNextTile()
    {
        tilesCleared += 1;

        if (tilesCleared > 40 && tilesCleared % 8 == 0)
        {
            return TileType.Monster;
        }

        return GetRandomTile();
    }

    TileType GetRandomTile()
    {
        int tileChoice = Random.Range(0, 4);
        return (TileType)tileChoice;
    }

    void PrepNewTile(GameTile tile)
    {
        switch (tile.tileType) {
            case TileType.Coin:
                tile.Power = 10;
                tile.HitPoints = 0;
                break;
            case TileType.Heart:
                tile.Power = 1;
                tile.HitPoints = 0;
                break;
            case TileType.Shield:
                tile.Power = 1;
                tile.HitPoints = 0;
                break;
            case TileType.Sword:
                tile.Power = 1;
                tile.HitPoints = 0;
                break;
            case TileType.Monster:
                tile.Power = enemyDmg;
                tile.HitPoints = enemyHp;
                break;
            default:
                tile.Power = 0;
                tile.HitPoints = 0;
                break;
        }
        tile.label1.text = tile.HitPoints > 0 ? tile.HitPoints + "" : "";
        tile.label2.text = tile.tileType == TileType.Monster ? tile.Power + "" : "";
        tile.TurnAppeared = turn;
    }

    public void ConvertTileToSword(GameTile tile)
    {
        PrepNewTile(tile);
    }

    // should not be called locally! board needs to cascade guys above
    public void RecycleTileForPosition(GameTile tile, Vector2 position)
    {
        tile.SetTileType(GetNextTile());
        tile.SnapToPosition(position.x, 7);
        tile.AssignPosition(position);
        PrepNewTile(tile);
    }
}
