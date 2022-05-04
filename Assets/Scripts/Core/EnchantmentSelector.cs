using System.Collections;
using System.Collections.Generic;


public class EnchantmentSelector
{
    public PlayerItem[] GetEnchantmentItemsForPlayer(PlayerAvatar player) {
        return new PlayerItem[] {
            new PlayerItem() {
                Name = "Defender",
                Description = "Add 1 Defense",
                Slot = ItemSlot.CHEST,
                Defense = 1
            },
            new PlayerItem() {
                Name = "Guiding",
                Description = "Add 1 Weapon Damage",
                Slot = ItemSlot.WEAPON,
                WeaponDamage = 1
            },
            new PlayerItem() {
                Name = "Blocking",
                Description = "Add 5% Armor Durability",
                Slot = ItemSlot.SHIELD,
                ArmorDurability = 5
            },
            new PlayerItem() {
                Name = "Gracefulness",
                Description = "Add 1 Luck",
                Slot = ItemSlot.HELMET,
                Luck = 1
            },
        };
    }
}
