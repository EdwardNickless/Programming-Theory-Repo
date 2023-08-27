using UnityEngine;

public class TransmissionController : MonoBehaviour
{
    [SerializeField] private TransmissionData transmissionData;

    private GearRatiosDictionary gearRatios;
    private int currentGear;
    private int topGear;
    private float differentialRatio;
    private float reverseGearRatio;
    private float outputTorque;

    public int CurrentGear { get { return currentGear; } private set { currentGear = value; } }
    public float OutputTorque { get { return outputTorque; } private set { outputTorque = value; } }

    public float GetMultiplier()
    {
        if (currentGear == 0)
        {
            return 1.0f;
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

    public float CalculateOutputTorque(float crankshaftTorque)
    {
        if (currentGear > 0)
        {
            return crankshaftTorque * GetMultiplier();
        }
        if (currentGear < 0)
        {
            return -(crankshaftTorque * GetMultiplier());
        }
        else { return 0.0f; }
    }

    private void Update()
    {
        ChangeGear();
    }

    private void ChangeGear()
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
        if (currentGear == -1)
        {
            currentGear = 0;
            return;
        }
        if (currentGear < topGear)
        {
            currentGear++;
            return;
        }
    }

    private void ShiftDown()
    {
        if (currentGear > 1)
        {
            currentGear--;
            return;
        }
        if (currentGear == 1)
        {
            currentGear = 0;
            return;
        }
        if (currentGear == 0)
        {
            currentGear = -1;
            return;
        }
    }
}
