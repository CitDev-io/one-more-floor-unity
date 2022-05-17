using UnityEngine;
using TMPro;

public class UI_StatDisplay : MonoBehaviour
{
    GridGameManager _rc;
    [SerializeField] TextMeshProUGUI primaryStatText;
    [SerializeField] TextMeshProUGUI strStatsText;
    [SerializeField] TextMeshProUGUI dexStatsText;
    [SerializeField] TextMeshProUGUI vitStatsText;
    [SerializeField] TextMeshProUGUI lucStatsText;
    [SerializeField] TextMeshProUGUI wdmStatsText;
    [SerializeField] TextMeshProUGUI defStatsText;
    [SerializeField] TextMeshProUGUI lvlText;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null || _rc.Board.Player == null) return;
        primaryStatText.text = $"{_rc.Board.Player.TotalStats.Strength}\n\n\n" +
        $"{_rc.Board.Player.TotalStats.Dexterity}\n\n\n" +
        $"{_rc.Board.Player.TotalStats.Vitality}\n\n\n" +
        $"{_rc.Board.Player.TotalStats.Luck}\n\n\n" +
        $"{_rc.Board.Player.TotalStats.WeaponDamage}\n\n" +
        $"{_rc.Board.Player.TotalStats.Defense}";
        strStatsText.text = $"{_rc.Board.Player.CalcBaseDamage()}\n" +
        $"{_rc.Board.Player.BonusXpChance()}";
        dexStatsText.text = $"{_rc.Board.Player.CalcArmorGained(1)}\n" +
        $"{_rc.Board.Player.BonusShieldChance()}";
        vitStatsText.text = $"{_rc.Board.Player.CalcMaxHp()}\n" +
        $"{_rc.Board.Player.BonusHpChance()}";
        lucStatsText.text = $"{_rc.Board.Player.CalcHealPerPotion()}\n" +
        $"{_rc.Board.Player.BonusCoinChance()}";
        wdmStatsText.text = $"{_rc.Board.Player.TotalStats.ArmorPiercing}%";
        defStatsText.text = $"{_rc.Board.Player.TotalStats.ArmorDurability}%";
        lvlText.text = $"Level {_rc.Board.Player.Level}";
    }
}
