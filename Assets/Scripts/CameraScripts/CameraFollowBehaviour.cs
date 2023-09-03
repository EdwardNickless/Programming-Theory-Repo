using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{
    [SerializeField] private AnimationCurve distanceCurve;

    private VehicleBehaviour vehicle;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 maxDistancePosition;

    private void Awake()
    {
        vehicle = GetComponentInParent<VehicleBehaviour>();
    }

    private void Start()
    {
        initialPosition = GetInitialPosition();
        maxDistancePosition = initialPosition + new Vector3(0.0f, 0.0f, -2.0f);
    }

    private Vector3 GetInitialPosition()
    {
        return transform.localPosition;
    }

    private void Update()
    {
        targetPosition = CalculateNextPosition();
    }

    private Vector3 CalculateNextPosition()
    {
        return Vector3.Lerp(initialPosition, maxDistancePosition, distanceCurve.Evaluate(vehicle.CurrentSpeed));
    }

    private void FixedUpdate()
    {
        transform.localPosition = targetPosition;
        transform.LookAt(vehicle.transform);
    }
}
