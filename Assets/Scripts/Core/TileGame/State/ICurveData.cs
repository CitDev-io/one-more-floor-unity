public interface ICurveData
{
    string Label { get; }
    float Evaluate(float time);
} 