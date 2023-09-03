using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchBehaviour : MonoBehaviour
{
    private Camera mainCamera;

    private int viewIndex;
    private string viewName;

    private Vector3 behindCarPosition;
    private Vector3 behindCarRotation;

    private Vector3 operatorViewPosition;
    private Vector3 operatorViewRotation;

    private Vector3 frontBumperPosition;
    private Vector3 frontBumperRotation;

    private List<string> cameraViewNames;
    private Dictionary<string, CameraView> cameraViews;

    public string ViewName { get { return viewName; } private set { viewName = value; } }

    public CameraView CurrentView { get { return cameraViews[ViewName]; } }

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        InitialiseViews();
        InitialiseCameraViewNamesList();
        InitialiseCameraViewsDictionary();
        viewIndex = 0;
        viewName = cameraViewNames[viewIndex];
        SetCameraPositionAndRotation(viewIndex);
    }

    private void InitialiseViews()
    {
        behindCarPosition = new Vector3(0.0f, 2.0f, -4.0f);
        behindCarRotation = new Vector3(15.0f, 0.0f, 0.0f);

        operatorViewPosition = new Vector3(-0.15f, 1.25f, 1.0f);
        operatorViewRotation = new Vector3(10.0f, 0.0f, 0.0f);

        frontBumperPosition = new Vector3(0.0f, 0.5f, 2.0f);
        frontBumperRotation = new Vector3(5.0f, 0.0f, 0.0f);
    }

    private void InitialiseCameraViewNamesList()
    {
        cameraViewNames = new List<string>
        {
            "Follow View",
            "Operator View",
            "Bumper View"
        };
    }

    private void InitialiseCameraViewsDictionary()
    {
        cameraViews = new Dictionary<string, CameraView>
        {
            ["Follow View"] = new CameraView(behindCarPosition, behindCarRotation),
            ["Operator View"] = new CameraView(operatorViewPosition, operatorViewRotation),
            ["Bumper View"] = new CameraView(frontBumperPosition, frontBumperRotation)
        };
    }

    private void SetCameraPositionAndRotation(int viewIndex)
    {
        viewName = cameraViewNames[viewIndex];
        mainCamera.transform.localPosition = cameraViews[viewName].GetPosition();
        mainCamera.transform.localEulerAngles = cameraViews[viewName].GetEulerAngles();
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
        viewIndex++;
        if (viewIndex >= cameraViewNames.Count)
        {
            viewIndex = 0;
        }
        SetCameraPositionAndRotation(viewIndex);
    }
}
