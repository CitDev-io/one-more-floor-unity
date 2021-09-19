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
    public int HitPoints { get; private set; } = 2;
    public int MaxHitPoints { get; private set; } = 2;
    public int Damage { get; private set; } = 2;
    int StunnedRounds = 0;
    int TurnsAlive = 0;
    public bool IsBeingCollected = false;

    public Tile(int x, int y, int HP, int DMG, TileType tt)
    {
        HitPoints = HP;
        MaxHitPoints = HP;
        Damage = DMG;

        col = x;
        row = y;
        SetTileType(tt);
    }
    public int row { get; private set; } = 5; // Y Y Y Y Y Y 
    public int col { get; private set; } = 5; // X X X X X X

    public void ClearAndDropTileAs(TileType tileType) {
        Reset();
        AssignPosition(col, 5);
        SetTileType(tileType);
    }

    public void SetTileType(TileType tt)
    {
        tileType = tt;
        OnTileTypeChange?.Invoke();
    }

    public void Stun() {
        StunnedRounds = 3;
        OnStunned?.Invoke();
    }

    public void AgeUp() {
        TurnsAlive += 1;
        StunnedRounds -= 1;

        if (StunnedRounds == 0) {
            OnUnstunned?.Invoke();
        }
    }

    public Vector3 GridPosition() {
        return new Vector3(
            col,
            row,
            0f
        );
    }

    public void TakeDamage(int dmg, DamageSource src) {
        HitPoints -= dmg;
        OnDamageTaken?.Invoke(dmg, src);
    }

    public bool isAlive() {
        return HitPoints > 0;
    }

    public bool isStunned() {
        return StunnedRounds > 0;
    }

    public bool wasAliveBeforeLastUserAction() {
        return TurnsAlive > 0;
    }

    public void Dropdown(){
        AssignPosition(col, row - 1);
    }

    public void DoAttack() {
        OnDoAttack?.Invoke();
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
        HitPoints = MaxHitPoints;
        TurnsAlive = 0;
        StunnedRounds = 0;
        IsBeingCollected = false;
    }
}
