using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private AnimationCurve wheelCurve;
    [SerializeField] private Transform wheelMeshTransform;
    [SerializeField] private Brake brake;
    [SerializeField] private Transmission transmission;
    [SerializeField] private bool isPowered;
    [SerializeField] private bool isSteered;

    private WheelCollider wheelCollider;
    private float turnForce;
    
    public bool IsPowered { get { return isPowered; } private set { isPowered = value; } }

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
        turnForce = Input.GetAxisRaw("Steer");
    }

    public void OperateWheel(float torqueAtWheel, float currentSpeed)
    {
        if (torqueAtWheel > 0)
        {
            Accelerate(torqueAtWheel);
        }
        else
        {
            Decelerate(torqueAtWheel);
        }
        Brake(brake.StoppingForce);
        Steer(currentSpeed);
    }

    private void Accelerate(float torqueAtWheel)
    {
        if (!isPowered)
        {
            return;
        }
        float torqueApplied = TractionControl(torqueAtWheel);
        wheelCollider.motorTorque = torqueApplied;
    }

    public void Decelerate(float torqueAtWheel)
    {
        if (!isPowered)
        {
            return;
        }
        wheelCollider.motorTorque = -torqueAtWheel;
    }

    private void Brake(float stoppingForce)
    {
        wheelCollider.brakeTorque = stoppingForce;
    }

    private void Steer(float currentSpeed)
    {
        if (!isSteered)
        {
            return;
        }
        float steerAngle = turnForce * wheelCurve.Evaluate(currentSpeed);
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
}
