using System;
using System.Collections.Generic;
using System.Linq;

public class StatSheet
{
    public StatSheet(params StatMatrix[] _startingStats) {
        initWith(_startingStats);
    }

    protected void initWith(StatMatrix[] stats, Dictionary<ItemSlot, PlayerItem> _inventory = null) {
        var total = StatMatrix.Reduce(stats);
        if (_inventory != null) {
            Inventory = _inventory;
        }
        StarterStats = total;
        TotalStats = total;
        ResetTotalStats();
        Hp = CalcMaxHp();
    }
    public RandomRoller roller = new RandomRoller();

    public int BASE_HP = 0;
    public int PERDEXTERITY_ShieldArmorPoints = 1;
    public int PERLUCK_PotionHealingPoints = 1;
    public int PERSTRENGTH_BaseDamage = 1;
    public int PERVITALITY_MaxHitPoints = 1;

    public StatMatrix StarterStats;
    public StatMatrix TotalStats { get; protected set; }
    Dictionary<ItemSlot, PlayerItem> Inventory = new Dictionary<ItemSlot, PlayerItem>();
    public List<PlayerSkillup> Skillups { get; protected set; } = new List<PlayerSkillup>();

    public PlayerItem GetItemInInventorySlot(ItemSlot slot) {
        return Inventory[slot];
    }

    public int Hp = 0;
    public int Armor { get; protected set; } = 0;
    public int Gold { get; protected set; } = 0;

    public bool isAlive() {
        return Hp > 0;
    }

    public int CalcMaxArmor() {
        return TotalStats.Defense;
    }

    public int CalcHealingDone(int potions) {
        return CalcHealPerPotion() * potions;
    }

    public int CalcDamageDone(int swords) {
        return CalcBaseDamage() + (swords * TotalStats.WeaponDamage);
    }

    public int CalcGoldGained(int coins) {
        return coins * 1;
    }

    public int CalcArmorGained(int shields) {
        return shields * PERDEXTERITY_ShieldArmorPoints;
    }

    public int CalcHealPerPotion() {
        return TotalStats.Luck * PERLUCK_PotionHealingPoints;
    }

    public int CalcBaseDamage() {
        return (TotalStats.Strength * PERSTRENGTH_BaseDamage) + TotalStats.BaseDamage;
    }

    public int CalcMaxHp() {
        return BASE_HP + TotalStats.HitPoints + (PERVITALITY_MaxHitPoints * TotalStats.Vitality);
    }


/*
        INTERNAL
*/

    // TODO: probably more cleanly handled on a VIRTUAL method
    internal void AddEnchantmentToItemInSameSlot(PlayerItem enchantment) {
        ItemSlot affectedSlot = enchantment.Slot;
        PlayerItem oldItem = GetItemInInventorySlot(affectedSlot);
        Inventory[affectedSlot] = PlayerItem.Reduce(enchantment, oldItem);
        ResetTotalStats();
    }

    internal void AddItemToInventory(PlayerItem item) {
        Inventory[item.Slot] = item;
        ResetTotalStats();
    }

    internal void AddSkillupsToInventory(List<PlayerSkillup> skillups) {
        Skillups.AddRange(skillups);
        ResetTotalStats();
    }

    internal DamageResult TakeDamage(int damageReceived, int attackerArmorPiercing = 0) {
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
        private
    */

    int doPiercingOnShieldChecks(int checks, int attackerArmorPiercing) {
        int shieldsPierced = doRollsAgainstChance(checks, attackerArmorPiercing);
        return shieldsPierced;
    }

    int doDamageOnShieldChecks(int checks) {
        int shieldSaves = doRollsAgainstChance(checks, TotalStats.ArmorDurability);
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

    protected void ResetTotalStats() {
        StatMatrix[] allStats = Inventory
            .Select(z => (StatMatrix) z.Value)
            .Append(StarterStats)
            .Concat(Skillups)
            .ToArray();

        TotalStats = StatMatrix.Reduce(allStats);
    }
}
