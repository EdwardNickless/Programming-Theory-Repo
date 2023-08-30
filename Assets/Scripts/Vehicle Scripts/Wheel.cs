using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;
    [SerializeField] private Transmission transmission;
    [SerializeField] private BrakeData brakeData;
    [SerializeField] private AnimationCurve wheelCurve;
    [SerializeField] private Transform wheelMeshTransform;
    [SerializeField] private bool isPowered;
    [SerializeField] private bool isSteered;

    private WheelCollider wheelCollider;

    public bool IsPowered { get { return isPowered; } private set { isPowered = value; } }
    public WheelCollider Collider { get { return wheelCollider; } }

    public float RPM()
    {
        return wheelCollider.rpm;
    }

    private void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        wheelCollider.motorTorque = Accelerate(Input.GetAxisRaw("Throttle"));
        wheelCollider.brakeTorque = Brake(Input.GetAxisRaw("BrakePedal"));
        wheelCollider.steerAngle = Steer(Input.GetAxisRaw("Steer"));
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
        float torqueApplied = TractionControl(throttle * transmission.CrankshaftTorque);
        return torqueApplied / vehicle.PoweredWheels;
    }

    private float TractionControl(float torqueAtWheel)
    {
        if (transmission.CurrentGear > 2)
        {
            return torqueAtWheel;
        }
        if ((wheelCollider.rotationSpeed) > 1080)
        {
            return torqueAtWheel * 0.6f;
        }
        if ((wheelCollider.rotationSpeed) > 720)
        {
            return torqueAtWheel * 0.7f;
        }
        if ((wheelCollider.rotationSpeed) > 360)
        {
            return torqueAtWheel * 0.8f;
        }
        return torqueAtWheel;
    }

    private float Brake(float brakePedal)
    {
        if (brakePedal < 0.1f)
        {
            return 0.0f;
        }
        float brakingForce = (vehicle.DownForce * 0.1f) + (vehicle.KerbWeight * brakeData.brakeEfficiency);
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
        float turnForce = wheelCurve.Evaluate(vehicle.CurrentSpeed);
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
