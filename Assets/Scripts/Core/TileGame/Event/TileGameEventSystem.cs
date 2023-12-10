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

public class TileGameEventSystem : IEventInvoker
{
    public event TilesDelegate OnPlayerCollectedTiles, OnSelectionChange;
    public event TileDelegate OnTileAddedToSelection;
    public event IntDelegate OnCoinCollected, OnPotionsCollected;
    public event IntDelegate OnShieldsCollected, OnPoisonCollected, OnSwordsCollected;
    public event IntDelegate OnMonsterKillsEarned, OnExperienceGained;
    public event NoParamDelegate OnEnemyStunned, OnGoldGoalReached, OnDefenseGoalReached;
    public event NoParamDelegate OnExperienceGoalReached;
    public event PhaseDelegate OnPhaseChange;
    public event DamageDelegate OnMonstersAttack;
    public event StatSheetDelegate OnLose;
    public event StringDelegate OnDebugLog;

    public void Invoke(TileGameEvent gameEvent) {
        switch(gameEvent) {
            case PlayerCollectedTilesEvent playerCollectedTilesEvent:
                OnPlayerCollectedTiles?.Invoke(playerCollectedTilesEvent.Tiles);
                break;
            case SelectionChangeEvent selectionChangeEvent:
                OnSelectionChange?.Invoke(selectionChangeEvent.Tiles);
                break;
            case TileAddedToSelectionEvent tileAddedToSelectionEvent:
                OnTileAddedToSelection?.Invoke(tileAddedToSelectionEvent.Tile);
                break;
            case CoinCollectedEvent coinCollectedEvent:
                OnCoinCollected?.Invoke(coinCollectedEvent.Amount);
                break;
            case PotionsCollectedEvent potionsCollectedEvent:
                OnPotionsCollected?.Invoke(potionsCollectedEvent.Amount);
                break;
            case ShieldsCollectedEvent shieldsCollectedEvent:
                OnShieldsCollected?.Invoke(shieldsCollectedEvent.Amount);
                break;
            case PoisonCollectedEvent poisonCollectedEvent:
                OnPoisonCollected?.Invoke(poisonCollectedEvent.Amount);
                break;
            case SwordsCollectedEvent swordsCollectedEvent: 
                OnSwordsCollected?.Invoke(swordsCollectedEvent.Amount);
                break;
            case MonsterKillsEarnedEvent monsterKillsEarnedEvent:
                OnMonsterKillsEarned?.Invoke(monsterKillsEarnedEvent.Amount);
                break;
            case ExperienceGainedEvent experienceGainedEvent:
                OnExperienceGained?.Invoke(experienceGainedEvent.Amount);
                break;
            case EnemyStunnedEvent enemyStunnedEvent:
                OnEnemyStunned?.Invoke();
                break;
            case GoldGoalReachedEvent goldGoalReachedEvent:
                OnGoldGoalReached?.Invoke();
                break;
            case DefenseGoalReachedEvent defenseGoalReachedEvent:
                OnDefenseGoalReached?.Invoke();
                break;
            case ExperienceGoalReachedEvent experienceGoalReachedEvent:
                OnExperienceGoalReached?.Invoke();
                break;
            case PhaseChangeEvent phaseChangeEvent:
                OnPhaseChange?.Invoke(phaseChangeEvent.Phase);
                break;
            case MonstersAttackEvent monstersAttackEvent: 
                OnMonstersAttack?.Invoke(monstersAttackEvent.Result);
                break;
            case LoseEvent loseEvent: 
                OnLose?.Invoke(loseEvent.statSheet);
                break;
            case DebugLogEvent debugLogEvent:
                OnDebugLog?.Invoke(debugLogEvent.str);
                break;
        }
    }

    public void Invoke(List<TileGameEvent> events)
    {
        foreach (TileGameEvent gameEvent in events) {
            Invoke(gameEvent);
        }
    }
}