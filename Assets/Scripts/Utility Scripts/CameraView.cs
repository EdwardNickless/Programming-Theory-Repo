using UnityEngine;

public class CameraView
{
    private Vector3 position;
    private Vector3 rotation;
    
    public CameraView (Vector3 position, Vector3 eulerRotation)
    {
        this.position = position;
        this.rotation = eulerRotation;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Vector3 GetEulerAngles()
    {
        return rotation;
    }
}