using UnityEngine;
using TMPro;

public class UI_ItemShop : MonoBehaviour
{
    GridGameManager _rc;
    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject replacementItem;

    int selectedItemIndex = -1;

    public void ToggleSelectionByIndex(int index) {
        if (selectedItemIndex == index) {
            selectedItemIndex = -1;
        } else {
            selectedItemIndex = index;
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
}
