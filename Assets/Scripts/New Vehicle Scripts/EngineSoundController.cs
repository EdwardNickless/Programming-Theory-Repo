using UnityEngine;

public class EngineSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource pistonFiringSound;
    [SerializeField] private EngineController engine;

    private void Update()
    {
        ChangePitch();
    }

    private void ChangePitch()
    {
        pistonFiringSound.pitch = engine.CurrentRPM / 1000.0f / engine.GetPistonCount();
    }
}

