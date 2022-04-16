using System;

public class StatSheet
{
    public class StatAppliedResult {
        public int Amount;
        public int Applied;
        public int Unapplied;
    }

    public StatSheet(int maxHp, int maxSp, int coins)
    {
        MaxHp = maxHp;
        Hp = maxHp;
        MaxSp = maxSp;
        Coins = coins;
    }
    public RandomRoller roller = new RandomRoller();

    int BASE_Damage = 2;
    int BASE_BonusXPChance = 15;
    int PERSTRENGTH_BonusXPChance = 5;
    int PERROLL_BonusXP = 1;

    public int Hp { get; private set; }
    public int MaxHp { get; private set; }
    public int Sp { get; private set; }
    public int MaxSp { get; private set; }
    public int Coins { get; private set; }


    public int Strength { get; private set; } = 1;
    public int Dexterity { get; private set; } = 1;
    public int Vitality { get; private set; } = 1;
    public int Luck { get; private set; } = 1;
    public int WeaponDamage { get; private set; } = 1;
    public int ArmorPiercing { get; private set; } = 50;
    public int ArmorDurability { get; private set; } = 50;
    public int Defense { get; private set; } = 1;


    public int DefensePoints { get; private set; }
    public int ExperiencePoints { get; private set; }
    public int CoinGoal = 15;
    public int DefenseGoal = 15;
    public int ExperienceGoal = 12;
    public int MonstersKilled { get; private set; }

    public int CalcDamageDone(int swords) {
        return CalcBaseDamage() + (swords * WeaponDamage);
    }

    public int CalcBaseDamage() {
        return Strength + BASE_Damage;
    }


    public bool HasReachedExperienceGoal() {
        return ExperiencePoints >= ExperienceGoal;
    }

    public bool HasReachedCoinGoal() {
        return Coins >= CoinGoal;
    }

    public bool HasReachedDefenseGoal() {
        return DefensePoints >= DefenseGoal;
    }

    // public void IterateToNextExpGoal() {
    //     ExperienceGoal += 12;
    // }

    public StatAppliedResult ApplyHP(int amt) {
        int overheal = Math.Max((Hp + amt) - MaxHp, 0);

        Hp = Math.Max(Math.Min(Hp + amt, MaxHp), 0);

        return new StatAppliedResult(){
            Amount = amt,
            Unapplied = overheal,
            Applied = amt - overheal
        };
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
        return new StatAppliedResult(){
            Amount = amt,
            Unapplied = 0,
            Applied = amt
        };
    }

    public int CollectCoins(int collected) {
        return Coins += collected;
    }

    public int SpendDownCoins() {
        return Coins -= CoinGoal;
    }

    public int SpendDownExp() {
        return ExperiencePoints -= ExperienceGoal;
    }

    public int SpendDownDefensePoints() {
        return DefensePoints - DefenseGoal;
    }

    public MonsterCollectionResult CollectKilledMonsters(int amt) {
        int experienceEarned = amt;
        MonstersKilled += amt;

        int bonusExperienceRollCount = Math.Max(amt - 3, 0);
        
        int successfulXpBonuses = doBonusExperienceRolls(bonusExperienceRollCount);
        int bonusXp = successfulXpBonuses * PERROLL_BonusXP;
        ExperiencePoints += experienceEarned + bonusXp;
        
        return new MonsterCollectionResult(){
            Collected = amt,
            GameTotalCollected = MonstersKilled,
            XPEarned = experienceEarned,
            BonusXPRollCount = bonusExperienceRollCount,
            SuccessfulBonusXPRollCount = successfulXpBonuses,
            BonusXPGained = bonusXp
        };
    }

    public int BonusXpChance() {
        return BASE_BonusXPChance + (PERSTRENGTH_BonusXPChance * Strength);
    }
    

    /*
        INTERNAL
    */

    int doBonusExperienceRolls(int bonusRolls) {
        var successfulRolls = 0;
        int percentChancePerRoll = BonusXpChance();
        for(var i=0; i<bonusRolls; i++) {
            int roll = roller.Roll();

            if (roll <= percentChancePerRoll) successfulRolls +=1;
        }
        return successfulRolls;
    }

}
