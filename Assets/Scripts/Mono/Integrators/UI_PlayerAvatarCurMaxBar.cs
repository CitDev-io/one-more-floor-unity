using System;
using UnityEngine;

public class UI_PlayerAvatarCurMaxBar : MonoBehaviour
{
    GridGameManager _rc;
    [SerializeField] int fullWidth = 172;
    [SerializeField] PlayerAvatarCurMaxType _curMaxType;

    private void Awake()
    {
        _rc = FindObjectOfType<GridGameManager>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;

        Tuple<int, int> curmax = _rc.Board.State.Player.GetCurMax(_curMaxType);
        float newWidth = ((float)  curmax.Item1 / curmax.Item2) * fullWidth;

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
    }
}
