using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemShopOptionTxter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemTitleTxt;
    [SerializeField] TextMeshProUGUI ItemDescTxt;
    [SerializeField] TextMeshProUGUI ItemStatTxt;
    [SerializeField] TextMeshProUGUI ItemEnchantTxt;
    [SerializeField] Image ItemIcon;


    public void setDisplayItem(PlayerItem? i) {
        if (i == null) {
            ItemTitleTxt.text = "";
            ItemDescTxt.text = "";
            ItemStatTxt.text = "";
            ItemEnchantTxt.text = "";
            return;
        }
        ItemTitleTxt.text = i.Name;
        ItemDescTxt.text = i.Description;
        ItemStatTxt.text = i.SummaryStats();
        ItemEnchantTxt.text = i.Enchantment.SummaryStats();
        Debug.Log(i.SummaryStats() + " " + i.Strength + " " + i.Dexterity + " " + i.Defense + " " + i.HitPoints);
        ItemIcon.sprite = Resources.Load<Sprite>($"Gear/{i.Slot.ToString().ToUpper()}X");
    }
}
