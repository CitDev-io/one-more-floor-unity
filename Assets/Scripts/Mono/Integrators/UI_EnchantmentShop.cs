using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class UI_EnchantmentShop : MonoBehaviour
{
    GameBridge _rc;
    [SerializeField] GameObject confirmButton;

    [SerializeField] UI_EnchantmentShopOptionTxter Option1;
    [SerializeField] UI_EnchantmentShopOptionTxter Option2;
    [SerializeField] UI_EnchantmentShopOptionTxter Option3;
    [SerializeField] UI_EnchantmentShopOptionTxter Option4;
    int selectedItemIndex = -1;

    public void ToggleSelectionByIndex(int index) {
        if (selectedItemIndex == index) {
            selectedItemIndex = -1;
        } else {
            selectedItemIndex = index;
        }
    }

    public void SetDisplayItems(List<ValueTuple<ItemSlot, StatMatrix>> items) {
        Option1.setDisplayItem(items[0]);
        Option2.setDisplayItem(items[1]);
        Option3.setDisplayItem(items[2]);
        Option4.setDisplayItem(items[3]);
    }

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GameBridge>();
        Hide();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;
        confirmButton.SetActive(selectedItemIndex > -1);
    }

    public void ClickItemShopConfirm(){
        _rc.PLYR_ChooseEnchantShopPurchaseByIndex(selectedItemIndex);
        selectedItemIndex = -1;
        Hide();
    }

        /* show and hide can be incorporated into the ui_toggle-active component */
    public void Show() {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void Hide() {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
