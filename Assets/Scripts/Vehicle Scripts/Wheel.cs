using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private AnimationCurve wheelCurve;
    [SerializeField] private Brake brake;
    [SerializeField] private Transmission transmission;
    [SerializeField] private Transform wheelMeshTransform;
    [SerializeField] private Vehicle vehicle;
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
        Brake(brake.StoppingForce);
        Steer(Input.GetAxisRaw("Steer"));
    }

    private float Accelerate(float throttle)
    {
        if (!isPowered)
        {
            return 0.0f;
        }
        if (vehicle.CurrentSpeed >= vehicle.MaxSpeed - 0.1f)
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

    private void Brake(float stoppingForce)
    {
        wheelCollider.brakeTorque = stoppingForce;
    }

    private void Steer(float turnForce)
    {
        if (!isSteered)
        {
            return;
        }
        float steerAngle = turnForce * wheelCurve.Evaluate(vehicle.CurrentSpeed);
        wheelCollider.steerAngle = steerAngle;
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
