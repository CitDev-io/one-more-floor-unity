using UnityEngine;

public class UI_LevelProgressTicker : MonoBehaviour
{
    GameBridge _rc;
    RectTransform _transform;

    public double _width;
    public double percentage;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GameBridge>();
        _transform = GetComponent<RectTransform>();
    }
    private void OnGUI()
    {
        if (_rc.Board == null) return;
        double percentageToGoal = 50.0;
        double width = percentageToGoal * 130f;
        percentage = percentageToGoal;
        _width = width;
        _transform.sizeDelta = new Vector2((float)width, _transform.sizeDelta.y);
    }
}
