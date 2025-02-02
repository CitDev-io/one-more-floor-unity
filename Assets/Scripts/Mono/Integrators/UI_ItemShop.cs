using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UI_ItemShop : MonoBehaviour
{
    GameBridge _rc;
    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject replacementItem;

    [SerializeField] UI_ItemShopOptionTxter Option1;
    [SerializeField] UI_ItemShopOptionTxter Option2;
    [SerializeField] UI_ItemShopOptionTxter Option3;
    [SerializeField] UI_ItemShopOptionTxter Replaces1;
    int selectedItemIndex = -1;


    void Awake() {
        _rc = GameObject.FindObjectOfType<GameBridge>();
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
    PlayerItem EMPTY_PLAYER_ITEM = new PlayerItem(){
        Slot = ItemSlot.WEAPON,
        Name = "Empty",
        Description = ""
    };

    public void ToggleSelectionByIndex(int index) {
        // if (selectedItemIndex == index) {
        //     selectedItemIndex = -1;
        //     Replaces1.setDisplayItem(null);
        // } else {
            selectedItemIndex = index;
            PlayerItem i = _rc.Board.State.Player.GetItemInInventorySlot(_rc.Board.State.ItemShopOptions[index].Slot);
            
            Replaces1.setDisplayItem(i??EMPTY_PLAYER_ITEM);
        // }
    }

    public void SetDisplayItems(List<PlayerItem> items) {
        Option1.setDisplayItem(items[0]);
        Option2.setDisplayItem(items[1]);
        Option3.setDisplayItem(items[2]);
    }

    private void OnGUI()
    {
        if (_rc.Board == null) return;
        confirmButton.SetActive(selectedItemIndex > -1);
        replacementItem.SetActive(selectedItemIndex > -1);
    }

    public void ClickItemShopConfirm(){
        _rc.PLYR_ChooseItemShopPurchaseByIndex(selectedItemIndex);
        selectedItemIndex = -1;
        Hide();
    }
}
