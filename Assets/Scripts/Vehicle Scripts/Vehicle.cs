using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private Wheel[] wheels;
    [SerializeField] private Engine engine;
    [SerializeField] private HUD headsUp;
    [SerializeField] private Transform centerOfMass;
    
    public float maxEnginePower;
    public float brakingForce;
    public float turnForce;

    private Rigidbody carRb;
    private float currentPower;
    private float currentSpeed;

    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass.position;
    }

    private void Update()
    {
        turnForce = Input.GetAxisRaw("Horizontal");
        currentPower = Input.GetAxisRaw("Vertical") * maxEnginePower;
        currentSpeed = carRb.velocity.magnitude;
        headsUp.UpdateGear(engine.CurrentGear);
    }

    private void FixedUpdate()
    {
        OperateWheels();
    }

    private void OperateWheels()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.OperateWheel(currentSpeed, engine.CalculateCurrentTorque(currentPower), 0.0f, turnForce);
        }
    }
}
