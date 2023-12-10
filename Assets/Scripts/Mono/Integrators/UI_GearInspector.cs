using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class UI_GearInspector : MonoBehaviour
{
    GridGameManager _rc;
    [SerializeField] TextMeshProUGUI ItemTitle;
    [SerializeField] TextMeshProUGUI ItemDesc;
    [SerializeField] TextMeshProUGUI ItemStats;
    [SerializeField] List<GameObject> HideObjectsOnStart;

    void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
    }

    void OnEnable() {
        foreach(GameObject g in HideObjectsOnStart) {
            g.SetActive(false);
        }
        ItemTitle.text = "";
        ItemDesc.text = "";
        ItemStats.text = "";
    }

    public void InspectInventoryAtSlot(string slot) {
        ItemSlot affectedSlot = Enum.Parse<ItemSlot>(slot);
        PlayerItem item = _rc.Board.State.Player.GetItemInInventorySlot(affectedSlot);
        ItemTitle.text = item.Name.ToUpper();
        ItemDesc.text = item.Description.ToUpper();
        ItemStats.text = item.SummaryStats();
    }
}
