using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public delegate void ActorTilesDelegate(List<GameTile> t);

public class GridGameManager : MonoBehaviour
{
    public NoParamDelegate OnRoundEnd;

    GameController_DDOL _gc;
    LineRenderer _lr;
    GridInputManager _gim;

    bool RoundHasEnded = false;
    public GameBoard Board;

    [SerializeField] GameObject dmgPrefab;
    [SerializeField] GameObject hpPrefab;
    [SerializeField] GameObject apPrefab;
    [SerializeField] GameObject gpPrefab;
    [SerializeField] GameObject xpPrefab;
    [SerializeField] GameObject floaterParent;
    [SerializeField] Transform floaterPositionRef;
    [SerializeField] Transform SelectionCountDoodad;
    [SerializeField] GameObject ItemShopMenu;
    [SerializeField] GameObject EnchantmentShopMenu;

    public void SelectItemShopAtIndex(int index) {
        Board.ItemShopPurchase(index);
    }

    public void SelectionEnchantmentShopOptionAtIndex(int index) {
        Board.EnchantmentShopPurchase(index);
    }

    void FloatExp(int xp) {
        var go = Instantiate(
            xpPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + xp + " XP";
    }
    void FloatDamage(int dmg)
    {
        var go = Instantiate(
            dmgPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "-" + dmg + " HP";
    }
    void FloatHeal(int dmg)
    {
        var go = Instantiate(
            hpPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " HP";
    }
    void FloatArmor(int dmg)
    {
        var go = Instantiate(
            apPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " AP";
    }
    void FloatGold(int dmg)
    {
        var go = Instantiate(
            gpPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " GP";
    }

    void Awake() {
        _lr = gameObject.GetComponent<LineRenderer>();
    }

    void Start()
    {
        _gc = FindObjectOfType<GameController_DDOL>();
        _gim = gameObject.GetComponent<GridInputManager>();
        Board = new GameBoard();

        foreach (Tile tile in Board.Tiles) {
            GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
            GameObject go = Instantiate(
                tilePrefab,
                tile.GridPosition(),
                Quaternion.identity
            );
            GameTile gt = go.GetComponent<GameTile>();
            gt.AttachToTile(tile);
            _gim.AttachTileToGrid(gt);
        }
        Board.OnPlayerCollectedTiles += HandlePlayerCollectedTiles;
        Board.OnTileAddedToSelection += HandleTileAddedToSelection;
        Board.OnSelectionChange += HandleSelectionChange;
        Board.OnMonstersAttack += HandleMonstersAttack;
        Board.OnCoinCollected += HandleCoinCollected;
        Board.OnPotionsCollected += HandlePotionsCollected;
        Board.OnShieldsCollected += HandleShieldsCollected;
        Board.OnSwordsCollected += HandleSwordsCollected;
        Board.OnEnemyStunned += HandleMonsterStunned;
        Board.OnMonsterKillsEarned += HandleMonsterKillEarned;
        Board.OnExperienceGained += HandleExperienceGained;
        Board.OnLose += HandleLose;
        Board.OnReadyForNextTurn += HandleReadyForNextTurn;
        _gim.OnUserDragIndicatingTile += Board.UserIndicatingTile;
        _gim.OnUserStartSelection += Board.UserStartSelection;
        _gim.OnUserEndSelection += Board.UserEndSelection;
        Board.OnGoldGoalReached += HandleGoldGoalReached;
        Board.OnDefenseGoalReached += HandleDefenseGoalReached;
        Board.OnExperienceGoalReached += HandleExperienceGoalReached;
    }

    void OnDestroy() {
        if (Board == null) return;
        
        Board.OnPlayerCollectedTiles -= HandlePlayerCollectedTiles;
        Board.OnTileAddedToSelection -= HandleTileAddedToSelection;
        Board.OnSelectionChange -= HandleSelectionChange;
        Board.OnMonstersAttack -= HandleMonstersAttack;
        Board.OnCoinCollected -= HandleCoinCollected;
        Board.OnPotionsCollected -= HandlePotionsCollected;
        Board.OnShieldsCollected -= HandleShieldsCollected;
        Board.OnSwordsCollected -= HandleSwordsCollected;
        Board.OnEnemyStunned -= HandleMonsterStunned;
        Board.OnMonsterKillsEarned -= HandleMonsterKillEarned;
        Board.OnExperienceGained -= HandleExperienceGained;
        Board.OnLose -= HandleLose;
        Board.OnReadyForNextTurn -= HandleReadyForNextTurn;
        Board.OnGoldGoalReached -= HandleGoldGoalReached;
        Board.OnDefenseGoalReached -= HandleDefenseGoalReached;
        Board.OnExperienceGoalReached -= HandleExperienceGoalReached;
    }

    void HandleExperienceGained(int exp) {
      //  Debug.Log($"exp gained: {exp}");
        FloatExp(exp);
    }

    void HandleExperienceGoalReached() {
      //  Debug.Log("HIT XP GOAL");
    }

    void HandleGoldGoalReached() {
        ItemShopMenu.SetActive(true);
    }

    void HandleDefenseGoalReached() {
        EnchantmentShopMenu.SetActive(true);
    }


    bool isActingRightNow() {
        return false;
    }

    IEnumerator StartNextRoundWhenDoneActing() {
        while (isActingRightNow()) {
            yield return new WaitForSeconds(0.2f);
        }
        
        Board.RoundProceed();
    }

    void HandleReadyForNextTurn() {
        StartCoroutine("StartNextRoundWhenDoneActing");
    }

    void HandlePlayerCollectedTiles(List<Tile> tiles) {

    }

    void HandleMonstersAttack(DamageResult playerTookDamage) {
        if (RoundHasEnded) return;
        int random = Random.Range(1, 4);
        _gc.PlaySound("Monster_Hit_" + random);
        FloatDamage(playerTookDamage.Attempted);
    }

    void HandleSelectionChange(List<Tile> selection) {
        _lr.positionCount = selection.Count;
        _lr.SetPositions(
            selection.Select((o) => o.GridPosition()).ToArray()
        );

        if (selection.Count == 0) {
            SelectionCountDoodad.gameObject.SetActive(false);
        } else {
            var doodadOffset = new Vector3(.5f, -.25f, 0);
            SelectionCountDoodad.gameObject.SetActive(true);
            SelectionCountDoodad.position = selection.ElementAt(selection.Count - 1).GridPosition() - doodadOffset;
            
            var circleText = selection.Where((o) => o.tileType != TileType.Monster).ToList().Count + "";
            int swordCount = selection.Where((o) => o.tileType == TileType.Sword).ToList().Count;
            int potionCount = selection.Where((o) => o.tileType == TileType.Potion).ToList().Count;
            int shieldCount = selection.Where((o) => o.tileType == TileType.Shield).ToList().Count;

            if (swordCount > 0) {
                circleText = $"{Board.Player.CalcDamageDone(swordCount)}D";
            }

            if (potionCount > 0) {
                circleText = $"{Board.Player.CalcHealingDone(potionCount)}H";
            }

            if (shieldCount > 0) {
                circleText = $"{Board.Player.CalcArmorGained(shieldCount)}A";
            }
            
            SelectionCountDoodad.GetComponent<DOODAD_SelectionCount>().SetText(circleText);
        }
    }

    void HandleTileAddedToSelection(Tile tile) {
        switch (tile.tileType) {
            case TileType.Coin:
                _gc.PlaySound("Coin_Select");
                break;
            case TileType.Potion:
                _gc.PlaySound("Potion_Select");
                break;
            case TileType.Shield:
                _gc.PlaySound("Shield_Select");
                break;

            default:
                _gc.PlaySound("Sword_Select");
                break;
        }
    }

    void HandleLose(StatSheet s) {
        _gc.PreviousRoundStats = s;
        DoLose();
    }
    void HandleMonsterKillEarned(int countKilled) {
       // Debug.Log("MONO SAW YOU KILL " + countKilled + " monsters");
    }
    void HandleMonsterStunned() {
        _gc.PlaySound("Sword_Hit");
    }
    void HandleCoinCollected(int amt) {
        _gc.PlaySound("Coin_Collect");
        FloatGold(amt);
    }

    void HandlePotionsCollected(int amt) {
        _gc.PlaySound("Potion_Use");
        FloatHeal(amt);
    }

    void HandleShieldsCollected(int amt) {
        _gc.PlaySound("Shield_Use");
        FloatArmor(amt);
    }

    void HandleSwordsCollected(int amt) {
        _gc.PlaySound("Sword_Hit");
    }

    void DoLose()
    {
        if (RoundHasEnded) return;
        RoundHasEnded = true;
        OnRoundEnd?.Invoke();
        StartCoroutine("LoseRoutine");
    }

    IEnumerator LoseRoutine()
    {
        _gc.PreviousRoundMoves = Board.MovesMade;
        yield return new WaitForSeconds(0.2f);
        _gc.ChangeScene("GameOver");
    }
}
