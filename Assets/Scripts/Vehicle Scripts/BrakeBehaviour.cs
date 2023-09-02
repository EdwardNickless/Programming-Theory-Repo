using UnityEngine;

public class BrakeBehaviour : MonoBehaviour
{
    [SerializeField] private BrakeData brakeData;

    private VehicleBehaviour vehicle;
    private WheelBehaviour wheel;
    private float brakingForce;
    private float fractionalDistribution;
    private bool hasABS;

    private void Awake()
    {
        vehicle = GetComponentInParent<VehicleBehaviour>();
        wheel = GetComponent<WheelBehaviour>();
    }

    private void Start()
    {
        brakingForce = brakeData.BrakeForce;
        fractionalDistribution = brakeData.FractionalDistribution;
        hasABS = brakeData.HasABS;
    }

    private void FixedUpdate()
    {
        wheel.SetBrakeTorque(ApplyABS());
    }

    private float ApplyABS()
    {
        if (!hasABS)
        {
            return CalculateBrakeTorque();
        }
        if (wheel.Collider.rotationSpeed == 0)
        {
            return 0.0f;
        }
        return CalculateBrakeTorque();
    }

    private float CalculateBrakeTorque()
    {
        float initialBrakeTorque = (vehicle.DownForce * 0.1f) + (vehicle.KerbWeight * brakingForce);
        return initialBrakeTorque * fractionalDistribution;
    }
}

