using UnityEngine;
using System.Collections.Generic;

public class UI_XPShop : MonoBehaviour
{
    GridGameManager _rc;
    [SerializeField] GameObject confirmButton;

    [SerializeField] List<UI_XPShopOptionTxter> Options;
    List<int> selectedItemIndexes = new List<int>();

    public void ToggleSelectionByIndex(int index) {
        if (selectedItemIndexes.Contains(index)) {
            selectedItemIndexes.Remove(index);
            Options[index].SetSelected(false);
            return;
        }

        if (selectedItemIndexes.Count < 2) {
            selectedItemIndexes.Add(index);
            Options[index].SetSelected(true);
        }
    }

    void OnEnable() {
        if (_rc.Board.XPShopOptions != null && _rc.Board.XPShopOptions.Length == 4) {
            selectedItemIndexes.Clear();
            Options[0].setDisplayOption(_rc.Board.XPShopOptions[0]);
            Options[1].setDisplayOption(_rc.Board.XPShopOptions[1]);
            Options[2].setDisplayOption(_rc.Board.XPShopOptions[2]);
            Options[3].setDisplayOption(_rc.Board.XPShopOptions[3]);
        }
    }

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;
        confirmButton.SetActive(selectedItemIndexes.Count == 2);
    }

    public void ClickXPShopConfirm(){
        _rc.SelectionXPShopOptionsAtIndexes(selectedItemIndexes);
        selectedItemIndexes.Clear();
    }
}
