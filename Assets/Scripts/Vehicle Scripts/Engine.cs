using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] private EngineScriptableObject engine;
    
    private float idleRPM;
    private float maxRPM;
    private float idleTorque;
    private float peakTorque;
    private float idleHorsePower;
    private float peakHorsePower;
    private GearRatiosDictionary gearRatios;

    private int currentGear;
    private int finalGear;

    private float finalDriveRatio;

    public int CurrentGear { get { return currentGear; } private set { currentGear = value; } }

    private void Start()
    {
        InitializeEngineSpecs();
        CurrentGear = 1;
        finalGear = 6;
    }

    private void InitializeEngineSpecs()
    {
        idleRPM = engine.IdleRPM;
        maxRPM = engine.MaxRPM;
        idleTorque = engine.IdleTorque;
        peakTorque = engine.PeakTorque;
        idleHorsePower = engine.IdleHorsePower;
        peakHorsePower = engine.PeakHorsePower;
        gearRatios = engine.GearRatios;
        finalDriveRatio = gearRatios.GetValue(0);
    }

    private void Update()
    {
        ChangeGear();
    }

    private void ChangeGear()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentGear < finalGear)
        {
            currentGear++;
            Debug.Log(currentGear);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && currentGear > 1)
        {
            currentGear--;
            Debug.Log(currentGear);
        }
    }

    public float CalculateCurrentTorque(float enginePower)
    {
        return enginePower * gearRatios.GetValue(currentGear) * finalDriveRatio;
    }
}
