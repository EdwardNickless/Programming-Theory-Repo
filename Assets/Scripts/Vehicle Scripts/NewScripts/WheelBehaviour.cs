using UnityEngine;

public class WheelBehaviour : MonoBehaviour
{
    private AxleBehaviour axle;
    private WheelCollider wheelCollider;
    private float torqueAtWheel;
    private float turnAngle;

    public float RPM { get { return wheelCollider.rpm; } }

    public bool IsPowered { get { return axle.IsDriveAxle; } }

    public WheelCollider Collider { get { return wheelCollider; } }

    private void Awake()
    {
        axle = GetComponentInParent<AxleBehaviour>();
        wheelCollider = GetComponent<WheelCollider>();
    }

    public void SetTorqueAtWheel(float torque)
    {
        torqueAtWheel = torque;
    }

    public void SetSteeringAngle(float steeringForce)
    {
        turnAngle = steeringForce;
    }

    private void FixedUpdate()
    {
        wheelCollider.motorTorque = torqueAtWheel * Input.GetAxisRaw("Throttle");
        wheelCollider.steerAngle = turnAngle * Input.GetAxisRaw("Steer");
        //wheelCollider.brakeTorque = Brake(Input.GetAxisRaw("BrakePedal"));
    }
}
