using UnityEngine;

public class EngineSound : MonoBehaviour
{
    [SerializeField] private AudioSource pistonFiringSound;
    [SerializeField] private Engine engine;

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
        float targetPitch = vehicle.CurrentRPM / 1000.0f / engine.GetPistonCount();
        float pitchModifier = targetPitch * 0.01f;
        return Mathf.PingPong(targetPitch-pitchModifier, targetPitch+pitchModifier);
    }
}
