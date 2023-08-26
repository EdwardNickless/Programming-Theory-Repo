using UnityEngine;

public class Brake : MonoBehaviour
{
    [SerializeField] private BrakeData brakeData;

    private float brakePedal;
    private float stoppingForce;
    float brakeEfficiency;
    float vehicleWeight;

    public float StoppingForce { get { return stoppingForce; } private set { stoppingForce = value; } }

    private void Start()
    {
        brakeEfficiency = brakeData.brakeEfficiency;
        vehicleWeight = GetComponentInParent<Rigidbody>().mass;
        foreach (WheelCollider coll in GetComponentsInParent<WheelCollider>())
        {
            vehicleWeight += coll.mass;
        }
    }

    private void Update()
    {
        brakePedal = Input.GetAxisRaw("Footbrake");
        StoppingForce = CalculateStoppingForce();
    }

    private float CalculateStoppingForce()
    {
        return brakePedal * vehicleWeight * brakeEfficiency;
    }
}
