using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;
    [SerializeField] private Transmission transmission;
    [SerializeField] private WheelData wheelData;
    [SerializeField] private AnimationCurve steeringCurve;
    [SerializeField] private Transform wheelMeshTransform;
    [SerializeField] private bool isPowered;
    [SerializeField] private bool isSteered;

    private WheelCollider wheelCollider;
    private float forwardAsymptoteSlip;
    private float sidewaysAsymptoteSlip;
    private bool hasTractionControl;
    private float asymptoteMultiplier;
    private WheelFrictionCurve forwardFriction;
    private WheelFrictionCurve sidewaysFriction;

    public bool IsPowered { get { return isPowered; } private set { isPowered = value; } }
    public WheelCollider Collider { get { return wheelCollider; } }

    public float RPM()
    {
        return wheelCollider.rpm;
    }

    private void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
        hasTractionControl = wheelData.TractionControl;

        forwardAsymptoteSlip = wheelCollider.forwardFriction.asymptoteSlip;
        sidewaysAsymptoteSlip = wheelCollider.sidewaysFriction.asymptoteSlip;
        
        asymptoteMultiplier = wheelData.AsymptoteMultiplier;
        forwardFriction = wheelCollider.forwardFriction;
        sidewaysFriction = wheelCollider.sidewaysFriction;
    }

    private void Update()
    {
        wheelCollider.motorTorque = Accelerate(Input.GetAxisRaw("Throttle"));
        wheelCollider.brakeTorque = Brake(Input.GetAxisRaw("BrakePedal"));
        wheelCollider.steerAngle = Steer(Input.GetAxisRaw("Steer"));
        ApplyTractionControl();
    }

    private void ApplyTractionControl()
    {
        if (!hasTractionControl)
        {
            return;
        }
        if (vehicle.CurrentSpeed == 0)
        {
            return;
        }
        if (Input.GetAxisRaw("Steer") == 0)
        {
            return;
        }

        float linearWheelSpeed = wheelCollider.rpm * (wheelCollider.radius * Mathf.PI * 2.0f) / 60.0f;
        float currentSlip = Mathf.Abs(vehicle.CurrentSpeed - linearWheelSpeed);

        if (currentSlip > wheelCollider.forwardFriction.extremumSlip)
        {
            forwardFriction.asymptoteSlip *= asymptoteMultiplier;
            sidewaysFriction.asymptoteSlip *= asymptoteMultiplier;
            wheelCollider.forwardFriction = forwardFriction;
            wheelCollider.sidewaysFriction = sidewaysFriction;
        }
        else
        {
            forwardFriction.asymptoteSlip = forwardAsymptoteSlip;
            sidewaysFriction.asymptoteSlip = sidewaysAsymptoteSlip;
            wheelCollider.forwardFriction = forwardFriction;
            wheelCollider.sidewaysFriction = sidewaysFriction;
        }
    }

    private float Accelerate(float throttle)
    {
        if (!isPowered)
        {
            return 0.0f;
        }
        if (vehicle.CurrentSpeed >= vehicle.MaxSpeed)
        {
            return 0.0f;
        }
        float torqueApplied = LaunchControl(throttle * transmission.DriveshaftTorque);
        return torqueApplied / vehicle.PoweredWheels;
    }

    private float LaunchControl(float torqueAtWheel)
    {
        if (!wheelData.LaunchControl)
        {
            return torqueAtWheel;
        }
        if ((wheelCollider.rotationSpeed) > 1080)
        {
            return torqueAtWheel * 0.7f;
        }
        if ((wheelCollider.rotationSpeed) > 720)
        {
            return torqueAtWheel * 0.8f;
        }
        if ((wheelCollider.rotationSpeed) > 360)
        {
            return torqueAtWheel * 0.9f;
        }
        return torqueAtWheel;
    }

    private float Brake(float brakePedal)
    {
        if (brakePedal < 0.1f)
        {
            return 0.0f;
        }
        float brakingForce = (vehicle.DownForce * 0.1f) + (vehicle.KerbWeight * wheelData.BrakeEfficiency);
        if (!isPowered)
        {
            return (brakePedal * brakingForce) * 0.3f;
        }
        return brakePedal * brakingForce;
    }

    private float Steer(float steerAngle)
    {
        if (!isSteered)
        {
            return 0.0f;
        }
        float turnForce = steeringCurve.Evaluate(vehicle.CurrentSpeed);
        return steerAngle * turnForce;
    }

    private void LateUpdate()
    {
        AnimateWheelMesh();
    }

    public void AnimateWheelMesh()
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelMeshTransform.position = position;
        wheelMeshTransform.rotation = rotation;
    }
}
