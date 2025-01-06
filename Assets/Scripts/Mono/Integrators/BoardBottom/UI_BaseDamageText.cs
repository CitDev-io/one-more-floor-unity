using UnityEngine;
using TMPro;

public class UI_BaseDamageText : MonoBehaviour
{
    GameBridge _rc;
    TextMeshProUGUI _txt;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GameBridge>();
        _txt = GetComponent<TextMeshProUGUI>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;
        _txt.text = "+" + _rc.Board.State.Player.CalcBaseDamage();
    }
}
