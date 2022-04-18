using System;

public class StatSheet
{
    public RandomRoller roller = new RandomRoller();

    public int BASE_HP = 0;
    public int BASE_Damage = 0;
    public int PERDEXTERITY_ShieldArmorPoints = 1;
    public int PERLUCK_PotionHealingPoints = 1;
    public int PERSTRENGTH_BaseDamage = 1;
    public int PERVITALITY_MaxHitPoints = 1;



    public int Hp = 0;
    public int Armor;
    public int Gold;
    public int Strength = 1;
    public int Dexterity = 1;
    public int Vitality = 1;
    public int Luck = 1;
    public int WeaponDamage = 1;
    public int Defense = 4;
    public int ArmorPiercing = 50;
    public int ArmorDurability = 50;


    public bool isAlive() {
        return Hp > 0;
    }

    public int CalcMaxArmor() {
        return Defense;
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

    public DamageResult TakeDamage(int damageReceived, int attackerArmorPiercing = 0) {
        int assignedToArmor = 0;
        int assignedToHealth = 0;
        int actualArmorDamage = 0;

        assignedToArmor = Math.Min(damageReceived, Armor);
        assignedToHealth = Math.Max(damageReceived - Armor, 0);
        
        int countOfPiercedShields = doPiercingOnShieldChecks(assignedToArmor, attackerArmorPiercing);
        assignedToArmor -= countOfPiercedShields;
        assignedToHealth += countOfPiercedShields;
        
        actualArmorDamage = doDamageOnShieldChecks(assignedToArmor);
        Armor -= actualArmorDamage;
        Hp -= assignedToHealth;
    
        return new DamageResult(){
            Attempted = damageReceived,
            AssignedToArmor = assignedToArmor,
            AssignedToHitPoints = assignedToHealth,
            ArmorRemoved = actualArmorDamage,
            HitPointsRemoved = assignedToHealth,
            ArmorPierced = countOfPiercedShields
        };
    }


    /*
        INTERNAL
    */

    int doPiercingOnShieldChecks(int checks, int attackerArmorPiercing) {
        int shieldsPierced = doRollsAgainstChance(checks, attackerArmorPiercing);
        return shieldsPierced;
    }

    int doDamageOnShieldChecks(int checks) {
        int shieldSaves = doRollsAgainstChance(checks, ArmorDurability);
        int lostShields = checks - shieldSaves;

        return lostShields;
    }

    protected int doRollsAgainstChance(int bonusRolls, int bonusChance) {
        var successfulRolls = 0;
        int percentChancePerRoll = bonusChance;
        for(var i=0; i<bonusRolls; i++) {
            int roll = roller.Roll();

            if (roll <= percentChancePerRoll) successfulRolls +=1;
        }
        return successfulRolls;
    }
}
