
public class LoseEvent : TileGameEvent {
    public readonly StatSheet statSheet;
    public LoseEvent(StatSheet statSheet) {
        this.statSheet = statSheet;
    }
}
