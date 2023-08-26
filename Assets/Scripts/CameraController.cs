using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject car;

    private Vector3 offset;

    private void Start()
    {
        offset = new(0.0f, 2.5f, -4.0f);
    }

    private void LateUpdate()
    {
        transform.position = car.transform.position + offset;
    }
}
