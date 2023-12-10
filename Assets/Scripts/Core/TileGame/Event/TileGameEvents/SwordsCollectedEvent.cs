
public class SwordsCollectedEvent : TileGameEvent {
    public readonly int Amount;
    public SwordsCollectedEvent(int amount) {
        this.Amount = amount;
    }
}
