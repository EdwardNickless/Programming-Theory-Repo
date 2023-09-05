using UnityEngine;

public class OdometerBehaviour : MonoBehaviour
{
    [SerializeField] VehicleBehaviour vehicle;

    private const float MAX_SPEED_ANGLE = -180;
    private const float MIN_SPEED_ANGLE = 0;

    private Transform pivotPoint;
    private float currentSpeed;
    private float maxSpeed;


    private void Awake()
    {
        pivotPoint = transform.Find("PivotPoint").GetComponent<Transform>();
    }

    private void Start()
    {
        maxSpeed = 180;
    }

    private void Update()
    {
        currentSpeed = vehicle.CurrentSpeed;
    }

    private void FixedUpdate()
    {
        CalculateDialPosition();
    }

    private float CalculateDialPosition()
    {
        float totalAngleSize = MIN_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float rpmNormalized = currentSpeed / maxSpeed;

        return MIN_SPEED_ANGLE - rpmNormalized * totalAngleSize;
    }

    private void LateUpdate()
    {
        MoveDial();
    }

    private void MoveDial()
    {
        pivotPoint.eulerAngles = new Vector3(0, 0, CalculateDialPosition());
    }
}
