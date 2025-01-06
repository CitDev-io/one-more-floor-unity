
public class PlayerReplacedItemEvent : TileGameEvent {
    public readonly PlayerItem Old;
    public readonly PlayerItem New;
    public PlayerReplacedItemEvent(PlayerItem _old, PlayerItem _new) {
        this.Old = _old;
        this.New = _new;
    }
}