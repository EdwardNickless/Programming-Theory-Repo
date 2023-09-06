using UnityEngine;

public class HandbrakeBehaviour : MonoBehaviour
{
    [SerializeField] private BrakeData brakeData;

    private VehicleBehaviour vehicle;
    private WheelBehaviour wheel;
    private WheelFrictionCurve forwardFriction;
    private WheelFrictionCurve sidewaysFriction;
    private float brakingForce;

    private float defaultForwardStiffness;
    private float defaultSidewaysStiffness;
    [SerializeField] private float increasedStiffness;
    [SerializeField] private float decreasedStiffness;

    private void Awake()
    {
        vehicle = GetComponentInParent<VehicleBehaviour>();
        wheel = GetComponent<WheelBehaviour>();
    }

    private void Start()
    {
        brakingForce = brakeData.BrakeForce;
        forwardFriction = wheel.Collider.forwardFriction;
        sidewaysFriction = wheel.Collider.sidewaysFriction;
        defaultForwardStiffness = forwardFriction.stiffness;
        defaultSidewaysStiffness = sidewaysFriction.stiffness;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyHandbrake();
            return;
        }
        ResetSidewaysFriction();
    }

    private void ApplyHandbrake()
    {
        wheel.Collider.brakeTorque = vehicle.DownForce + (vehicle.KerbWeight * brakingForce);
        DecreaseSidewaysFriction();
    }

    private void DecreaseSidewaysFriction()
    {
        if (sidewaysFriction.stiffness == decreasedStiffness)
        {
            return;
        }
        sidewaysFriction.stiffness = decreasedStiffness;
        forwardFriction.stiffness = increasedStiffness;
        wheel.Collider.forwardFriction = forwardFriction;
        wheel.Collider.sidewaysFriction = sidewaysFriction;
    }

    private void ResetSidewaysFriction()
    {
        forwardFriction.stiffness = defaultForwardStiffness;
        sidewaysFriction.stiffness = defaultSidewaysStiffness;
        wheel.Collider.forwardFriction = forwardFriction;
        wheel.Collider.sidewaysFriction = sidewaysFriction;
    }
}
