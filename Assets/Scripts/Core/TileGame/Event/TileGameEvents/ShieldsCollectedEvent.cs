
public class ShieldsCollectedEvent : TileGameEvent {
    public readonly CollectionResult CollectionResult;
    public ShieldsCollectedEvent(CollectionResult cr) {
        CollectionResult = cr;
    }
}
