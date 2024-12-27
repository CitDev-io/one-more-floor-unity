public class ExperienceGainedEvent : TileGameEvent {
    public readonly CollectionResult collectionResult;
    public ExperienceGainedEvent(CollectionResult cr) {
        collectionResult = cr;
    }
}
