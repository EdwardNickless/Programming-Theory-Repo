using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] private EngineData engine;
    [SerializeField] private Transmission transmission;

    private float accelerationForce;
    public float AccelerationForce { get { return accelerationForce; } private set {  accelerationForce = value; } }
    
    public float CalculateOutputTorque(float currentRPM, float currentSpeed, int poweredWheelCount)
    {
        if (currentRPM >= (engine.MaxRPM - engine.IdleRange))
        {
            return 0.0f;
        }
        if (currentSpeed >= 144)
        {
            return 0.0f;
        }
        float currentPower = CalculateCurrentTorque(currentRPM);
        return (accelerationForce * transmission.CalculateOutputTorque(currentPower)) / poweredWheelCount;
    }

    public float CalculateResistanceTorque(float currentRPM, float currentSpeed, int poweredWheelCount)
    {
        if (currentRPM >= (engine.MaxRPM - engine.IdleRange))
        {
            return 0.0f;
        }
        if (currentSpeed >= 144)
        {
            return 0.0f;
        }
        float currentPower = CalculateCurrentTorque(currentRPM);
        return -(accelerationForce * transmission.CalculateOutputTorque(currentPower)) / poweredWheelCount;
    }

    public float GetMinRPM()
    {
        return engine.MinRPM;
    }

    public float GetMaxRPM()
    {
        return engine.MaxRPM;
    }

    public float GetIdleRange()
    {
        return engine.IdleRange;
    }

    public float GetPistonCount()
    {
        return engine.PistonCount;
    }

    private void Update()
    {
        accelerationForce = Input.GetAxisRaw("Throttle");
    }

    private float CalculateCurrentTorque(float currentRPM)
    {
        return Mathf.Max(engine.TorqueCurve.Evaluate(engine.MinRPM), engine.TorqueCurve.Evaluate(currentRPM));
    }
}
