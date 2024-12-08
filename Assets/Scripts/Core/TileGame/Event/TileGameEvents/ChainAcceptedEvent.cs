using System.Collections.Generic;
using System.Linq;

public class ChainAcceptedEvent : TileGameEvent, IDomainEvent {
    public List<Tile> Chain;
    public ChainAcceptedEvent(List<Tile> _chain) {
        Chain = _chain;
    }

    public List<TileGameEvent> Visit(GameState currentState)
    {
        List<TileGameEvent> outputEvents = new ();
        currentState.MovesMade += 1;
        // currentState.TilesCleared += Chain.Count;

        var tilesCollectedCountByType = Chain
            .GroupBy((o) => o.tileType)
            .Select((o) => new { TileType = o.Key, Count = o.Count() })
            .ToList();

        

        //  COINS
        List<Tile> coinsCollected = Chain.Where((o) => o.tileType == TileType.Coin).ToList();

        int coinGained = Chain
            .Where((o) => o.tileType == TileType.Coin)
            .ToList().Count + (Chain
            .Where((o) => o.tileType == TileType.Treasure)
            .ToList().Count * 10);
        

        if (coinGained > 0) {
            CollectionResult cr = currentState.Player.CollectCoins(coinGained);
            outputEvents.Add(
                new CoinCollectedEvent(cr.Earned + cr.BonusGained)
            );
        }

        // POTIONS
        int potionsCollected = Chain
            .Where((o) => o.tileType == TileType.Potion)
            .ToList().Count;

        if (potionsCollected != 0)
        {
            CollectionResult cr = currentState.Player.CollectPotions(potionsCollected);
            outputEvents.Add(
                new PotionsCollectedEvent(cr.Earned + cr.BonusGained)
            );
        }

        // SWORDS ENEMIES AND ARMOR
        int shieldsCollected = Chain
            .Where((o) => o.tileType == TileType.Shield)
            .ToList().Count;
        int swordsCollected = Chain
            .Where((o) => o.tileType == TileType.Sword)
            .ToList().Count;
        
        if (swordsCollected > 0) {
            outputEvents.Add(
                new SwordsCollectedEvent(swordsCollected)
            );
        }
        
        List<Tile> enemiesInSelection = Chain
            .Where((o) => o.tileType == TileType.Monster).ToList();

        if (shieldsCollected != 0 && enemiesInSelection.Count == 0)
        {
            CollectionResult result = currentState.Player.CollectShields(shieldsCollected);
            outputEvents.Add(
                new ShieldsCollectedEvent(result.Earned + result.BonusGained)
            );
        }
    
        if (enemiesInSelection.Count > 0 && shieldsCollected > 0) {
            outputEvents.Add(
                new EnemyStunnedEvent()
            );
        }

        int damageDealt = currentState.Player.CalcDamageDone(swordsCollected);

        foreach (Tile monster in enemiesInSelection)
        {
            monster.TakeDamage(damageDealt, currentState.Player.TotalStats.ArmorPiercing);
        }

        int killCount = enemiesInSelection.Where((o) => !o.isAlive()).Count();
        if (killCount > 0) {
            outputEvents.Add(
                new MonsterKillsEarnedEvent(killCount)
            );
            CollectionResult result = currentState.Player.CollectKilledMonsters(killCount);
            outputEvents.Add(
                new ExperienceGainedEvent(result.Earned + result.BonusGained)
            );
        }

        outputEvents.Add(
            new PlayerCollectedTilesEvent(Chain)
        );

        currentState.PlayersTurn = false;
        currentState.Tiles.TagCollectibleTiles(Chain);
        currentState.Tiles.FlushCollectedAndDeadTiles((ITileCollectorContext)currentState);
        currentState.SetPhase(BoardPhase.PLAYERTURN);

        outputEvents.Add(
            new MonsterTurnEvent()
        );


        return outputEvents;
    }
}