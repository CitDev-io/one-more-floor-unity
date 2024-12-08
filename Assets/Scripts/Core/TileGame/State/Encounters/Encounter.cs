using System.Collections.Generic;

public abstract class Encounter
{
    protected TileSelector tileSelector;
    protected MonsterSelector monsterSelector;
    public int cols = 6;
    public int rows = 6;

    public Encounter()
    {
        this.tileSelector = new TileSelector(new List<TileType> {
            TileType.Shield,
            TileType.Sword,
            TileType.Potion,
            TileType.Coin
        });
        this.monsterSelector = new MonsterSelector();
    }

    public abstract StatSheet GetNextMonster(ITileCollectorContext context);
    public abstract TileType GetNextTile(ITileCollectorContext context);
}
