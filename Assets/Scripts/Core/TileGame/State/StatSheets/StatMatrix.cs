public class StatMatrix
{

    // followed the thread of having enum mapped stats in a dictionary. it's wordy in syntax, but it's beautiful 
    // as something easily read and maintained!
    public int Strength = 0;
    public int Dexterity = 0;
    public int Vitality = 0;
    public int Luck = 0;
    public int WeaponDamage = 0;
    public int Defense = 0;
    public int ArmorPiercing = 0;
    public int ArmorDurability = 0;
    public int HitPoints = 0;
    public int BaseDamage = 0;
    public StatMatrix Clone() {
        return Reduce(this);
    }

    public static StatMatrix BASE_MONSTER() {
        return new StatMatrix(){
            Strength = 0,
            Dexterity = 0,
            Vitality = 0,
            Luck = 0,
            WeaponDamage = 0,
            Defense = 0,
            ArmorDurability = 50,
            ArmorPiercing = 0,
            HitPoints = 0,
            BaseDamage = 0
        };
    }

    public static StatMatrix BASE_PLAYER() {
        return new StatMatrix(){
            Strength = 1,
            Dexterity = 1,
            Vitality = 1,
            Luck = 1,
            WeaponDamage = 0,
            Defense = 1,
            ArmorDurability = 50,
            ArmorPiercing = 50,
            HitPoints = 0,
            BaseDamage = 1
        };
    }
    public static StatMatrix Reduce(params StatMatrix[] args) {
        StatMatrix result = new StatMatrix();

        foreach(StatMatrix m in args) {
            result.Strength += m.Strength;
            result.Dexterity += m.Dexterity;
            result.Vitality += m.Vitality;
            result.Luck += m.Luck;
            result.WeaponDamage += m.WeaponDamage;
            result.Defense += m.Defense;
            result.ArmorPiercing += m.ArmorPiercing;
            result.ArmorDurability += m.ArmorDurability;
            result.HitPoints += m.HitPoints;
            result.BaseDamage += m.BaseDamage;
        }

        return result;
    }

    public string SummaryStats() {
        string s = "";
        if (Strength > 0) s += $"STR {Strength} ";
        if (Dexterity > 0) s += $"DEX {Dexterity} ";
        if (Vitality > 0) s += $"VIT {Vitality} ";
        if (Luck > 0) s += $"LUC {Luck} ";
        if (WeaponDamage > 0) s += $"DMG {WeaponDamage} ";
        if (Defense > 0) s += $"DEF {Defense} ";
        if (ArmorDurability > 0) s += $"DUR {ArmorDurability} ";
        if (ArmorPiercing > 0) s += $"PRC {ArmorPiercing} ";
        if (HitPoints > 0) s += $"HP {HitPoints} ";
        if (BaseDamage > 0) s += $"BDMG {BaseDamage}";
        return s;
    }
}
