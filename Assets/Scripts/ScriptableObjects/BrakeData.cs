using UnityEngine;

[CreateAssetMenu(fileName = "New Brake", menuName ="Scriptable Objects/Brake")]
public class BrakeData : ScriptableObject
{
    [SerializeField] private float stoppingForce;

    public float brakeEfficiency { get { return stoppingForce; } }
}