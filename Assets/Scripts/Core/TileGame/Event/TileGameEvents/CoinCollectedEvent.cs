
public class CoinCollectedEvent : TileGameEvent {
    public readonly CollectionResult collectionResult;
    public CoinCollectedEvent(CollectionResult cr) {
        collectionResult = cr;
    }
}
