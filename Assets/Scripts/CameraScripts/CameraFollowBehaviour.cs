using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{
    [SerializeField] private AnimationCurve distanceCurve;

    private CameraSwitchBehaviour switchBehaviour;
    private VehicleBehaviour vehicle;

    private Vector3 viewPosition;
    private Vector3 viewTarget;
    private Vector3 maxDistancePosition;

    private string currentView;

    private void Awake()
    {
        switchBehaviour = GetComponent<CameraSwitchBehaviour>();
        vehicle = FindObjectOfType<VehicleBehaviour>();
    }
    private void Update()
    {
        UpdateCameraOnViewChange();
    }

    private void LateUpdate()
    {
        FollowVehicle();
    }

    private void FollowVehicle()
    {
        transform.position = viewPosition;
        transform.LookAt(viewTarget);
    }

    private void UpdateCameraOnViewChange()
    {
        currentView = switchBehaviour.CameraName;
        viewPosition = switchBehaviour.CurrentView.position;
        viewTarget = switchBehaviour.CurrentTarget.position;
    }

    private Vector3 IncreaseFollowDistance()
    {
        return Vector3.Lerp(viewPosition, maxDistancePosition, distanceCurve.Evaluate(vehicle.CurrentSpeed));
    }
}
