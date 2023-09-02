using UnityEngine;

[CreateAssetMenu(fileName = "New Axle", menuName = "Scriptable Objects/Axle")]
public class AxleData : ScriptableObject
{
    [SerializeField] private bool isDriveAxle;
    [SerializeField] private bool hasLaunchControl;

    public bool IsDriveAxle { get { return isDriveAxle; } }
    public bool HasLaunchControl { get {  return hasLaunchControl; } }
}
