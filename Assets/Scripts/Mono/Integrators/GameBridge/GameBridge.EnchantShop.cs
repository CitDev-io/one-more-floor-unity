using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// Enchant Shop
public partial class GameBridge {
    bool _userChoosingEnchant = false;
    ValueTuple<StatMatrix, StatMatrix> _enchantSwap;

    public void PLYR_ChooseEnchantShopPurchaseByIndex(int index) {
        _enchantSwap = Board.State.EnchantShopPurchase(index);
        _userChoosingEnchant = false;
    }

    IEnumerator PlayerInputYield_EnchantShop() {
        List<ValueTuple<ItemSlot, StatMatrix>> shopItems = Board.State.GetNewEnchantShopOptions();
        var EnchantShop = FindObjectOfType<UI_EnchantmentShop>();
        EnchantShop.SetDisplayItems(shopItems);
        EnchantShop.Show();
        _userChoosingEnchant = true;
        while (_userChoosingEnchant) {
            yield return null;
        }
        // can show the old and new items and do a performance here if we want
        // _itemSwap is set to a tuple old/new
        EnchantShop.Hide();
    }
}
