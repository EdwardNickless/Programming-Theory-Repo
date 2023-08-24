using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private AnimationCurve wheelCurve;
    [SerializeField] private Transform wheelMeshTransform;
    [SerializeField] private bool isPowered;
    [SerializeField] private bool isSteered;

    private WheelCollider wheelCollider;
    
    public bool IsPowered { get { return isPowered; } private set { isPowered = value; } }

    public float RPM()
    {
        return wheelCollider.rpm;
    }

    private void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }

    public void OperateWheel(float torqueAtWheel, float stoppingForce, float currentSpeed, float turnMultiplier)
    {
        Accelerate(torqueAtWheel);
        Brake(stoppingForce);
        Steer(currentSpeed, turnMultiplier);
    }

    private void Accelerate(float torqueAtWheel)
    {
        if (!isPowered)
        {
            return;
        }
        wheelCollider.motorTorque = torqueAtWheel;
    }

    private void Brake(float stoppingForce)
    {
        
    }

    private void Steer(float currentSpeed, float turnMultiplier)
    {
        if (!isSteered)
        {
            return;
        }
        float steerAngle = turnMultiplier * wheelCurve.Evaluate(currentSpeed);
        wheelCollider.steerAngle = turnMultiplier * wheelCurve.Evaluate(currentSpeed);
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
