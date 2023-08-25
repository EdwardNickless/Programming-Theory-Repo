using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private Wheel[] wheels;
    [SerializeField] private Engine engine;
    [SerializeField] private Transmission transmission;
    [SerializeField] private HUD headsUp;
    [SerializeField] private Transform centerOfMass;
    
    public float maxEnginePower;
    public float brakingForce;
    public float turnForce;

    private Rigidbody carRb;
    private int poweredWheelCount;
    private float currentSpeed;
    private float currentRPM;
    private float scalingFactor = 100;

    public int PoweredWheelCount { get { return poweredWheelCount; } private set { poweredWheelCount = value; } }
    public float CurrentRPM { get { return currentRPM; } private set { currentRPM = value; } }

    public void CalculateCurrentRPM(Wheel[] wheels, int poweredWheelCount)
    {
        float totalRPM = 0.0f;

        foreach (Wheel wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                totalRPM += wheel.RPM();
            }
        }

        CurrentRPM = (totalRPM >= 0) ? CalculateRPMScaled(poweredWheelCount, totalRPM) : -(CalculateRPMScaled(poweredWheelCount, totalRPM));

    }

    private float CalculateRPMScaled(int poweredWheelCount, float totalRPM)
    {
        return (totalRPM / poweredWheelCount) * transmission.GetMultiplier() / scalingFactor;
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
        turnForce = Input.GetAxisRaw("Steer");
        currentSpeed = (carRb.velocity.magnitude * 3600f) / 1609.34f;
        CalculateCurrentRPM(wheels, PoweredWheelCount);
        headsUp.UpdateHUD(transmission.CurrentGear, currentSpeed, CurrentRPM);
    }

    private void FixedUpdate()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                float torqueAtWheel = engine.CalculateOutputTorque(CurrentRPM, PoweredWheelCount);
                wheel.OperateWheel(torqueAtWheel, brakingForce, currentSpeed, turnForce);
            }
        }
    }
}
