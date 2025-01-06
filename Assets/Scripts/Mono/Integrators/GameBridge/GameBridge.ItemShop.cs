using UnityEngine;
using System.Collections;
using System;

// Item Shop
public partial class GameBridge {
    bool _userPurchasingItem = false;
    ValueTuple<PlayerItem, PlayerItem> _itemSwap;

    public void PLYR_ChooseItemShopPurchaseByIndex(int index) {
        _itemSwap = Board.State.ItemShopPurchase(index);
        _userPurchasingItem = false;
    }

    IEnumerator PlayerInputYield_GoldShop() {
        var shopItems = Board.State.GetNewItemShopOptions();
        var ItemShop = FindObjectOfType<UI_ItemShop>();
        ItemShop.SetDisplayItems(shopItems);
        ItemShop.Show();
        _userPurchasingItem = true;
        while (_userPurchasingItem) {
            yield return null;
        }
        // can show the old and new items and do a performance here if we want
        // _itemSwap is set to a tuple old/new
        ItemShop.Hide();
    }
}
