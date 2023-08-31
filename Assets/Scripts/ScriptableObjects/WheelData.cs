using UnityEngine;

[CreateAssetMenu(fileName = "New Wheel", menuName ="Scriptable Objects/Wheel")]
public class WheelData : ScriptableObject
{
    [SerializeField] private float brakeEfficiency;
    [SerializeField] private bool launchControl;
    [SerializeField] private bool tractionControl;
    [SerializeField] private float asymptoteMultiplier;
    public float BrakeEfficiency { get { return brakeEfficiency; } }
    public bool LaunchControl { get {  return launchControl; } }
    public bool TractionControl { get { return tractionControl; } }
    public float AsymptoteMultiplier { get {  return asymptoteMultiplier; } }
}