using System;

public class StatSheet
{
    public class StatAppliedResult {
        public int Amount;
        public int Applied;
        public int Unapplied;
    }

    public StatSheet(int maxSp, int coins)
    {
        Hp = CalcMaxHp();
        MaxArmor = maxSp;
        Gold = coins;
    }
    public RandomRoller roller = new RandomRoller();

    int BASE_HP = 35;
    int BASE_Damage = 2;
    int BASE_BonusXPChance = 15;
    int BASE_BonusHPChance = 15;
    int BASE_BonusAPChance = 15;
    int BASE_BonusCoinChance = 15;
    int PERDEXTERITY_BonusShieldChance = 5;
    int PERDEXTERITY_ShieldArmorPoints = 1;
    int PERLUCK_BonusCoinChance = 5;
    int PERLUCK_PotionHealingPoints = 1;
    int PERSTRENGTH_BonusXPChance = 5;
    int PERSTRENGTH_BaseDamage = 1;
    int PERVITALITY_BonusPotionChance = 5;
    int PERVITALITY_MaxHitPoints = 5;
    int PERROLL_BonusXP = 1;

    public int Hp { get; private set; }
    public int Armor { get; private set; }
    public int MaxArmor { get; private set; }
    public int Gold { get; private set; }


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
    public int GoldGoal = 15;
    public int DefenseGoal = 15;
    public int ExperienceGoal = 12;
    public int MonstersKilled { get; private set; }

    public bool isAlive() {
        return Hp > 0;
    }

    public int CalcHealingDone(int potions) {
        return CalcHealPerPotion() * potions;
    }

    public int CalcDamageDone(int swords) {
        return CalcBaseDamage() + (swords * WeaponDamage);
    }

    public int CalcGoldGained(int coins) {
        return coins * 1;
    }

    public int CalcArmorGained(int shields) {
        return shields * PERDEXTERITY_ShieldArmorPoints;
    }

    public int CalcHealPerPotion() {
        return Luck * PERLUCK_PotionHealingPoints;
    }

    public int CalcBaseDamage() {
        return (Strength * PERSTRENGTH_BaseDamage) + BASE_Damage;
    }

    public int CalcMaxHp() {
        return BASE_HP + (PERVITALITY_MaxHitPoints * Vitality);
    }

    public bool HasReachedExperienceGoal() {
        return ExperiencePoints >= ExperienceGoal;
    }

    public bool HasReachedCoinGoal() {
        return Gold >= GoldGoal;
    }

    public bool HasReachedDefenseGoal() {
        return DefensePoints >= DefenseGoal;
    }

    public DamageResult TakeDamage(int damageReceived) {
        int assignedToArmor = 0;
        int assignedToHealth = 0;
        int actualArmorDamage = 0;
        int actualHPDamage = 0;

        if (Armor >= damageReceived)
        {
            assignedToArmor = damageReceived;
            actualArmorDamage = doDamageOnShieldChecks(damageReceived);
        } else {
            actualArmorDamage = doDamageOnShieldChecks(Armor);

            int remainingDmg = damageReceived - Armor;
            assignedToArmor = Armor;
            assignedToHealth = remainingDmg;
            Hp -= remainingDmg;
        }

        return new DamageResult(){
            Attempted = damageReceived,
            AssignedToArmor = assignedToArmor,
            AssignedToHitPoints = assignedToHealth,
            ArmorRemoved = actualArmorDamage,
            HitPointsRemoved = actualHPDamage
        };
    }

    public CollectionResult CollectShields(int shieldsCollected) {
        int bonusShieldRollCount = Math.Max(shieldsCollected - 3, 0);
        int bonusShields = doRollsAgainstChance(bonusShieldRollCount, BonusShieldChance());
        
        int armorEarned = CalcArmorGained(shieldsCollected);
        int bonusGained = CalcArmorGained(bonusShields);
        int armorToApply = armorEarned + bonusGained;

        int overheal = Math.Max((Armor + armorToApply) - MaxArmor, 0);
    
        Armor = Math.Max(Math.Min(Armor + armorToApply, MaxArmor), 0);
        DefensePoints += overheal;

        return new CollectionResult() {
            Collected = shieldsCollected,
            GameTotalCollected = Armor,
            Earned = armorEarned,
            BonusRollCount = bonusShieldRollCount,
            SuccessfulBonusRollCount = bonusShields,
            BonusGained = bonusGained
        };
    }

    public CollectionResult CollectCoins(int collected) {
        int coinsEarned = collected;
        int bonusCoinRollCount = Math.Max(collected - 3, 0);

        int successfulCoinBonuses = doRollsAgainstChance(bonusCoinRollCount, BonusCoinChance());
        int bonusCoins = successfulCoinBonuses;

        Gold += CalcGoldGained(coinsEarned + bonusCoins);

        return new CollectionResult() {
            Collected = collected,
            GameTotalCollected = Gold,
            Earned = CalcGoldGained(coinsEarned),
            BonusRollCount = bonusCoins,
            SuccessfulBonusRollCount = successfulCoinBonuses,
            BonusGained = CalcGoldGained(bonusCoins)
        };
    }

    public CollectionResult CollectPotions(int collected) {
        int healingEarned = CalcHealingDone(collected);
        int bonusPotionRollCount = Math.Max(collected - 3, 0);

        int successfulPotionBonuses = doRollsAgainstChance(bonusPotionRollCount, BonusHpChance());
        int bonusHealGained = CalcHealingDone(successfulPotionBonuses);

        
        int totalHealing = healingEarned + bonusHealGained;
        int overheal = Math.Max((Hp + totalHealing) - CalcMaxHp(), 0);

        Hp = Math.Max(Math.Min(Hp + totalHealing, CalcMaxHp()), 0);

        return new CollectionResult() {
            Collected = collected,
            GameTotalCollected = Hp,
            Earned = healingEarned,
            BonusRollCount = bonusPotionRollCount,
            SuccessfulBonusRollCount = successfulPotionBonuses,
            BonusGained = bonusHealGained
        };
    }

    public CollectionResult CollectKilledMonsters(int amt) {
        int experienceEarned = amt;
        MonstersKilled += amt;

        int bonusExperienceRollCount = Math.Max(amt - 3, 0);
        
        int successfulXpBonuses = doRollsAgainstChance(bonusExperienceRollCount, BonusXpChance());
        int bonusXp = successfulXpBonuses * PERROLL_BonusXP;
        ExperiencePoints += experienceEarned + bonusXp;
        
        return new CollectionResult(){
            Collected = amt,
            GameTotalCollected = MonstersKilled,
            Earned = experienceEarned,
            BonusRollCount = bonusExperienceRollCount,
            SuccessfulBonusRollCount = successfulXpBonuses,
            BonusGained = bonusXp
        };
    }


    public int SpendDownCoins() {
        return Gold -= GoldGoal;
    }

    public int SpendDownExp() {
        return ExperiencePoints -= ExperienceGoal;
    }

    public int SpendDownDefensePoints() {
        return DefensePoints -= DefenseGoal;
    }




    public int BonusXpChance() {
        return BASE_BonusXPChance + (PERSTRENGTH_BonusXPChance * Strength);
    }

    public int BonusCoinChance() {
        return BASE_BonusCoinChance + (PERLUCK_BonusCoinChance * Luck);
    }

    public int BonusHpChance() {
        return BASE_BonusHPChance + (PERVITALITY_BonusPotionChance * Vitality);
    }
    
    public int BonusShieldChance() {
        return BASE_BonusAPChance + (PERDEXTERITY_BonusShieldChance * Dexterity);
    }

    /*
        INTERNAL
    */


    int doDamageOnShieldChecks(int checks) {
        int shieldSaves = doRollsAgainstChance(checks, ArmorDurability);
        int lostShields = checks - shieldSaves;

        Armor -= lostShields;

        return lostShields;
    }

    int doRollsAgainstChance(int bonusRolls, int bonusChance) {
        var successfulRolls = 0;
        int percentChancePerRoll = bonusChance;
        for(var i=0; i<bonusRolls; i++) {
            int roll = roller.Roll();

            if (roll <= percentChancePerRoll) successfulRolls +=1;
        }
        return successfulRolls;
    }
}
