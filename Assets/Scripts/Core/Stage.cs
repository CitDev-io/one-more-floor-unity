using System;

public class Stage
{
    public int StageNumber = 1;
    public int KillRequirement() {
        return 14 + StageNumber;
    }
    public int EnemyHp()
    {
        return 7 + (int) Math.Ceiling(StageNumber / 3f) + 1;
    }

    public int EnemyDmg() {
        return 1 + (int) (StageNumber / 4f);
    }
}
