using System.Collections.Generic;

public class SelectionChangeEvent : TileGameEvent {
    public readonly List<Tile> Tiles;
    public SelectionChangeEvent(List<Tile> tiles) {
        this.Tiles = tiles;
    }
}
