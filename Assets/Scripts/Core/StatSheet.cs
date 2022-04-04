using System;

public class StatSheet : IExperiencable
{
    public StatSheet(int maxHp, int maxSp, int dmg, int coins)
    {
        MaxHp = maxHp;
        Hp = maxHp;
        MaxSp = maxSp;
        Damage = dmg;
        Coins = coins;
    }
    public int Damage { get; private set; }
    public int Hp { get; private set; }
    public int MaxHp { get; private set; }
    public int Sp { get; private set; }
    public int MaxSp { get; private set; }
    public int Coins { get; private set; }
    int CoinGoal = 4;

    public bool HasReachedCoinGoal() {
        return CoinGoal >= Coins;
    }

    public void ApplyHP(int amt) {
        int overheal = Math.Max((Hp + amt) - MaxHp, 0);

        Hp = Math.Max(Math.Min(Hp + amt, MaxHp), 0);
        HeartExpPoints += overheal;
    }

    public void ApplySP(int amt) {
        int overheal = Math.Max((Sp + amt) - MaxSp, 0);
    
        Sp = Math.Max(Math.Min(Sp + amt, MaxSp), 0);
        SpecialExpPoints += overheal;
    }

    public void ApplySwords(int amt) {
        SwordExpPoints += amt;
    }

    public int CollectCoins(int collected) {
        return Coins += collected;
    }

    public int SpendCoins(int spent) {
        return Coins -= spent;
    }

    public int ExpPoints { get; set; }
    public int SwordExpPoints { get; set; }
    public int HeartExpPoints { get; set; }
    public int SpecialExpPoints { get; set; }
}
