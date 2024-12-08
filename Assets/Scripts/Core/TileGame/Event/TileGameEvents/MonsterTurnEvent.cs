using System.Collections.Generic;
using System.Linq;

public class MonsterTurnEvent : TileGameEvent, IDomainEvent {
    public MonsterTurnEvent() {

    }

    public List<TileGameEvent> Visit(GameState currentState) {
        List<TileGameEvent> outputEvents = new ();
        currentState.SetPhase(BoardPhase.MONSTERTURN);
        var awakeMonsters = currentState.GetTiles().TileList.Where((o) => o.tileType == TileType.Monster
            && o.wasAliveBeforeLastUserAction()
            && !o.isStunned()
            && o.isAlive()
        ).ToList();


        // MonstersAttack(monsters) :
        int damageReceived = awakeMonsters.Sum((o) => o.CurrentMonster.TotalStats.Strength);
        outputEvents.Add( new DebugLogEvent("Monsters attack for " + damageReceived + " damage"));
        
        if (damageReceived > 0) {
            var badGuyArmorPiercingAgainstMe = 0; // Consider that if we do pass armor piercing, it'll have to be by-source. that changes the signature anyway
            DamageResult result = currentState.Player.TakeDamage(damageReceived, badGuyArmorPiercingAgainstMe);
            outputEvents.Add(
                new MonstersAttackEvent(result)
            );
            foreach(Tile monster in awakeMonsters) {
                monster.DoAttack(); // doing event inside of Tile for UI to see. should it bubble up with the rest instead?
            }
        }

        // AgeAllMonsters() :
        var allMonsters = currentState.GetTiles().TileList.Where((o) => o.tileType == TileType.Monster);
        outputEvents.Add(new DebugLogEvent(allMonsters.Count() + " Monsters age up"));
        foreach(Tile monster in allMonsters) {
            monster.AgeUp();
        }

        currentState.PlayersTurn = true;
        // if this wasn't an obvious red flag for refactoring **** REFACTOR THIS!
        currentState.Tiles.FlushCollectedAndDeadTiles((ITileCollectorContext)currentState);
        currentState.SetPhase(BoardPhase.PLAYERTURN);

        return outputEvents;
    }
}
