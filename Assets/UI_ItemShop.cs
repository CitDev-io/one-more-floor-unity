using UnityEngine;
using TMPro;

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

    public void ToggleSelectionByIndex(int index) {
        if (selectedItemIndex == index) {
            selectedItemIndex = -1;
        } else {
            selectedItemIndex = index;
            Replaces1.setDisplayItem(_rc.Board.Player.GetItemInInventorySlot(_rc.Board.ItemShopOptions[index].Slot));
        }
    }

    void Start() {
        if (_rc.Board.ItemShopOptions != null && _rc.Board.ItemShopOptions.Length == 3) {
            Option1.setDisplayItem(_rc.Board.ItemShopOptions[0]);
            Option2.setDisplayItem(_rc.Board.ItemShopOptions[1]);
            Option3.setDisplayItem(_rc.Board.ItemShopOptions[2]);
        }
    }

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;
        confirmButton.SetActive(selectedItemIndex > -1);
        replacementItem.SetActive(selectedItemIndex > -1);
    }

    public void ClickItemShopConfirm(){
        _rc.SelectItemShopAtIndex(selectedItemIndex);
    }
}
