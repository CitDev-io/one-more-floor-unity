using UnityEngine;
using TMPro;

public class UI_AvatarStatFeed : MonoBehaviour
{
    GridGameManager _rc;
    TextMeshProUGUI _txt;
    [SerializeField] PlayerAvatarStatType _statType;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
        _txt = GetComponent<TextMeshProUGUI>();
    }
    private void OnGUI() {
        if (_rc.Board == null) return;
        _txt.text = _rc.Board.State.Player.GetStat(_statType) + "";
    }
}
