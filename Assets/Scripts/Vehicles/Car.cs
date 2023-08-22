using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Wheel[] wheels;
    
    public float maxEnginePower;
    public float brakingForce;
    public float turnForce;
    private float currentPower;
    public float maxTurnAngle = 30.0f;

    private void Update()
    {
        turnForce = Input.GetAxisRaw("Horizontal") * maxTurnAngle;
        currentPower = Input.GetAxisRaw("Vertical") * maxEnginePower;
    }

    private void FixedUpdate()
    {
        OperateMovement();
    }

    private void OperateMovement()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.OperateWheel(currentPower, 0.0f, turnForce);
        }
    }
}
