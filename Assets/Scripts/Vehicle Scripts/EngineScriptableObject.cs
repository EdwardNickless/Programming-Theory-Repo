using UnityEngine;

[System.Serializable]
public class GearRatiosDictionary : SerializableDictionary<int, float> { }

[CreateAssetMenu(fileName = "New Engine", menuName = "Scriptable Objects/Engine")]
public class EngineScriptableObject : ScriptableObject
{
    [SerializeField] private float idleRPM;
    [SerializeField] private float maxRPM;
    [SerializeField] private float idleTorque;
    [SerializeField] private float peakTorque;
    [SerializeField] private float idleHorsePower;
    [SerializeField] private float peakHorsePower;
    [SerializeField] private GearRatiosDictionary gearRatios;

    public float IdleRPM { get { return idleRPM; } }
    public float MaxRPM { get { return maxRPM; } }
    public float IdleTorque { get { return idleTorque; } }
    public float PeakTorque { get { return peakTorque; } }
    public float IdleHorsePower { get { return idleHorsePower; } }
    public float PeakHorsePower { get { return peakHorsePower; } }
    public GearRatiosDictionary GearRatios { get { return gearRatios; } }
}
