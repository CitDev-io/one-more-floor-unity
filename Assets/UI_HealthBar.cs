using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealthBar : MonoBehaviour
{
    GridGameManager _rc;
    int fullWidth = 172;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;

        float newWidth = ((float) _rc.Board.State.Player.Hp / _rc.Board.State.Player.CalcMaxHp()) * fullWidth;

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
    }
}
