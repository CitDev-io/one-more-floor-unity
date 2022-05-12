using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_XPShopOptionTxter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemTitleTxt;
    [SerializeField] TextMeshProUGUI ItemDescTxt;
    [SerializeField] TextMeshProUGUI ItemStatTxt;
    [SerializeField] GameObject selectionCircle;


    public void SetSelected(bool isSelected) {
        selectionCircle.SetActive(isSelected);
    }

    public void setDisplayOption(PlayerSkillup i) {
        ItemTitleTxt.text = i.Name;
        ItemDescTxt.text = i.Description;
        ItemStatTxt.text = i.SummaryStats();
        SetSelected(false);
        // ItemIcon.sprite = Resources.Load<Sprite>($"Gear/{i.Slot.ToString().ToUpper()}X");
    }
}
