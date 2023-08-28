using UnityEngine;

public class EngineSound : MonoBehaviour
{
    [SerializeField] private AudioSource pistonFiringSound;
    [SerializeField] private EngineData engineData;

    private Vehicle vehicle;

    private void Start()
    {
        vehicle = GetComponentInParent<Vehicle>();
    }

    private void Update()
    {
        ChangePitch();
    }

    private void ChangePitch()
    {
        pistonFiringSound.pitch = CalculatePitch();
    }

    private float CalculatePitch()
    {
        return vehicle.CurrentRPM / engineData.PistonCount / 1000.0f;
    }
}
