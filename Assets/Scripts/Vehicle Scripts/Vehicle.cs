using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private Wheel[] wheels;
    [SerializeField] private Engine engine;
    [SerializeField] private Transmission transmission;
    [SerializeField] private HUD headsUp;
    [SerializeField] private Transform centerOfMass;

    private Rigidbody carRb;
    private int poweredWheelCount;
    private float currentSpeed;
    private float currentRPM;
    private float currentTime = 0.5f;

    public Rigidbody Rigidbody { get { return carRb; } }
    public int PoweredWheelCount { get { return poweredWheelCount; } private set { poweredWheelCount = value; } }
    public float CurrentRPM { get { return currentRPM; } private set { currentRPM = value; } }

    public void CalculateCurrentRPM(Wheel[] wheels)
    {
        if (Input.GetKey(KeyCode.S))
        {
            return;
        }

        float totalRPM = 0.0f;

        foreach (Wheel wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                totalRPM = (wheel.RPM() >= 0) ? wheel.RPM() : -(wheel.RPM());
            }
        }
        totalRPM *= transmission.GetMultiplier();

        CurrentRPM = Mathf.Min(totalRPM, engine.GetMaxRPM());

        if (CurrentRPM < (engine.GetMinRPM() + engine.GetIdleRange()))
        {
            CurrentRPM = EngineIdleRPM();
        }
    }

    private float EngineIdleRPM()
    {
        currentTime += Time.deltaTime * 2.5f;

        float minRPM = engine.GetMinRPM() - engine.GetIdleRange();
        float maxRPM = engine.GetMinRPM() + engine.GetIdleRange();

        float currentStep = Mathf.PingPong(currentTime, 1.0f);

        if (currentStep >= 1.0f)
        {
            currentTime = 0.0f;
        }

        return Mathf.Lerp(minRPM, maxRPM, currentStep);
    }

    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass.position;
        poweredWheelCount = CountPoweredWheels();
    }

    private int CountPoweredWheels()
    {
        PoweredWheelCount = 0;
        foreach (Wheel wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                PoweredWheelCount++;
            }
        }
        return PoweredWheelCount;
    }

    private void Update()
    {
        currentSpeed = (carRb.velocity.magnitude * 3600f) / 1609.34f;
        CalculateCurrentRPM(wheels);
        headsUp.UpdateHUD(transmission.CurrentGear, currentSpeed, CurrentRPM);
    }

    private void FixedUpdate()
    {
        foreach (Wheel wheel in wheels)
        {
            float torqueAtWheel = engine.CalculateOutputTorque(CurrentRPM, currentSpeed, PoweredWheelCount);
            wheel.OperateWheel(torqueAtWheel, currentSpeed);
        }
    }
}
