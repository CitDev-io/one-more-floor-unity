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
        primaryStatText.text = $"{_rc.Board.Player.Strength}\n\n\n" +
        $"{_rc.Board.Player.Dexterity}\n\n\n" +
        $"{_rc.Board.Player.Vitality}\n\n\n" +
        $"{_rc.Board.Player.Luck}\n\n\n" +
        $"{_rc.Board.Player.WeaponDamage}\n\n" +
        $"{_rc.Board.Player.Defense}";
        strStatsText.text = $"{_rc.Board.Player.CalcBaseDamage()}\n" +
        $"{_rc.Board.Player.BonusXpChance()}";
        dexStatsText.text = $"{_rc.Board.Player.CalcArmorGained(1)}\n" +
        $"{_rc.Board.Player.BonusShieldChance()}";
        vitStatsText.text = $"{_rc.Board.Player.CalcMaxHp()}\n" +
        $"{_rc.Board.Player.BonusHpChance()}";
        lucStatsText.text = $"{_rc.Board.Player.CalcHealPerPotion()}\n" +
        $"{_rc.Board.Player.BonusCoinChance()}";
        wdmStatsText.text = $"{_rc.Board.Player.ArmorPiercing}%";
        defStatsText.text = $"{_rc.Board.Player.ArmorDurability}%";
        lvlText.text = $"Level {_rc.Board.Player.Level}";
    }
}
