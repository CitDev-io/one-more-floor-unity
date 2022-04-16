using UnityEngine;
using TMPro;

public class UI_StatDump : MonoBehaviour
{
    GridGameManager _rc;
    TextMeshProUGUI _txt;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
        _txt = GetComponent<TextMeshProUGUI>();
    }
    private void OnGUI() {
        if (_rc.Board == null) return;

        var ss = _rc.Board.Player;
        _txt.text = $"STR={ss.Strength}, DEX={ss.Dexterity}, VIT={ss.Vitality}," +
        $" LCK={ss.Luck}, WDMG={ss.WeaponDamage}, APRC={ss.ArmorPiercing}, ADUR={ss.ArmorDurability}," +
        $" DEF={ss.Defense}";
    }
}
