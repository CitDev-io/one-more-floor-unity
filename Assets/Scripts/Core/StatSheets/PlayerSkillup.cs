public class PlayerSkillup: StatMatrix
{
    public string Name;
    public string Description;

    
    public static PlayerSkillup Reduce(params PlayerSkillup[] args) {
        PlayerSkillup result = new PlayerSkillup();

        foreach(PlayerSkillup m in args) {
            result.Strength += m.Strength;
            result.Dexterity += m.Dexterity;
            result.Vitality += m.Vitality;
            result.Luck += m.Luck;
            result.WeaponDamage += m.WeaponDamage;
            result.Defense += m.Defense;
            result.ArmorPiercing += m.ArmorPiercing;
            result.ArmorDurability += m.ArmorDurability;
            result.HitPoints += m.HitPoints;
            result.Name = m.Name;
            result.Description = m.Description;
        }

        return result;
    }
}
