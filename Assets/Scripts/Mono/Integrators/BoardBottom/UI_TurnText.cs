using UnityEngine;
using TMPro;

public class UI_TurnText : MonoBehaviour
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

        var b = _rc.Board;
        _txt.text = $"TURN {b.State.MovesMade + 1}";
    }
}
