using UnityEngine;

public class TachometerBehaviour : MonoBehaviour
{
    [SerializeField] VehicleBehaviour vehicle;

    private const float MAX_RPM_ANGLE = -215;
    private const float Min_RPM_ANGLE = 0;

    private Transform pivotPoint;
    private float currentRPM;
    private float maxRPM;


    private void Awake()
    {
        pivotPoint = transform.Find("PivotPoint").GetComponent<Transform>();
    }

    private void Start()
    {
        maxRPM = vehicle.MaxRPM;
    }

    private void Update()
    {
        currentRPM = vehicle.CurrentRPM;
    }

    private void FixedUpdate()
    {
        CalculateDialPosition();
    }

    private float CalculateDialPosition()
    {
        float totalAngleSize = Min_RPM_ANGLE - MAX_RPM_ANGLE;
        float rpmNormalized = currentRPM / maxRPM;

        return Min_RPM_ANGLE - rpmNormalized * totalAngleSize;
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