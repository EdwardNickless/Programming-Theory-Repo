using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private GameObject[] wheelList;
    [SerializeField] private Transform centerOfMass;
    
    public float horsePower;
    public float brakingForce;

    private Rigidbody rb;
    private WheelCollider[] wheels;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.position;

        for (int i = 0; i < wheelList.Length; i++)
        {
            wheels[i] = wheelList[i].GetComponent<WheelCollider>();
        }
    }

    private void Update()
    {
        Accelerate();
    }

    protected virtual void Accelerate()
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = horsePower;
        }
    }

    protected virtual void Decelerate()
    {
        foreach (WheelCollider wheel in wheels)
        {

        }
    }

    protected virtual void Brake()
    {
        foreach (WheelCollider wheel in wheels)
        {

        }
    }
}
