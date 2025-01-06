using UnityEngine;
using TMPro;

public class UI_CoinTicker : MonoBehaviour
{
    GameBridge _rc;
    TextMeshProUGUI _txt;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GameBridge>();
        _txt = GetComponent<TextMeshProUGUI>();
    }
    private void OnGUI() {
        if (_rc.Board == null) return;
        _txt.text = _rc.Board.State.Player.Gold + " / " + _rc.Board.State.Player.GoldGoal;
    }
}
