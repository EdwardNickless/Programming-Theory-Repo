using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchBehaviour : MonoBehaviour
{
    [SerializeField] private Transform roadView;
    [SerializeField] private Transform operatorView;
    [SerializeField] private Transform followView;
    private Transform roadTarget;
    private Transform operatorTarget;
    private Transform followTarget;

    private Camera viewPoint;
    private List<string> referenceNames;
    private Dictionary<string, Transform> cameraViews;
    private Dictionary<string, Transform> cameraTargets;

    private int cameraIndex;
    private string cameraName;
    private string targetName;

    public string CameraName { get { return cameraName; } private set { cameraName = value; } }
    public string TargetName { get { return targetName; } private set { targetName = value; } }

    public Transform CurrentView { get { return cameraViews[cameraName]; } }
    public Transform CurrentTarget { get { return cameraTargets[TargetName]; } }

    private void Awake()
    {
        viewPoint = GetComponent<Camera>();
        roadTarget = roadView.Find("RoadTarget");
        operatorTarget = operatorView.Find("OperatorTarget");
        followTarget = followView.Find("FollowTarget");
    }

    private void Start()
    {
        InitialiseCameraNames();
        InitialiseCameraViewsDictionary();
        InitialiseCameraTargetsDictionary();
        cameraIndex = 0;
        CameraName = referenceNames[cameraIndex];
        UpdateCurrentCamera(cameraIndex);
    }

    private void InitialiseCameraNames()
    {
        referenceNames = new List<string>
        {
            "Follow View",
            "Operator View",
            "Road View"
        };
    }

    private void InitialiseCameraViewsDictionary()
    {
        cameraViews = new Dictionary<string, Transform>
        {
            ["Follow View"] = followView,
            ["Operator View"] = operatorView,
            ["Road View"] = roadView
        };
    }

    private void InitialiseCameraTargetsDictionary()
    {
        cameraTargets = new Dictionary<string, Transform>
        {
            ["Follow View"] = followTarget,
            ["Operator View"] = operatorTarget,
            ["Road View"] = roadTarget
        };
    }

    private void UpdateCurrentCamera(int viewIndex)
    {
        CameraName = referenceNames[viewIndex];
        TargetName = referenceNames[viewIndex];
        viewPoint.transform.position = cameraViews[cameraName].position;
        viewPoint.transform.localEulerAngles = cameraViews[cameraName].eulerAngles;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeView();
        }
    }

    private void ChangeView()
    {
        cameraIndex++;
        if (cameraIndex >= referenceNames.Count)
        {
            cameraIndex = 0;
        }
        UpdateCurrentCamera(cameraIndex);
    }
}
