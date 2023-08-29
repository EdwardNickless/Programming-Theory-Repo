using UnityEngine;

public class Transmission : MonoBehaviour
{
    [SerializeField] private TransmissionData transmissionData;
    [SerializeField] private Engine engine;
    [SerializeField] private AnimationCurve torqueCurve;

    private GearRatiosDictionary gearRatios;
    private int currentGear;
    private int topGear;
    private float differentialRatio;
    private float reverseGearRatio;
    private float crankshaftTorque;

    public int CurrentGear { get { return currentGear; } private set { currentGear = value; } }
    public float CrankshaftTorque { get { return crankshaftTorque; } private set { crankshaftTorque = value; } }

    public float GetMultiplier()
    {
        if (currentGear == 0)
        {
            return gearRatios.GetValue(currentGear) * differentialRatio * 2.0f;
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
        CrankshaftTorque = CalculateTorque();
        ChangeGearOnInput();
    }

    public float CalculateTorque()
    {
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
        int previousGear = CurrentGear;
        CurrentGear++;
        int nextGear = CurrentGear;
        engine.RPMCoroutine();
        engine.ChangeRPMOnGearChange(previousGear, nextGear);
    }

    private void ShiftDown()
    {
        if (CurrentGear == -1)
        {
            return;
        }
        int previousGear = CurrentGear;
        CurrentGear--;
        int nextGear = CurrentGear;
        engine.RPMCoroutine();
        engine.ChangeRPMOnGearChange(previousGear, nextGear);
    }
}
