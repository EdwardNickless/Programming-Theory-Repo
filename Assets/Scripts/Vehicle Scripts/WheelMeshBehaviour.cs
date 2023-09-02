using UnityEngine;

public class WheelMeshBehaviour : MonoBehaviour
{
    [SerializeField] WheelBehaviour wheel;

    private void LateUpdate()
    {
        AnimateWheelMesh();
    }

    public void AnimateWheelMesh()
    {
        Vector3 position;
        Quaternion rotation;
        wheel.Collider.GetWorldPose(out position, out rotation);
        transform.position = position;
        transform.rotation = rotation;
    }
}
