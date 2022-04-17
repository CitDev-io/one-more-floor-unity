using UnityEngine;
using TMPro;

public class UI_StatDump2 : MonoBehaviour
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
        _txt.text = $"XP={ss.ExperiencePoints}/{ss.ExperienceGoal}, " +
        $"GEAR={ss.GearPoints}/{ss.GearGoal}, COIN={ss.Gold}/{ss.GoldGoal}";
    }
}
