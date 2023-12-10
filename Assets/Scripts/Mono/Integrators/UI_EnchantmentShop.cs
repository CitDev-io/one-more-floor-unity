using UnityEngine;
using TMPro;

public class UI_EnchantmentShop : MonoBehaviour
{
    GridGameManager _rc;
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

    void OnEnable() {
        // // if (_rc.Board.EnchantmentShopOptions != null && _rc.Board.EnchantmentShopOptions.Length == 4) {
        //     Option1.setDisplayItem(_rc.Board.EnchantmentShopOptions[0]);
        //     Option2.setDisplayItem(_rc.Board.EnchantmentShopOptions[1]);
        //     Option3.setDisplayItem(_rc.Board.EnchantmentShopOptions[2]);
        //     Option4.setDisplayItem(_rc.Board.EnchantmentShopOptions[3]);
        // }
    }

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;
        confirmButton.SetActive(selectedItemIndex > -1);
    }

    public void ClickItemShopConfirm(){
        // _rc.SelectionEnchantmentShopOptionAtIndex(selectedItemIndex);
        selectedItemIndex = -1;
    }
}
