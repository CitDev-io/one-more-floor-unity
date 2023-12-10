
public class PhaseChangeEvent : TileGameEvent {
    public readonly BoardPhase Phase;
    public PhaseChangeEvent(BoardPhase phase) {
        Phase = phase;
    }
}
