using System.Collections.Generic;


public class PlayerCollectedTilesEvent : TileGameEvent {
    public readonly List<Tile> Tiles;
    public PlayerCollectedTilesEvent(List<Tile> tiles) {
        this.Tiles = tiles;
    }
}
