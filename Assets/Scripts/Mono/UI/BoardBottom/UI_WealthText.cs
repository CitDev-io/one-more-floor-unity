using UnityEngine;
using TMPro;

public class UI_WealthText : MonoBehaviour
{
    GridGameManager _rc;
    TextMeshProUGUI _txt;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
        _txt = GetComponent<TextMeshProUGUI>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;
        _txt.text = _rc.Board.Player.Gold + " / " + _rc.Board.Player.GoldGoal;
    }
}
