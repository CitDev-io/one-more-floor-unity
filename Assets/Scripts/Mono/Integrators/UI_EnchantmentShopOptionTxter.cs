using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnchantmentShopOptionTxter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemTitleTxt;
    [SerializeField] TextMeshProUGUI ItemDescTxt;
    [SerializeField] TextMeshProUGUI ItemStatTxt;
    [SerializeField] Image ItemIcon;


    public void setDisplayItem(ValueTuple<ItemSlot, StatMatrix> i) {
        ItemTitleTxt.text = "Enchantment";
        ItemDescTxt.text = "What it does";
        ItemStatTxt.text = i.Item2.SummaryStats();
        ItemIcon.sprite = Resources.Load<Sprite>($"Gear/{i.Item1.ToString().ToUpper()}X");
    }
}
