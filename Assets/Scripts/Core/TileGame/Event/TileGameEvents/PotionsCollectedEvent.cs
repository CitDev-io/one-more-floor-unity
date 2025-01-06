
public class PotionsCollectedEvent : TileGameEvent {
    public readonly CollectionResult collectionResult;
    public PotionsCollectedEvent(CollectionResult cr) {
        this.collectionResult = cr;
    }
}