using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemShopOptionTxter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemTitleTxt;
    [SerializeField] TextMeshProUGUI ItemDescTxt;
    [SerializeField] TextMeshProUGUI ItemStatTxt;
    [SerializeField] Image ItemIcon;


    public void setDisplayItem(PlayerItem? i) {
        if (i == null) {
            ItemTitleTxt.text = "";
            ItemDescTxt.text = "";
            ItemStatTxt.text = "";
            return;
        }
        ItemTitleTxt.text = i.Name;
        ItemDescTxt.text = i.Description;
        ItemStatTxt.text = i.SummaryStats();
        Debug.Log(i.SummaryStats() + " " + i.Strength + " " + i.Dexterity + " " + i.Defense + " " + i.HitPoints);
        ItemIcon.sprite = Resources.Load<Sprite>($"Gear/{i.Slot.ToString().ToUpper()}X");
    }
}
