using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable: CurveData", menuName = "Game Data/Curve Data", order = 1)]
public class CurveDataSO : ScriptableObject, ICurveData
{
    [SerializeField]
    private string label;

    [SerializeField]
    private AnimationCurve curve;

    public string Label => label;

    public float Evaluate(float time)
    {
        return curve.Evaluate(time);
    }
}