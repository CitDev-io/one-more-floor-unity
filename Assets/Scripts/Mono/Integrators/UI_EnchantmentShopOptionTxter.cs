using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnchantmentShopOptionTxter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemTitleTxt;
    [SerializeField] TextMeshProUGUI ItemDescTxt;
    [SerializeField] TextMeshProUGUI ItemStatTxt;
    [SerializeField] Image ItemIcon;


    public void setDisplayItem(PlayerItem i) {
        ItemTitleTxt.text = i.Name;
        ItemDescTxt.text = i.Description;
        ItemStatTxt.text = i.SummaryStats();
        ItemIcon.sprite = Resources.Load<Sprite>($"Gear/{i.Slot.ToString().ToUpper()}X");
    }
}
