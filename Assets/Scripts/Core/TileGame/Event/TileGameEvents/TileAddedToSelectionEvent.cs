
public class TileAddedToSelectionEvent : TileGameEvent {
    public readonly Tile Tile;
    public TileAddedToSelectionEvent(Tile tile) {
        this.Tile = tile;
    }
}
