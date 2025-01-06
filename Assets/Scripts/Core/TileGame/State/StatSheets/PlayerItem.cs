using System.Collections.Generic;
using System;

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

    public static List<PlayerItem> GetRandomItemsForPlayerLevel(int itemCount, int level) {
        List<PlayerItem> items = new List<PlayerItem>();
        for (int i = 0; i < itemCount; i++) {
            Random random = new Random();
            items.Add(new PlayerItem() {
                Strength = random.Next(0, level + 1),
                Dexterity = random.Next(0, level + 1),
                Vitality = random.Next(0, level + 1),
                Luck = random.Next(0, level + 1),
                WeaponDamage = random.Next(0, level + 1),
                Defense = random.Next(0, level + 1),
                ArmorPiercing = random.Next(0, level + 1),
                ArmorDurability = random.Next(0, level + 1),
                HitPoints = random.Next(0, level + 1),
                Name = "Item",
                Description = "A random item",
                Slot = (ItemSlot) random.Next(0, Enum.GetValues(typeof(ItemSlot)).Length)
            });
        }
        return items;
    }
}
