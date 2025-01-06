using UnityEngine;
using TMPro;

public class UI_StatDisplay : MonoBehaviour
{
    GameBridge _rc;
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
        _rc = GameObject.FindObjectOfType<GameBridge>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null || _rc.Board.State.Player == null) return;
        primaryStatText.text = $"{_rc.Board.State.Player.TotalStats.Strength}\n\n\n" +
        $"{_rc.Board.State.Player.TotalStats.Dexterity}\n\n\n" +
        $"{_rc.Board.State.Player.TotalStats.Vitality}\n\n\n" +
        $"{_rc.Board.State.Player.TotalStats.Luck}\n\n\n" +
        $"{_rc.Board.State.Player.TotalStats.WeaponDamage}\n\n" +
        $"{_rc.Board.State.Player.TotalStats.Defense}";
        strStatsText.text = $"{_rc.Board.State.Player.CalcBaseDamage()}\n" +
        $"{_rc.Board.State.Player.BonusXpChance()}";
        dexStatsText.text = $"{_rc.Board.State.Player.CalcArmorGained(1)}\n" +
        $"{_rc.Board.State.Player.BonusShieldChance()}";
        vitStatsText.text = $"{_rc.Board.State.Player.CalcMaxHp()}\n" +
        $"{_rc.Board.State.Player.BonusHpChance()}";
        lucStatsText.text = $"{_rc.Board.State.Player.CalcHealPerPotion()}\n" +
        $"{_rc.Board.State.Player.BonusCoinChance()}";
        wdmStatsText.text = $"{_rc.Board.State.Player.TotalStats.ArmorPiercing}%";
        defStatsText.text = $"{_rc.Board.State.Player.TotalStats.ArmorDurability}%";
        lvlText.text = $"Level {_rc.Board.State.Player.Level}";
    }
}
