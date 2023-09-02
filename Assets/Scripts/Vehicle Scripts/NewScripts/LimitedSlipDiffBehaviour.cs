using UnityEngine;

public class LimitedSlipDiffBehaviour : MonoBehaviour
{
    [SerializeField] private bool hasTractionControl;
    [SerializeField] private float asymptoteIncreased;

    private VehicleBehaviour vehicle;
    private WheelBehaviour[] wheels;
    private WheelFrictionCurve forwardFriction;
    private WheelFrictionCurve sidewaysFriction;
    private float defaultForwardAsymptoteSlip;
    private float defaultSidewaysAsymptoteSlip;

    private void Awake()
    {
        vehicle = GetComponentInParent<VehicleBehaviour>();
        wheels = GetComponentsInChildren<WheelBehaviour>();
    }

    private void Start()
    {
        defaultForwardAsymptoteSlip = CalculateForwardAsymptoteSlip();
        defaultSidewaysAsymptoteSlip = CalculateSidewaysAsymptoteSlip();
    }

    private float CalculateForwardAsymptoteSlip()
    {
        float summedSlip = 0.0f;
        foreach (WheelBehaviour wheel in wheels)
        {
            summedSlip += wheel.Collider.forwardFriction.asymptoteSlip;
        }
        return summedSlip / wheels.Length;
    }

    private float CalculateSidewaysAsymptoteSlip()
    {
        float summedSlip = 0.0f;
        foreach (WheelBehaviour wheel in wheels)
        {
            summedSlip += wheel.Collider.sidewaysFriction.asymptoteSlip;
        }
        return summedSlip / wheels.Length;
    }

    private void FixedUpdate()
    {
        foreach (WheelBehaviour wheel in wheels)
        {
            ApplyTractionControl(wheel);
        }
    }

    private void ApplyTractionControl(WheelBehaviour wheel)
    {
        if (!hasTractionControl) { return; }

        if (vehicle.CurrentSpeed == 0) { return; }

        if (Input.GetAxisRaw("Steer") == 0) { return; }

        forwardFriction = wheel.Collider.forwardFriction;
        sidewaysFriction = wheel.Collider.sidewaysFriction;

        float wheelCircumference = (wheel.Collider.radius * Mathf.PI * 2.0f);
        float linearWheelSpeed = (wheel.Collider.rpm * wheelCircumference) / 60.0f;
        float currentSlip = Mathf.Abs(vehicle.CurrentSpeed - linearWheelSpeed);

        if (currentSlip > wheel.Collider.forwardFriction.extremumSlip)
        {
            forwardFriction.asymptoteSlip = asymptoteIncreased;
            sidewaysFriction.asymptoteSlip = asymptoteIncreased;
            wheel.Collider.forwardFriction = forwardFriction;
            wheel.Collider.sidewaysFriction = sidewaysFriction;
        }
        else
        {
            forwardFriction.asymptoteSlip = defaultForwardAsymptoteSlip;
            sidewaysFriction.asymptoteSlip = defaultSidewaysAsymptoteSlip;
            wheel.Collider.forwardFriction = forwardFriction;
            wheel.Collider.sidewaysFriction = sidewaysFriction;
        }
    }
}
