
public class PotionsCollectedEvent : TileGameEvent {
    public readonly int Amount;
    public PotionsCollectedEvent(int amount) {
        this.Amount = amount;
    }
}