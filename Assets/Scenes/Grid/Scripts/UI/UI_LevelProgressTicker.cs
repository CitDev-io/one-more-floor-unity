using UnityEngine;
using TMPro;

namespace citdev {
public class UI_LevelProgressTicker : MonoBehaviour
{
    GridGameManager _rc;
    RectTransform _transform;

    public double _width;
    public double percentage;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
        _transform = GetComponent<RectTransform>();
    }
    private void OnGUI()
    {
        double percentageToGoal = _rc.Kills / (double) _rc.KillRequirement;
        double width = percentageToGoal * 130f;
        percentage = percentageToGoal;
        _width = width;
        _transform.sizeDelta = new Vector2((float)width, _transform.sizeDelta.y);
    }
}
}
