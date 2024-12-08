using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public delegate void ActorTilesDelegate(List<GameTile> t);

public class GridGameManager : MonoBehaviour
{
    public NoParamDelegate OnRoundEnd;

    GameController_DDOL _gc;
    LineRenderer _lr;
    GridInputManager _gim;
    List<GameTile> GameTiles = new List<GameTile>();
    bool RoundHasEnded = false;
    public TileGame Board;

    [SerializeField] GameObject dmgPrefab;
    [SerializeField] GameObject hpPrefab;
    [SerializeField] GameObject apPrefab;
    [SerializeField] GameObject gpPrefab;
    [SerializeField] GameObject xpPrefab;
    [SerializeField] GameObject floaterParent;
    [SerializeField] Transform floaterPositionRef;
    [SerializeField] Transform SelectionCountDoodad;
    [SerializeField] GameObject ItemShopMenu;
    [SerializeField] AnimationCurve curve;

    Queue<TileGameEvent> _eventQueue = new Queue<TileGameEvent>();
    // [SerializeField] GameObject EnchantmentShopMenu;
    // [SerializeField] GameObject XPShopMenu;

    public void SelectItemShopAtIndex(int index) {
        Debug.Log("ITEM");
        Board.State.ItemShopPurchase(index);
    }

    // public void SelectionEnchantmentShopOptionAtIndex(int index) {
    //     Debug.Log("ENCHANTMENT");
    //     Board.EnchantmentShopPurchase(index);
    // }

    // public void SelectionXPShopOptionsAtIndexes(List<int> indexes) {
    //     Debug.Log("XP STUFF");
    //     Board.LevelUpPurchase(indexes);
    // }

    void FloatExp(int xp) {
        var go = Instantiate(
            xpPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + xp + " XP";
    }
    void FloatDamage(int dmg, Vector2 spot)
    {
        var go = Instantiate(
            dmgPrefab,
            spot,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = dmg + " dmg";
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

        Board = new TileGame();

        foreach (Tile tile in Board.State.Tiles.TileList) {
            GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
            GameObject go = Instantiate(
                tilePrefab,
                tile.GridPosition(),
                Quaternion.identity
            );
            GameTile gt = go.GetComponent<GameTile>();
            gt.AttachToTile(tile);
            _gim.AttachTileToGrid(gt);
            GameTiles.Add(gt);
        }

        // even if its a simple call through, ALWAYS wrap this in a local handler
        _gim.OnUserDragIndicatingTile += HandleUserIndicatingTile;
        _gim.OnUserStartSelection += HandleUserStartSelection;
        _gim.OnUserEndSelection += HandleUserEndSelection;


        Board.Events.OnDebugLog += HandleDebugLog;
        Board.Events.OnRawEvent += HandleRawEvent;
        StartCoroutine(AnimationEventQueue());
    }

    IEnumerator AnimationEventQueue() {
        while (true) {
            if (_eventQueue.Count > 0) {
                TileGameEvent gameEvent = _eventQueue.Dequeue();
                yield return StartCoroutine(DoPerformance(gameEvent));
            }
            yield return null;
        }
    }

    // Vector3 DockedHeroLeft = new Vector3(-5.5f, 1.87f, 0);
    // Vector3 OnStageHero = new Vector3(0, 1.87f, 0);
    // IEnumerator SlideHeroIn() {
    //     GameObject.Find("Adventurer").transform.position = DockedHeroLeft;

    //     while (Vector3.Distance(GameObject.Find("Adventurer").transform.position, OnStageHero) > 0.1f) {
    //         GameObject.Find("Adventurer").transform.position = Vector3.Lerp(GameObject.Find("Adventurer").transform.position, OnStageHero, Time.deltaTime * 20f);
    //         yield return new WaitForSeconds(0.01f);
    //     }
    // }

    // IEnumerator SlideHeroOut() {
    //     GameObject.Find("Adventurer").transform.position = OnStageHero;

    //     while (Vector3.Distance(GameObject.Find("Adventurer").transform.position, DockedHeroLeft) > 0.1f) {
    //         GameObject.Find("Adventurer").transform.position = Vector3.Lerp(GameObject.Find("Adventurer").transform.position, DockedHeroLeft, Time.deltaTime * 20f);
    //         yield return new WaitForSeconds(0.01f);
    //     }
    // }

    // take as a parameter a method passed in as a callback
    IEnumerator PerformMonstersAttack(DamageResult damageResult) {
        // todo: better filter
        List<GameTile> NonContenders = GameTiles.FindAll((tile) => tile._tile.tileType != TileType.Monster || tile._tile.TurnsAlive < 3);
        // StartCoroutine(SlideHeroIn());

        // float alpha = 1;
        // while (alpha > 0.1f) {
        //     alpha -= 0.1f;
        //     foreach (GameTile tile in NonContenders) {
        //         tile.SetIconChildAlpha(alpha);
        //     }
        // }
        Coroutine last = null;
        foreach (GameTile tile in NonContenders) {
            last = StartCoroutine(tile.FadeToOpacity(0.23f));
        }
        if (last != null) yield return last;

        yield return new WaitForSeconds(0.1f);

        // TODO instantiate if it doesn't exist. orchestrator stuff.
        var DmgCounter = GameObject
            .Find("MonstersAttackDamageCounter")
            .GetComponent<UI_MonstersAttackDmgCounter>();
        DmgCounter.Reset();
        DmgCounter.Show();
        
        float msBetweenAttacks = 0.08f;
        foreach (GameTile tile in GameTiles) {
            if (tile._tile.tileType == TileType.Monster && tile._tile.TurnsAlive > 2) {
                yield return StartCoroutine(tile.DoAttackAnimation(() => {
                    Vector2 tilePositionOnCanvas = Camera.main.WorldToScreenPoint(tile.transform.position);
                    int dmgDealt = tile._tile.CurrentMonster.CalcBaseDamage();
                    FloatDamage(dmgDealt, tilePositionOnCanvas);
                    _gc.PlaySound("Monster_Hit_" + UnityEngine.Random.Range(1, 4));
                    DmgCounter.AddDamage(dmgDealt);
                }));
                yield return new WaitForSeconds(msBetweenAttacks);
                StartCoroutine(tile.FadeToOpacity(0.23f));
            }
        }
        var ShieldCounter = GameObject
            .Find("MonstersAttackPlayerShieldCounter")
            .GetComponent<UI_MonstersAttackPlayerShieldCounter>();
        
        ShieldCounter.Reset();
        ShieldCounter.AdjustShields(Math.Max(Board.State.Player.Armor, damageResult.AssignedToArmor));
        ShieldCounter.Show();
        yield return new WaitForSeconds(1f);

        ShieldCounter.MoveToBonkIntercept();
        yield return new WaitForSeconds(headstart_delay);
        bool SkippingIntercept = damageResult.AssignedToHitPoints > 0 && damageResult.AssignedToHitPoints == damageResult.Attempted;
        if (SkippingIntercept) {
            ShieldCounter.AdjustShields(Board.State.Player.Armor);
            DmgCounter.SetDamage(damageResult.AssignedToHitPoints);
            //fade the shield out

            yield return DmgCounter.OffTheTopRope();
            Camera.main.GetComponent<CameraShake>().shakeDuration = 0.369f;
        } else {
            yield return DmgCounter.MoveToBonkIntercept();

            Debug.Log("Flying text with armor evaluation"); // show pierced, recovered, lost armor
            ShieldCounter.AdjustShields(Board.State.Player.Armor);
            DmgCounter.SetDamage(damageResult.AssignedToHitPoints);
            
            if (damageResult.AssignedToHitPoints == 0) {
                // bounce backwards for attacker
                yield return DmgCounter.BounceBack();
                Debug.Log("Shoot off armor stats");
                // do this

            } else {
                Debug.Log("ATTACK!!");
                ShieldCounter.Drift(
                    new Vector2(-20f, 15f),
                    1.5f
                );
                yield return DmgCounter.Drift(new Vector2(0f, 15f), 0.369f);
                yield return DmgCounter.AttackRun();
                Camera.main.GetComponent<CameraShake>().shakeDuration = 0.369f;
            }
        }

        

        yield return new WaitForSeconds(cool_off_delay);
        Debug.Log("Roll out hero, fade in tiles");
        DmgCounter.Hide();
        ShieldCounter.Hide();
        // StartCoroutine(SlideHeroOut());

        foreach (GameTile tile in GameTiles) {
            // can yield until last is completed after this loop if you have more to do
            last = StartCoroutine(tile.FadeToOpacity(1f));
        }
        // number updates will come from the subsequent events running
    }
    public float headstart_delay = .25f;
    public float cool_off_delay = .5f;

    IEnumerator DoPerformance(TileGameEvent gameEvent) {
        // should ship off to  a coroutine for a performance, but in the end, we need to chirp the event
        // to UI land and exit
        yield return null;

        switch (gameEvent) {
                case MonsterTurnEvent monsterTurnEvent:
                yield return null;
                break;
                case MonstersAttackEvent monstersAttackEvent:
                // can time the monsters attack event going public timing if using CB
                //Action callback = () => HandlePlayerCollectedTiles(playerCollectedTilesEvent.Tiles);
                // yield return StartCoroutine(PerformMonstersAttack(callback));
                yield return StartCoroutine(PerformMonstersAttack(monstersAttackEvent.Result));

                HandleMonstersAttack(monstersAttackEvent.Result);
                break;
            case PlayerCollectedTilesEvent playerCollectedTilesEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                 
                 
                 //_lr.enabled = false;
                 //what i really wanted to do was cleanup the selection
                 HandleSelectionChange(new List<Tile>());

                HandlePlayerCollectedTiles(playerCollectedTilesEvent.Tiles);
                break;
            case SelectionChangeEvent selectionChangeEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleSelectionChange(selectionChangeEvent.Tiles);
                break;
            case TileAddedToSelectionEvent tileAddedToSelectionEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleTileAddedToSelection(tileAddedToSelectionEvent.Tile);
                break;
            case CoinCollectedEvent coinCollectedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleCoinCollected(coinCollectedEvent.Amount);
                break;
            case PotionsCollectedEvent potionsCollectedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandlePotionsCollected(potionsCollectedEvent.Amount);
                break;
            case ShieldsCollectedEvent shieldsCollectedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleShieldsCollected(shieldsCollectedEvent.Amount);
                break;
            case SwordsCollectedEvent swordsCollectedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleSwordsCollected(swordsCollectedEvent.Amount);
                break;
            case MonsterKillsEarnedEvent monsterKillsEarnedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleMonsterKillEarned(monsterKillsEarnedEvent.Amount);
                break;
            case ExperienceGainedEvent experienceGainedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleExperienceGained(experienceGainedEvent.Amount);
                break;
            case EnemyStunnedEvent enemyStunnedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleMonsterStunned();
                break;
            case GoldGoalReachedEvent goldGoalReachedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleGoldGoalReached();
                break;
            case DefenseGoalReachedEvent defenseGoalReachedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleDefenseGoalReached();
                break;
            case PhaseChangeEvent phaseChangeEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandlePhaseChange(phaseChangeEvent.Phase);
                break;
            case ExperienceGoalReachedEvent experienceGoalReachedEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleExperienceGoalReached();
                break;
            case LoseEvent loseEvent:
                // yield return performance here
                // can maybe pass this handle as a lambda to time its fire
                HandleLose(loseEvent.statSheet);
                break; 
            default :
                break;
        }
    }

    void HandleRawEvent(TileGameEvent gameEvent) {
        if (gameEvent is not DebugLogEvent) {
            _eventQueue.Enqueue(gameEvent);
        }
        Debug.Log(gameEvent.GetType().ToString());
    }

    void OnDestroy() {
        if (Board == null) return;
        
        Board.Events.OnRawEvent -= HandleRawEvent;
        Board.Events.OnDebugLog -= HandleDebugLog;
    }

/*

    HANDLE methods are what we want to do now that it's public knowledge event happening right now
    Performances must have already happened! They start before public knowledge.

    Modify in DoPerformance if you want to do something prior to this event going public

*/

    void HandleUserIndicatingTile(Tile tile) {
        Board.Input.UserIndicatingTile(tile);
    }
    
    void HandleUserStartSelection(Tile tile) {
        Board.Input.UserStartSelection(tile);
    }

    void HandleUserEndSelection() {
        Board.Input.UserEndSelection();
    }
    

    void HandleDebugLog(string msg) {
        Debug.Log($">CORE< {msg}");
    }

    void HandlePhaseChange(BoardPhase phase) {
        Debug.Log($"PHASE NOW {phase.ToString()}");
    }

    void HandleExperienceGained(int exp) {
      //  Debug.Log($"exp gained: {exp}");
        FloatExp(exp);
    }

    void HandleExperienceGoalReached() {
        // XPShopMenu.SetActive(true);
    }

    void HandleGoldGoalReached() {
        ItemShopMenu.SetActive(true);
    }

    void HandleDefenseGoalReached() {
        // EnchantmentShopMenu.SetActive(true);
    }


    void HandlePlayerCollectedTiles(List<Tile> tiles) {

    }

    void HandleMonstersAttack(DamageResult playerTookDamage) {
        if (RoundHasEnded) return;

        //FloatDamage(playerTookDamage.Attempted);
    }

    void HandleSelectionChange(List<Tile> selection) {
        _lr.positionCount = selection.Count;
        
        List<Vector3> positionList = selection.Select(o => new Vector3(o.col, o.row, 0f)).ToList();
        _lr.SetPositions(positionList.ToArray());


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
                circleText = $"{Board.State.Player.CalcDamageDone(swordCount)}D";
            }

            if (potionCount > 0) {
                circleText = $"{Board.State.Player.CalcHealingDone(potionCount)}H";
            }

            if (shieldCount > 0) {
                circleText = $"{Board.State.Player.CalcArmorGained(shieldCount)}A";
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
        _gc.PreviousRoundMoves = Board.State.MovesMade;
        yield return new WaitForSeconds(0.2f);
        _gc.ChangeScene("Title");
    }
}
