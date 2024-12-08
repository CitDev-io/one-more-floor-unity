using System;
using System.Collections.Generic;

public class ItemSelector 
{
    public PlayerItem[] GetShopItemsForPlayer(PlayerAvatar player) {
        return new PlayerItem[]{
            new PlayerItem(){
                Slot = ItemSlot.CHEST,
                Name = "Smelly Jerkin",
                Description = "Defend yourself with the power of odor",
                Defense = player.GetItemInInventorySlot(ItemSlot.CHEST).Defense + 1
            },
            new PlayerItem(){
                Slot = ItemSlot.WEAPON,
                Name = "Less Moldy Sword",
                Description = "It's still wooden and moldy... but less so",
                WeaponDamage = player.GetItemInInventorySlot(ItemSlot.WEAPON).WeaponDamage + 1
            },
            new PlayerItem(){
                Slot = ItemSlot.SHIELD,
                Name = "Wooden Shield",
                Description = "At least its something",
                Defense = player.GetItemInInventorySlot(ItemSlot.SHIELD).Defense + 1
            }
        };
    }
    
}
