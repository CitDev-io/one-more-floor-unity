
public class MonsterKillsEarnedEvent : TileGameEvent {
    public readonly int Amount;
    public MonsterKillsEarnedEvent(int amount) {
        this.Amount = amount;
    }
}
