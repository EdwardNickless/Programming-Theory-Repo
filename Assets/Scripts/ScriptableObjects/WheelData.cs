using UnityEngine;

[CreateAssetMenu(fileName = "New Brake", menuName ="Scriptable Objects/Brake")]
public class WheelData : ScriptableObject
{
    [SerializeField] private float brakeEfficiency;
    [SerializeField] private bool launchControl;
    [SerializeField] private bool tractionControl;
    public float BrakeEfficiency { get { return brakeEfficiency; } }
    public bool LaunchControl { get {  return launchControl; } }
    public bool TractionControl { get { return tractionControl; } }
}