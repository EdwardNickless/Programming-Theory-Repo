using System;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private Engine engine;
    [SerializeField] private Transmission transmission;
    [SerializeField] private Wheel[] wheels;

    private Rigidbody carRb;

    private float currentSpeed;
    private float currentRPM;
    private int currentGear;

    public Rigidbody Rigidbody { get { return carRb; } }
    public float CurrentSpeed { get { return currentSpeed; } private set { currentSpeed = value; } }
    public float CurrentRPM { get { return currentRPM; } private set { currentRPM = value; } }
    public int CurrentGear {  get { return currentGear; } private set { currentGear = value; } }
    public int MaxSpeed { get; private set; }

    public float PoweredWheels { get; private set; }

    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass.position;
        PoweredWheels = CountPoweredWheels();
        MaxSpeed = engine.GetMaxSpeed();
    }

    private float CountPoweredWheels()
    {
        int count = 0;
        foreach (var wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                count++;
            }
        }
        return count;
    }

    private void Update()
    {
        currentSpeed = Mathf.RoundToInt((carRb.velocity.magnitude * 3600f) / 1609.34f);
        currentRPM = engine.CalculateCurrentRPM(wheels);
        currentGear = transmission.CurrentGear;
    }
}
