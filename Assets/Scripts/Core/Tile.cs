using UnityEngine;

public class Tile
{
    public NoParamDelegate OnTileTypeChange;
    public NoParamDelegate OnPositionChange;
    public NoParamDelegate OnStunned;
    public NoParamDelegate OnUnstunned;
    public NoParamDelegate OnDoAttack;
    public SourcedDamageDelegate OnDamageTaken;
    public TileType tileType { get; private set; }

    public StatSheet CurrentMonster { get; internal set; } = new StatSheet();
    int StunnedRounds = 0;
    public int TurnsAlive { get; private set; } = 0;
    public bool IsBeingCollected { get; internal set; } = false;
    public int selectedAgainstDamage { get; internal set; } = 0;

    public Tile(int x, int y, StatSheet enemy, TileType tt)
    {
        CurrentMonster = enemy;

        col = x;
        row = y;
        SetTileType(tt);
    }
    public int row { get; private set; } = 5; // Y Y Y Y Y Y 
    public int col { get; private set; } = 5; // X X X X X X

    /*
        internal controls for board
    */


    internal void ClearAndDropTileAs(TileType tileType) {
        Reset();
        AssignPosition(col, 5);
        SetTileType(tileType);
    }

    internal void SetTileType(TileType tt)
    {
        tileType = tt;
        OnTileTypeChange?.Invoke();
    }

    internal void Stun() {
        StunnedRounds = 3;
        OnStunned?.Invoke();
    }

    internal void AgeUp() {
        TurnsAlive += 1;
        StunnedRounds -= 1;

        if (StunnedRounds == 0) {
            OnUnstunned?.Invoke();
        }
    }

    internal void TakeDamage(int dmg, int armorPiercing) {
        CurrentMonster.TakeDamage(dmg, armorPiercing);
        OnDamageTaken?.Invoke(dmg);
    }

    internal void Dropdown(){
        AssignPosition(col, row - 1);
    }

    internal void DoAttack() {
        OnDoAttack?.Invoke();
    }

    /*
        public info fetches
    */

    public Vector3 GridPosition() {
        return new Vector3(
            col,
            row,
            0f
        );
    }

    public bool isAlive() {
        return CurrentMonster.isAlive();
    }

    public bool isStunned() {
        return StunnedRounds > 0;
    }

    public bool wasAliveBeforeLastUserAction() {
        return TurnsAlive > 0;
    }


    /**

        private

    **/

    void AssignPosition(float x, float y)
    {
        row = (int) y;
        col = (int) x;
        OnPositionChange?.Invoke();
    }

    void Reset() {
        TurnsAlive = 0;
        StunnedRounds = 0;
        IsBeingCollected = false;
        selectedAgainstDamage = 0;
    }
}
