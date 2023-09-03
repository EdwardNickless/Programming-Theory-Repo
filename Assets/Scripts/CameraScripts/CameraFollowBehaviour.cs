using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{
    [SerializeField] private AnimationCurve distanceCurve;

    private CameraSwitchBehaviour switchBehaviour;
    private VehicleBehaviour vehicle;

    private Vector3 viewPosition;
    private Vector3 targetPosition;
    private Vector3 maxDistancePosition;

    private string currentView;

    private void Awake()
    {
        switchBehaviour = GetComponent<CameraSwitchBehaviour>();
        vehicle = FindObjectOfType<VehicleBehaviour>();
    }

    private void Start()
    {
        currentView = switchBehaviour.ViewName;
        viewPosition = switchBehaviour.CurrentView.GetPosition();
        maxDistancePosition = viewPosition + new Vector3(0.0f, 0.0f, -2.0f);
    }

    private void LateUpdate()
    {
        FollowVehicle();
    }

    private void FollowVehicle()
    {
        targetPosition = CalculateNextPosition();
        if (currentView == "Follow View")
        {
            targetPosition = IncreaseFollowDistance();
        }
        transform.position = targetPosition;
    }

    private Vector3 CalculateNextPosition()
    {
        UpdateCameraOnViewChange();
        return vehicle.transform.position + viewPosition;
    }

    private void UpdateCameraOnViewChange()
    {
        currentView = switchBehaviour.ViewName;
        viewPosition = switchBehaviour.CurrentView.GetPosition();
        transform.eulerAngles = switchBehaviour.CurrentView.GetEulerAngles();
    }

    private Vector3 IncreaseFollowDistance()
    {
        return Vector3.Lerp(viewPosition, maxDistancePosition, distanceCurve.Evaluate(vehicle.CurrentSpeed));
    }
}
