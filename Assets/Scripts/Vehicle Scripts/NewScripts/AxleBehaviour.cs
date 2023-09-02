using UnityEngine;

public class AxleBehaviour : MonoBehaviour
{
    [SerializeField] private bool isDriveAxle;
    [SerializeField] private bool hasLaunchControl;

    private WheelBehaviour[] wheels;
    private TransmissionBehaviour transmission;
    private float driveShaftTorque;

    public bool IsDriveAxle {  get { return isDriveAxle; } }

    private void Awake()
    {
        wheels = GetComponentsInChildren<WheelBehaviour>();
        transmission = GetComponentInParent<TransmissionBehaviour>();
    }

    private void FixedUpdate()
    {
        if (!isDriveAxle) { return; }

        driveShaftTorque = transmission.DriveshaftTorque;
        CalculateWheelIndependantTorque(driveShaftTorque);
    }

    private void CalculateWheelIndependantTorque(float initialTorque)
    {
        foreach (WheelBehaviour wheel in wheels)
        {
            float torqueAtWheel = CalculateTorqueAtWheel(wheel, initialTorque);
            wheel.SetTorqueAtWheel(torqueAtWheel / wheels.Length);
        }
    }

    private float CalculateTorqueAtWheel(WheelBehaviour wheel, float initialTorque)
    {
        float appliedTorque = initialTorque;
        if (transmission.CurrentGear == 1)
        {
            appliedTorque *= ApplyLaunchControl(wheel);
        }
        return appliedTorque;
    }

    private float ApplyLaunchControl(WheelBehaviour wheel)
    {
        if (!hasLaunchControl)
        {
            return 1.0f;
        }
        if (wheel.Collider.rotationSpeed > 1080)
        {
            return 0.7f;
        }
        if ((wheel.Collider.rotationSpeed) > 720)
        {
            return 0.8f;
        }
        if ((wheel.Collider.rotationSpeed) > 360)
        {
            return 0.9f;
        }
        return 1.0f;
    }
}
