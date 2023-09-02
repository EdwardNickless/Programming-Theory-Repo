using UnityEngine;

public class VehicleBehaviour : MonoBehaviour
{
    [SerializeField] private Transform centerOfMass;

    private Rigidbody carRb;
    private EngineBehaviour engine;
    private TransmissionBehaviour transmission;
    private AxleBehaviour[] axles;
    private WheelBehaviour[] wheels;

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

    private void Awake()
    {
        carRb = GetComponent<Rigidbody>();
        engine = GetComponentInChildren<EngineBehaviour>();
        transmission = GetComponentInChildren<TransmissionBehaviour>();
        axles = GetComponentsInChildren<AxleBehaviour>();
        wheels = GetComponentsInChildren<WheelBehaviour>();
    }

    private void Start()
    {
        carRb.centerOfMass = centerOfMass.position;
        CalculateKerbWeight();
        downForceMultiplier = 0.01f;
        MaxSpeed = engine.GetMaxSpeed();
        PoweredWheels = CountPoweredWheels();
    }

    private float CountPoweredWheels()
    {
        int count = 0;
        foreach (AxleBehaviour axle in axles)
        {
            if (axle.IsDriveAxle)
            {
                count += 2;
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
        foreach (WheelBehaviour wheel in wheels)
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
        foreach (WheelBehaviour wheel in wheels)
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
