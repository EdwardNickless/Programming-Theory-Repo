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

    public float CalculateCurrentRPM(Wheel[] wheels)
    {
        if (!vehicle.IsGrounded)
        {
            return DisengagedRPM();
        }
        if (transmission.CurrentGear == 0)
        {
            return DisengagedRPM();
        }
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

        if (throttle > 0)
        {
            return IncreaseRPM(throttle);
        }
        else
        {
            return DecreaseRPM();
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
        CurrentRPM = Mathf.Min(RedlineRPM(), CurrentRPM - (engineData.PowerCurve.Evaluate(CurrentRPM)));
        return CurrentRPM;
    }

    private void CalculateRPMFromWheels(Wheel[] wheels)
    {
        float totalRPM = 0.0f;

        foreach (Wheel wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                totalRPM += Mathf.Abs(wheel.RPM());
            }
        }
        totalRPM = (totalRPM * transmission.GetMultiplier()) / vehicle.PoweredWheels;

        CurrentRPM = Mathf.Min(totalRPM, RedlineRPM());
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
}
