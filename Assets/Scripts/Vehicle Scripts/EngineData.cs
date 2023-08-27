using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "Scriptable Objects/Engine")]
public class EngineData : ScriptableObject
{
    [SerializeField] private AnimationCurve torqueCurve;
    [SerializeField] private float minRPM;
    [SerializeField] private float maxRPM;
    [SerializeField] private float idleRange;
    [SerializeField] private float redLineRange;
    [SerializeField] private int pistonCount;

    public AnimationCurve TorqueCurve { get { return torqueCurve; } }
    public float MinRPM { get { return minRPM; } }
    public float MaxRPM { get { return maxRPM; } }
    public float IdleRange { get { return idleRange; } }
    public float RedLineRange { get { return redLineRange; } }
    public int PistonCount { get { return pistonCount; } }
}
