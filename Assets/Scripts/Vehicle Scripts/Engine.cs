using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] private EngineData engine;
    [SerializeField] private Transmission transmission;

    private float accelerationForce;
    public float CalculateOutputTorque(float currentRPM, int poweredWheelCount)
    {
        float currentPower = CalculateCurrentPower(currentRPM);
        return (accelerationForce * transmission.CalculateOutputTorque(currentPower)) / poweredWheelCount;
    }

    private void Update()
    {
        accelerationForce = Input.GetAxisRaw("Vertical");
    }

    private float CalculateCurrentPower(float currentRPM)
    {
        return Mathf.Max(engine.IdleRPM, engine.PowerCurve.Evaluate(currentRPM));
    }
}
