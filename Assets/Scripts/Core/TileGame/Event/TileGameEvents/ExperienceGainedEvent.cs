public class ExperienceGainedEvent : TileGameEvent {
    public readonly int Amount;
    public ExperienceGainedEvent(int amount) {
        this.Amount = amount;
    }
}
