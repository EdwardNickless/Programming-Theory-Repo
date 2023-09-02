using UnityEngine;

public class TransmissionBehaviour : MonoBehaviour
{
    [SerializeField] private TransmissionData transmissionData;
    [SerializeField] private AnimationCurve torqueCurve;

    private EngineBehaviour engine;
    private GearRatiosDictionary gearRatios;
    private int currentGear;
    private int topGear;
    private float differentialRatio;
    private float reverseGearRatio;
    private float driveshaftTorque;

    public int CurrentGear { get { return currentGear; } private set { currentGear = value; } }
    public float DriveshaftTorque { get { return driveshaftTorque; } private set { driveshaftTorque = value; } }

    private void Awake()
    {
        engine = GetComponent<EngineBehaviour>();
    }

    public float GetMultiplier()
    {
        if (currentGear == 0)
        {
            return gearRatios.GetValue(currentGear) * differentialRatio * 1.5f;
        }
        if (currentGear > 0)
        {
            return gearRatios.GetValue(currentGear) * differentialRatio;
        }
        return reverseGearRatio * differentialRatio;
    }

    private void Start()
    {
        gearRatios = transmissionData.GearRatios;
        topGear = transmissionData.GearRatios.Count - 1;
        differentialRatio = transmissionData.DifferentialRatio;
        reverseGearRatio = transmissionData.ReverseGearRatio;
    }

    private void Update()
    {
        DriveshaftTorque = CalculateTorque();
        ChangeGearOnInput();
    }

    public float CalculateTorque()
    {
        if (engine.GetCurrentSpeed() >= engine.GetMaxSpeed())
        {
            return 0.0f;
        }
        if (engine.CurrentRPM >= engine.RedLineMinRPM)
        {
            return 0.0f;
        }
        float currentTorque = torqueCurve.Evaluate(engine.CurrentRPM) * GetMultiplier();
        if (currentGear == -1)
        {
            currentTorque *= -1;
        }
        return currentTorque;
    }

    private void ChangeGearOnInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShiftUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShiftDown();
        }
    }

    private void ShiftUp()
    {
        if (currentGear == topGear)
        {
            return;
        }
        CurrentGear++;
    }

    private void ShiftDown()
    {
        if (CurrentGear == -1)
        {
            return;
        }
        CurrentGear--;
    }
}
