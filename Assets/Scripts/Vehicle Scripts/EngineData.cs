using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "Scriptable Objects/Engine")]
public class EngineData : ScriptableObject
{
    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField] private float idleRPM;
    public AnimationCurve PowerCurve { get { return powerCurve; } private set { powerCurve = value; } }
    public float IdleRPM { get { return idleRPM; } private set { idleRPM = value; } }
}
