
public class ShieldsCollectedEvent : TileGameEvent {
    public readonly int Amount;
    public ShieldsCollectedEvent(int amount) {
        this.Amount = amount;
    }
}
