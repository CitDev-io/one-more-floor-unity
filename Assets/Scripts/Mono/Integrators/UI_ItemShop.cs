using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UI_ItemShop : MonoBehaviour
{
    GridGameManager _rc;
    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject replacementItem;

    [SerializeField] UI_ItemShopOptionTxter Option1;
    [SerializeField] UI_ItemShopOptionTxter Option2;
    [SerializeField] UI_ItemShopOptionTxter Option3;
    [SerializeField] UI_ItemShopOptionTxter Replaces1;
    int selectedItemIndex = -1;
    [SerializeField] bool StartVisible = false;


    void Awake() {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
        if (StartVisible) {
            Show();
        } else {
            Hide();
        }
    }

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
        Slot = ItemSlot.CLASSITEM,
        Name = "Empty",
        Description = ""
    };

    public void ToggleSelectionByIndex(int index) {
        if (selectedItemIndex == index) {
            selectedItemIndex = -1;
        } else {
            selectedItemIndex = index;
            PlayerItem i = _rc.Board.State.Player.GetItemInInventorySlot(_rc.Board.State.ItemShopOptions[index].Slot);
            
            Replaces1.setDisplayItem(i??EMPTY_PLAYER_ITEM);
        }
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
        _rc.SelectItemShopAtIndex(selectedItemIndex);
        selectedItemIndex = -1;
    }
}
