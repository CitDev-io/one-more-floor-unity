
public class MonstersAttackEvent : TileGameEvent {
    public readonly DamageResult Result;
    public MonstersAttackEvent(DamageResult result) {
        this.Result = result;
    }
}
