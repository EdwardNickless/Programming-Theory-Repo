using System;
using System.Collections;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] private EngineData engineData;
    [SerializeField] private Vehicle vehicle;
    [SerializeField] private Transmission transmission;

    private float minRPM;
    private float maxRPM;
    private float currentPistonStep;
    private float idleMinRPM;
    private float idleMaxRPM;
    private float redLineMinRPM;
    private float brakingDecreaseMultiplier = 7.5f;

    public float CurrentRPM { get; private set; }
    public float RedLineMinRPM { get { return redLineMinRPM; } private set { redLineMinRPM = value; } }

    public int GetMaxSpeed()
    {
        return engineData.MaxSpeed;
    }

    private void Start()
    {
        minRPM = engineData.MinRPM;
        maxRPM = engineData.MaxRPM;
        CurrentRPM = minRPM;
        idleMinRPM = engineData.MinRPM - engineData.IdleRange;
        idleMaxRPM = engineData.MinRPM + engineData.IdleRange;
        redLineMinRPM = maxRPM - engineData.RedLineRange;
    }

    public void ChangeRPMOnGearChange(int previousGear, int nextGear)
    {
        if (vehicle.CurrentSpeed == 0)
        {
            return;
        }
        if (!vehicle.IsGrounded)
        {
            return;
        }
        if (Input.GetAxisRaw("BrakePedal") < 0.1f)
        {
            return;
        }

        float currentRpmPercentage = (CurrentRPM / maxRPM);

        // Introduce a gear-dependent factor to reduce the impact in lower gears
        float gearFactor = 1.0f / (previousGear * 0.5f + 1.0f); // Adjust the coefficient (0.5f) as needed

        float torqueFactor = previousGear * engineData.TorqueCurve.Evaluate(CurrentRPM);
        float speedFactor = (vehicle.CurrentSpeed * 0.5f);

        // Adjust the factor for speedFactor in lower gears
        float speedFactorAdjustment = 1.0f / (previousGear * 0.25f + 1.0f); // Adjust the coefficient (0.25f) as needed

        float finalChangeNemerator = currentRpmPercentage * torqueFactor * speedFactor;
        float finalChangeDenominator = (speedFactor * 0.25f) * gearFactor * speedFactorAdjustment;
        float finalChange = finalChangeNemerator / finalChangeDenominator;

        if (nextGear == 0)
        {
            return;
        }
        if (previousGear > nextGear)
        {
            CurrentRPM += finalChange;
        }
        else
        {
            CurrentRPM -= finalChange;
        }
    }

    public float CalculateCurrentRPM(Wheel[] wheels)
    {
        if (!vehicle.IsGrounded) { return DisengagedRPM(); }

        if (transmission.CurrentGear == 0) { return DisengagedRPM(); }

        if (Input.GetAxisRaw("BrakePedal") > 0.0f) { return DisengagedRPM(); }

        CalculateRPMFromWheels(wheels);

        if (CurrentRPM <= idleMinRPM)
        {
            return IdleRPM();
        }
        return CurrentRPM;
    }

    private float DisengagedRPM()
    {
        float throttle = Input.GetAxisRaw("Throttle");

        if (throttle < 0.1f)
        {
            return DecreaseRPM();
        }
        else
        {
            return IncreaseRPM(throttle);
        }
    }

    private float IncreaseRPM(float throttle)
    {
        if (CurrentRPM >= maxRPM - engineData.RedLineRange)
        {
            return RedlineRPM();
        }
        CurrentRPM = Mathf.Max(IdleRPM(), CurrentRPM + (throttle * engineData.PowerCurve.Evaluate(CurrentRPM)));
        return CurrentRPM;
    }

    private float DecreaseRPM()
    {
        if (CurrentRPM <= minRPM + engineData.IdleRange)
        {
            return IdleRPM();
        }

        float targetRPM = CurrentRPM - (engineData.PowerCurve.Evaluate(CurrentRPM) / 75);
        CurrentRPM = Mathf.Min(RedlineRPM(), targetRPM);

        if (transmission.CurrentGear == 0)
        {
            return CurrentRPM;
        }
        if (!vehicle.IsGrounded)
        {
            float airborneDecrease = (engineData.PowerCurve.Evaluate(CurrentRPM) * transmission.CurrentGear);
            return CurrentRPM - airborneDecrease;
        }
        if (Input.GetAxisRaw("BrakePedal") > 0.0f)
        {
            float brakingDecrease = (engineData.TorqueCurve.Evaluate(CurrentRPM) * brakingDecreaseMultiplier);
            return Mathf.Max(IdleRPM(), CurrentRPM - brakingDecrease);
        }

        return CurrentRPM;
    }

    private void CalculateRPMFromWheels(Wheel[] wheels)
    {
        float totalWheelRPM = 0.0f;

        foreach (Wheel wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                totalWheelRPM += Mathf.Abs(wheel.RPM());
            }
        }
        float newRPM = (totalWheelRPM * transmission.GetMultiplier()) / vehicle.PoweredWheels;

        CurrentRPM = Mathf.Min(newRPM, RedlineRPM());
    }

    private float IdleRPM()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return CurrentRPM;
        }
        currentPistonStep += Time.deltaTime;

        float currentStep = Mathf.PingPong(currentPistonStep, 1.0f);

        if (currentStep >= 1.0f)
        {
            currentPistonStep = 0.0f;
        }

        return Mathf.Lerp(idleMinRPM, idleMaxRPM, currentStep);
    }

    private float RedlineRPM()
    {
        currentPistonStep += Time.deltaTime * 5.0f;

        float currentStep = Mathf.PingPong(currentPistonStep, 1.0f);

        if (currentStep >= 1.0f)
        {
            currentPistonStep = 0.0f;
        }

        return Mathf.Lerp(redLineMinRPM, maxRPM, currentStep);
    }

    public void RPMCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(LimitRPMDecrease));
    }

    private IEnumerator LimitRPMDecrease()
    {
        float originalDecreaseMultiplier = brakingDecreaseMultiplier;
        brakingDecreaseMultiplier *= 0.1f;
        yield return new WaitForSeconds(transmission.CurrentGear / 2);
        brakingDecreaseMultiplier = originalDecreaseMultiplier;
    }
}
