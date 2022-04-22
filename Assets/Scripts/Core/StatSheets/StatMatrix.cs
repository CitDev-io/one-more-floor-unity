public class StatMatrix
{
    public int Strength = 0;
    public int Dexterity = 0;
    public int Vitality = 0;
    public int Luck = 0;
    public int WeaponDamage = 0;
    public int Defense = 0;
    public int ArmorPiercing = 0;
    public int ArmorDurability = 0;

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
            ArmorPiercing = 0
        };
    }

    public static StatMatrix BASE_PLAYER() {
        return new StatMatrix(){
            Strength = 1,
            Dexterity = 1,
            Vitality = 1,
            Luck = 1,
            WeaponDamage = 1,
            Defense = 4,
            ArmorDurability = 50,
            ArmorPiercing = 50
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
        }

        return result;
    }
}
