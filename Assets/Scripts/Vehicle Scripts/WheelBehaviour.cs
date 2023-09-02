using UnityEngine;

public class WheelBehaviour : MonoBehaviour
{
    private AxleBehaviour axle;
    private WheelCollider wheelCollider;
    private float accelerationTorque;
    private float turnAngle;
    private float brakeTorque;

    public float RPM { get { return wheelCollider.rpm; } }

    public bool IsPowered { get { return axle.IsDriveAxle; } }

    public WheelCollider Collider { get { return wheelCollider; } }

    private void Awake()
    {
        axle = GetComponentInParent<AxleBehaviour>();
        wheelCollider = GetComponent<WheelCollider>();
    }

    public void SetAccelerationTorque(float torque)
    {
        accelerationTorque = torque;
    }

    public void SetSteeringAngle(float steeringForce)
    {
        turnAngle = steeringForce;
    }

    public void SetBrakeTorque(float torque)
    {
        brakeTorque = torque;
    }

    private void FixedUpdate()
    {
        wheelCollider.motorTorque = accelerationTorque * Input.GetAxisRaw("Throttle");
        wheelCollider.steerAngle = turnAngle * Input.GetAxisRaw("Steer");
        wheelCollider.brakeTorque = brakeTorque * Input.GetAxisRaw("BrakePedal");
    }
}
