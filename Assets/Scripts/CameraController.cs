using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;
    [SerializeField] private AnimationCurve speedCurve;

    private Transform vehicleTransform;
    private Vector3 targetOffset;
    private float xOffset;
    private float yOffset;
    private float zOffset;
    public float lerpSpeed;

    private void Start()
    {
        vehicleTransform = vehicle.transform;
        InitialiseOffset();
    }

    private void InitialiseOffset()
    {
        xOffset = transform.position.x;
        yOffset = transform.position.y;
        zOffset = transform.position.z;
        targetOffset = new(xOffset, yOffset, zOffset);
    }


    private void Update()
    {
        targetOffset = CalculateDesiredOffset();
    }

    private void LateUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, vehicleTransform.position + targetOffset, lerpSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    private Vector3 CalculateDesiredOffset()
    {
        xOffset = CalculateX();
        yOffset = CalculateY();
        zOffset = CalculateZ();
        return new(xOffset, yOffset, zOffset);
    }

    private float CalculateX()
    {
        return xOffset;
    }

    private float CalculateY()
    {
        return yOffset;
    }

    private float CalculateZ()
    {
        return speedCurve.Evaluate(vehicle.CurrentSpeed);
    }
}
