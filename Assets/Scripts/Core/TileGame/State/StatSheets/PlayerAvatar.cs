using System;
using System.Collections.Generic;

public enum PlayerAvatarStatType {
    MAXHP,
    HP,
    XP,
    COINS,
    ARMOR,
    MONSTERKILLS,
    BOSSKILLS,
    HPCURMAX,
    GEARCURMAX,
    GEARCUR,
    GEARMAX,
    COINSCURMAX,
    COINSCUR,
    COINSMAX,
    XPCURMAX,
    XPCUR,
    XPMAX
}

public enum PlayerAvatarCurMaxType {
    HP,
    XP,
    COINS,
    ARMOR
}

public class PlayerAvatar: StatSheet {
    public Tuple<int, int> GetCurMax(PlayerAvatarCurMaxType curMaxType) {
        return curMaxType switch {
            PlayerAvatarCurMaxType.HP => new Tuple<int, int>(Hp, CalcMaxHp()),
            PlayerAvatarCurMaxType.XP => new Tuple<int, int>(ExperiencePoints, ExperienceGoal),
            PlayerAvatarCurMaxType.COINS => new Tuple<int, int>(Gold, GoldGoal),
            PlayerAvatarCurMaxType.ARMOR => new Tuple<int, int>(GearPoints, GearGoal),
            _ => throw new NotImplementedException()
        };
    }
    public String GetStat(PlayerAvatarStatType statType) {
        return statType switch {
            PlayerAvatarStatType.MAXHP => CalcMaxHp()+"",
            PlayerAvatarStatType.HP => Hp+"",
            PlayerAvatarStatType.XP => ExperiencePoints+"",
            PlayerAvatarStatType.COINS => Gold+"",
            PlayerAvatarStatType.ARMOR => Armor+"",
            PlayerAvatarStatType.MONSTERKILLS => MonstersKilled+"",
            PlayerAvatarStatType.BOSSKILLS => BossesKilled+"",
            PlayerAvatarStatType.HPCURMAX => Hp + " / " + CalcMaxHp(),
            PlayerAvatarStatType.GEARCURMAX => GearPoints + " / " + GearGoal,
            PlayerAvatarStatType.GEARCUR => GearPoints + "",
            PlayerAvatarStatType.GEARMAX => GearGoal + "",
            PlayerAvatarStatType.COINSCURMAX => Gold + " / " + GoldGoal,
            PlayerAvatarStatType.COINSCUR => Gold + "",
            PlayerAvatarStatType.COINSMAX => GoldGoal + "",
            PlayerAvatarStatType.XPCURMAX => ExperiencePoints + " / " + ExperienceGoal,
            PlayerAvatarStatType.XPCUR => ExperiencePoints + "",
            PlayerAvatarStatType.XPMAX => ExperienceGoal + "",
            _ => throw new NotImplementedException()
        };
    }
    public PlayerAvatar(params StatMatrix[] _startingStats) : base(_startingStats) {
        BASE_HP = 25;
        PERVITALITY_MaxHitPoints = 5;
        initWith(_startingStats, new Dictionary<ItemSlot, PlayerItem>() {
            { ItemSlot.WEAPON, new PlayerItem(){
                Slot = ItemSlot.WEAPON,
                Name = "Moldy Wooden Sword",
                Description = "Ew... It's kinda slimy",
                WeaponDamage = 1
            } },
            { ItemSlot.CHEST, new PlayerItem(){
                Slot = ItemSlot.CHEST,
                Name = "Worn Cloth Tunic",
                Description = "It's not very warm"
            } },
            { ItemSlot.SHIELD, new PlayerItem(){
                Slot = ItemSlot.SHIELD,
                Name = "Chipped Round Shield",
                Description = "Minimal Protection"
            } },
            { ItemSlot.HELMET, new PlayerItem(){
                Slot = ItemSlot.HELMET,
                Name = "Grimy Leather Cap",
                Description = "Discarded long ago"
            } },
            { ItemSlot.POTION, new PlayerItem(){
                Slot = ItemSlot.POTION,
                Name = "Periapt of Fortitude",
                Description = "Gleaming golden elephant"
            } }
        });
    }
    int PERVITALITY_BonusPotionChance = 5;
    public int BASE_BonusXPChance = 15;
    public int BASE_BonusHPChance = 15;
    public int BASE_BonusAPChance = 15;
    public int BASE_BonusCoinChance = 15;
    int PERDEXTERITY_BonusShieldChance = 5;
    int PERLUCK_BonusCoinChance = 5;
    int PERSTRENGTH_BonusXPChance = 5;
    int PERROLL_BonusXP = 1;

    public int GoldGoal = 100;
    public int GearPoints { get; private set; }
    public int GearGoal = 50;
    public int ExperiencePoints { get; private set; }
    public int ExperienceGoal = 50;
    public int MonstersKilled { get; private set; }
    public int BossesKilled { get; private set; }
    public int Level { get; private set; } = 1;
    
    public bool HasReachedExperienceGoal() {
        return ExperiencePoints >= ExperienceGoal;
    }

    public bool HasReachedCoinGoal() {
        return Gold >= GoldGoal;
    }

    public bool HasReachedDefenseGoal() {
        return GearPoints >= GearGoal;
    }

    internal CollectionResult CollectShields(int shieldsCollected) {
        int bonusShieldRollCount = Math.Max(shieldsCollected - 3, 0);
        int bonusShields = doRollsAgainstChance(bonusShieldRollCount, BonusShieldChance());
        
        int armorEarned = CalcArmorGained(shieldsCollected);
        int bonusGained = CalcArmorGained(bonusShields);
        int armorToApply = armorEarned + bonusGained;

        int overheal = Math.Max((Armor + armorToApply) - CalcMaxArmor(), 0);
    
        Armor = Math.Max(Math.Min(Armor + armorToApply, CalcMaxArmor()), 0);
        GearPoints += overheal;

        return new CollectionResult() {
            Collected = shieldsCollected,
            GameTotalCollected = Armor,
            Earned = armorEarned,
            BonusRollCount = bonusShieldRollCount,
            SuccessfulBonusRollCount = bonusShields,
            BonusGained = bonusGained
        };
    }

    internal CollectionResult CollectCoins(int collected) {
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

    internal CollectionResult CollectPotions(int collected) {
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

    internal CollectionResult CollectKilledMonsters(int amt) {
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

    public int BonusXpChance() {
        return BASE_BonusXPChance + (PERSTRENGTH_BonusXPChance * TotalStats.Strength);
    }

    public int BonusCoinChance() {
        return BASE_BonusCoinChance + (PERLUCK_BonusCoinChance * TotalStats.Luck);
    }

    public int BonusHpChance() {
        return BASE_BonusHPChance + (PERVITALITY_BonusPotionChance * TotalStats.Vitality);
    }
    
    public int BonusShieldChance() {
        return BASE_BonusAPChance + (PERDEXTERITY_BonusShieldChance * TotalStats.Dexterity);
    }

    internal int SpendDownCoins() {
        return Gold -= GoldGoal;
    }

    internal int SpendDownExp() {
        Level++;
        return ExperiencePoints -= ExperienceGoal;
    }

    internal int SpendDownDefensePoints() {
        return GearPoints -= GearGoal;
    }
}
