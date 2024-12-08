public class PlayerItem: StatMatrix
{
    public string Name;
    public string Description;
    public ItemSlot Slot;

    
    public static PlayerItem Reduce(params PlayerItem[] args) {
        PlayerItem result = new PlayerItem();

        foreach(PlayerItem m in args) {
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
            result.Slot = m.Slot;
        }

        return result;
    }
}
