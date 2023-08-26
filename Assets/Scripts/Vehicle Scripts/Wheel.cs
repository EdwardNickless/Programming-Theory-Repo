using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private AnimationCurve wheelCurve;
    [SerializeField] private Transform wheelMeshTransform;
    [SerializeField] private Brake brake;
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
        Accelerate(torqueAtWheel);
        Brake(brake.StoppingForce);
        Steer(currentSpeed);
    }

    private void Accelerate(float torqueAtWheel)
    {
        if (!isPowered)
        {
            return;
        }
        wheelCollider.motorTorque = torqueAtWheel;
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
}
