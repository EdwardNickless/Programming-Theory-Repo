using UnityEngine;

public class EngineSoundBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource pistonFiringSound;
    [SerializeField] private EngineData engineData;

    private VehicleBehaviour vehicle;

    private void Start()
    {
        vehicle = GetComponentInParent<VehicleBehaviour>();
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
