using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    [SerializeField] private AnimationCurve steeringCurve;

    private VehicleBehaviour vehicle;
    private WheelBehaviour[] wheels;
    

    private void Awake()
    {
        vehicle = GetComponentInParent<VehicleBehaviour>();
        wheels = GetComponentsInChildren<WheelBehaviour>();
    }

    private void FixedUpdate()
    {
        float turnForce = steeringCurve.Evaluate(vehicle.CurrentSpeed);

        foreach (WheelBehaviour wheel in wheels)
        {
            wheel.SetSteeringAngle(turnForce);
        }
    }
}
