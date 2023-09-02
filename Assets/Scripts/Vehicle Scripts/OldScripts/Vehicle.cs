using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private Engine engine;
    [SerializeField] private Transmission transmission;
    [SerializeField] private Wheel[] wheels;

    private Rigidbody carRb;

    private int currentGear;
    private int currentSpeed;
    private float currentRPM;
    private float kerbWeight;
    private float downForce;
    private float downForceMultiplier;

    public Rigidbody Rigidbody { get { return carRb; } }
    public int CurrentGear { get { return currentGear; } private set { currentGear = value; } }
    public int CurrentSpeed { get { return currentSpeed; } private set { currentSpeed = value; } }
    public float CurrentRPM { get { return currentRPM; } private set { currentRPM = value; } }
    public float KerbWeight { get { return kerbWeight; } private set { kerbWeight = value; } }
    public float DownForce { get { return downForce; } private set { downForce = value; } }

    public int MaxSpeed { get; private set; }
    public float PoweredWheels { get; private set; }
    public bool IsGrounded { get; private set; }


    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass.position;
        CalculateKerbWeight();
        downForceMultiplier = 0.01f;
        MaxSpeed = engine.GetMaxSpeed();
        PoweredWheels = CountPoweredWheels();
    }

    private float CountPoweredWheels()
    {
        int count = 0;
        foreach (var wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                count++;
            }
        }
        return count;
    }

    private void Update()
    {
        CurrentRPM = engine.CalculateCurrentRPM(wheels);
        CurrentGear = transmission.CurrentGear;
        CurrentSpeed = Mathf.RoundToInt((carRb.velocity.magnitude * 3600f) / 1609.34f);
        IsGrounded = CheckGroundContact();
        DownForce = CurrentSpeed * kerbWeight * downForceMultiplier;
        SimulateDownForce();
    }

    private bool CheckGroundContact()
    {
        int count = 0;
        foreach (Wheel wheel in wheels)
        {
            if (wheel.Collider.isGrounded)
            {
                count++;
            }
        }
        return (count == 4);
    }

    private void CalculateKerbWeight()
    {
        KerbWeight = carRb.mass;
        foreach (Wheel wheel in wheels)
        {
            KerbWeight += wheel.Collider.mass;
        }
    }

    private void SimulateDownForce()
    {
        if (IsGrounded)
        {
            carRb.AddForce(DownForce * Vector3.down);
        }
    }
}
