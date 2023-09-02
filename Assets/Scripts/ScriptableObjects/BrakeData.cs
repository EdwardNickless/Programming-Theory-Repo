using UnityEngine;

[CreateAssetMenu(fileName = "New Brake", menuName = "Scriptable Objects/Brake")]
public class BrakeData : ScriptableObject
{
    [SerializeField] private float brakeForce;
    [SerializeField] private float fractionalDistribution;
    [SerializeField] private bool hasABS;

    public float BrakeForce { get { return brakeForce; } }
    public float FractionalDistribution { get { return fractionalDistribution; } }
    public bool HasABS { get {  return hasABS; } }
}
