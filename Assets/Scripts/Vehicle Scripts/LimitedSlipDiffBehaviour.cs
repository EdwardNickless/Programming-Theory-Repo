using UnityEngine;

public class LimitedSlipDiffBehaviour : MonoBehaviour
{
    [SerializeField] private LimitedSlipDiffData limitedSlipDiffData;

    private VehicleBehaviour vehicle;
    private WheelBehaviour wheel;

    private WheelFrictionCurve forwardFriction;
    private WheelFrictionCurve sidewaysFriction;

    private bool hasTractionControl;

    private float defaultForwardStiffness;
    private float defaultSidewaysStiffness;
    private float increasedStiffness;

    private void Awake()
    {
        vehicle = GetComponentInParent<VehicleBehaviour>();
        wheel = GetComponent<WheelBehaviour>();
    }

    private void Start()
    {
        hasTractionControl = limitedSlipDiffData.HasTractionControl;
        forwardFriction = wheel.Collider.forwardFriction;
        sidewaysFriction = wheel.Collider.sidewaysFriction;
        defaultForwardStiffness = limitedSlipDiffData.DefaultForwardStiffness;
        defaultSidewaysStiffness = limitedSlipDiffData.DefaultSidewaysStiffness;
        increasedStiffness = limitedSlipDiffData.IncreasedStiffness;
    }

    private void FixedUpdate()
    {
        if (!hasTractionControl)
        {
            return;
        }
        ApplyTractionControl();
    }

    private void ApplyTractionControl()
    {
        if (vehicle.CurrentSpeed < 20) { return; }

        if (Input.GetAxisRaw("Steer") == 0)
        {
            ResetAsymptoteSlip();
            return;
        }

        float wheelCircumference = (wheel.Collider.radius * Mathf.PI * 2.0f);
        float linearWheelSpeed = (wheel.Collider.rpm * wheelCircumference) / 60.0f;
        float currentSlip = Mathf.Abs(vehicle.CurrentSpeed - linearWheelSpeed);

        if (currentSlip > wheel.Collider.forwardFriction.extremumSlip)
        {
            IncreaseAsymptoteSlip();
        }
    }

    private void IncreaseAsymptoteSlip()
    {
        if (forwardFriction.stiffness == increasedStiffness)
        {
            return;
        }
        forwardFriction.stiffness = increasedStiffness;
        sidewaysFriction.stiffness = increasedStiffness;
        wheel.Collider.forwardFriction = forwardFriction;
        wheel.Collider.sidewaysFriction = sidewaysFriction;
    }

    private void ResetAsymptoteSlip()
    {
        if (forwardFriction.stiffness == defaultForwardStiffness)
        {
            return;
        }
        forwardFriction.stiffness = defaultForwardStiffness;
        sidewaysFriction.stiffness = defaultSidewaysStiffness;
        wheel.Collider.forwardFriction = forwardFriction;
        wheel.Collider.sidewaysFriction = sidewaysFriction;
    }
}
