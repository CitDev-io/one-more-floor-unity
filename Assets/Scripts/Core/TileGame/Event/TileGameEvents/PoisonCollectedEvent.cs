
public class PoisonCollectedEvent : TileGameEvent {
    public readonly int Amount;
    public PoisonCollectedEvent(int amount) {
        this.Amount = amount;
    }
}