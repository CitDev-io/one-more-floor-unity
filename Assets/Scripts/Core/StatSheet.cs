using System;

public class StatSheet
{
    public class StatAppliedResult {
        public int Amount;
        public int Applied;
        public int Unapplied;
    }

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
    public int DefensePoints { get; private set; }
    int CoinGoal = 25;
    int DefenseGoal = 25;

    public bool HasReachedCoinGoal() {
        return Coins >= CoinGoal;
    }

    public bool HasReachedDefenseGoal() {
        return DefensePoints >= DefenseGoal;
    }

    public StatAppliedResult ApplyHP(int amt) {
        int overheal = Math.Max((Hp + amt) - MaxHp, 0);

        Hp = Math.Max(Math.Min(Hp + amt, MaxHp), 0);
        HeartExpPoints += overheal;

        return new StatAppliedResult(){
            Amount = amt,
            Unapplied = overheal,
            Applied = amt - overheal
        };
    }

    public int SpendDefensePoints(int spent) {
        return DefensePoints -= spent;
    }

    public StatAppliedResult ApplySP(int amt) {
        int overheal = Math.Max((Sp + amt) - MaxSp, 0);
    
        Sp = Math.Max(Math.Min(Sp + amt, MaxSp), 0);
        DefensePoints += overheal;

        return new StatAppliedResult(){
            Amount = amt,
            Unapplied = overheal,
            Applied = amt - overheal
        };
    }

    public StatAppliedResult ApplySwords(int amt) {
        SwordExpPoints += amt;

        return new StatAppliedResult(){
            Amount = amt,
            Unapplied = 0,
            Applied = amt
        };
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
}
