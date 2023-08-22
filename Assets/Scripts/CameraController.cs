using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject car;

    private Vector3 offset;

    private void Start()
    {
        offset = new(5.0f, 2.5f, 0.0f);
    }

    private void LateUpdate()
    {
        transform.position = car.transform.position + offset;
    }
}
