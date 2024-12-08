using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealthBar : MonoBehaviour
{
    GridGameManager _rc;
    int fullHeight = 50;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;

        // assuming fullHeight is 50...
        float newHeight = (float)(_rc.Board.State.Player.Hp / _rc.Board.State.Player.CalcMaxHp()) * 50;

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, newHeight);
    }
}
