using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "Scriptable Objects/Engine")]
public class EngineData : ScriptableObject
{
    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField] private float idleRPM;
    [SerializeField] private float maxRPM;
    public AnimationCurve PowerCurve { get { return powerCurve; } }
    public float IdleRPM { get { return idleRPM; } }
    public float MaxRPM { get { return maxRPM; } }
}
