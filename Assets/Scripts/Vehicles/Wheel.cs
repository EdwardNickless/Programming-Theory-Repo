using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Transform wheelMeshTransform;
    [SerializeField] private bool isPowered;
    [SerializeField] private bool isSteered;
    
    private WheelCollider wheelCollider;
    private Vector3 meshPositionOffset;

    private void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
        meshPositionOffset = wheelMeshTransform.position - wheelCollider.transform.position;
    }

    public void OperateWheel(float torqueAtWheel, float stoppingForce, float turnAngle)
    {
        Accelerate(torqueAtWheel);
        Brake(stoppingForce);
        Steer(turnAngle);
    }

    private void Accelerate(float torqueAtWheel)
    {
        if (!isPowered)
        {
            return;
        }
        wheelCollider.motorTorque = torqueAtWheel;
    }

    private void Brake(float stoppingForce)
    {
        
    }

    private void Steer(float turnAngle)
    {
        if (!isSteered)
        {
            return;
        }
        wheelCollider.steerAngle = turnAngle;
    }

    private void LateUpdate()
    {
        AnimateWheelMesh();
    }

    public void AnimateWheelMesh()
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelMeshTransform.position = position;
        wheelMeshTransform.rotation = rotation;
    }
}
