
public class CoinCollectedEvent : TileGameEvent {
    public readonly int Amount;
    public CoinCollectedEvent(int amount) {
        this.Amount = amount;
    }
}
