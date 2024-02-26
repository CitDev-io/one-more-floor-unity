using System.Collections.Generic;
using System.Linq;

public class Encounter
{
    // public PlayerAvatar _player; // TODO: address access. passing through for ChainBuilder to get to player
    TileSelector tileSelector;
    MonsterSelector monsterSelector;
    public int cols = 6;
    public int rows = 6;

    public Encounter()
    {
        // this._player = player;
        this.tileSelector = new TileSelector(new List<TileType> {
            TileType.Shield,
            TileType.Sword,
            TileType.Potion,
            TileType.Coin
        });
        this.monsterSelector = new MonsterSelector();
    }

    public StatSheet GetNextMonster() {
        return monsterSelector.NextMonster(1);
    }

    public TileType GetNextTile() {
        return tileSelector.GetNextTile();
    }
}
