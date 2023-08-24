using UnityEngine;

[CreateAssetMenu(fileName = "New Transmission", menuName = "Scriptable Objects/Transmission")]
public class TransmissionData : ScriptableObject
{
    [SerializeField] private GearRatiosDictionary gearRatios;
    [SerializeField] private float differentialRatio;
    [SerializeField] private float reverseGearRatio;
    public GearRatiosDictionary GearRatios { get { return gearRatios; } }
    public float DifferentialRatio { get { return differentialRatio; } }
    public float ReverseGearRatio { get { return reverseGearRatio; } }
}
