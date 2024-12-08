using System.Collections.Generic;

public class Draft1Encounter : Encounter
{

    public Draft1Encounter()
    {
        this.tileSelector = new TileSelector(new List<TileType> {
            TileType.Shield,
            TileType.Sword,
            TileType.Potion,
            TileType.Coin
        });
        this.monsterSelector = new MonsterSelector();
    }
    public override StatSheet GetNextMonster(ITileCollectorContext context) {
        return monsterSelector.NextMonster(context);
    }

    public override TileType GetNextTile(ITileCollectorContext context) {
        return tileSelector.GetNextTile(context);
    }
}
