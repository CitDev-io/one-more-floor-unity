using UnityEngine;

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
        if (_rc.Board == null) return;
        double percentageToGoal = _rc.Board.Kills / (double) _rc.Board.KillRequirement;
        double width = percentageToGoal * 130f;
        percentage = percentageToGoal;
        _width = width;
        _transform.sizeDelta = new Vector2((float)width, _transform.sizeDelta.y);
    }
}
