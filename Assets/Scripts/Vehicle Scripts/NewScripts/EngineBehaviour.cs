using UnityEngine;

public class EngineBehaviour : MonoBehaviour
{
    [SerializeField] private EngineData engineData;

    private VehicleBehaviour vehicle;
    private TransmissionBehaviour transmission;
    private float currentPistonStep;
    private float idleMinRPM;
    private float idleMaxRPM;
    private float redLineMinRPM;
    private float redLineMaxRPM;

    public float CurrentRPM { get; private set; }
    public float RedLineMinRPM { get { return redLineMinRPM; } private set { redLineMinRPM = value; } }

    public int GetMaxSpeed()
    {
        return engineData.MaxSpeed;
    }

    public int GetCurrentSpeed()
    {
        return vehicle.CurrentSpeed;
    }

    private void Awake()
    {
        vehicle = GetComponentInParent<VehicleBehaviour>();
        transmission = GetComponent<TransmissionBehaviour>();
    }

    private void Start()
    {
        idleMinRPM = engineData.MinRPM - engineData.IdleRange;
        idleMaxRPM = engineData.MinRPM + engineData.IdleRange;
        redLineMinRPM = engineData.MaxRPM - engineData.RedLineRange;
        redLineMaxRPM = engineData.MaxRPM + engineData.RedLineRange;
        CurrentRPM = idleMinRPM;
    }

    public float CalculateCurrentRPM(WheelBehaviour[] wheels)
    {
        if (!vehicle.IsGrounded) { return ArtificialRPM(); }

        if (transmission.CurrentGear == 0) { return ArtificialRPM(); }

        CalculateRPMFromWheels(wheels);

        if (CurrentRPM <= idleMinRPM)
        {
            return IdleRPM();
        }
        return CurrentRPM;
    }

    private float ArtificialRPM()
    {
        float throttle = Input.GetAxisRaw("Throttle");

        if (throttle < 0.1f)
        {
            return DecreaseArtificialRPM();
        }
        else
        {
            return IncreaseArtificalRPM(throttle);
        }
    }

    private float IncreaseArtificalRPM(float throttle)
    {
        if (CurrentRPM >= redLineMinRPM)
        {
            return RedlineRPM();
        }
        CurrentRPM = Mathf.Max(IdleRPM(), CurrentRPM + (throttle * engineData.PowerCurve.Evaluate(CurrentRPM)));
        return CurrentRPM;
    }

    private float DecreaseArtificialRPM()
    {
        if (CurrentRPM <= idleMaxRPM)
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

        return CurrentRPM;
    }

    private void CalculateRPMFromWheels(WheelBehaviour[] wheels)
    {
        float totalWheelRPM = 0.0f;

        foreach (WheelBehaviour wheel in wheels)
        {
            if (wheel.IsPowered)
            {
                totalWheelRPM += wheel.RPM;
            }
        }

        float newRPM = (totalWheelRPM * transmission.GetMultiplier()) / vehicle.PoweredWheels;

        CurrentRPM = Mathf.Min(newRPM, RedlineRPM());

        if (CurrentRPM <= idleMaxRPM)
        {
            CurrentRPM = IdleRPM();
        }
    }

    private float IdleRPM()
    {
        currentPistonStep += Time.deltaTime;

        float currentStep = Mathf.PingPong(currentPistonStep, 1.0f);

        if (currentStep >= 1.0f)
        {
            currentPistonStep = 0.0f;
        }
        float lerpedRPM = Mathf.Lerp(idleMinRPM, idleMaxRPM, currentStep);
        return Mathf.Max(idleMinRPM, lerpedRPM);
    }

    private float RedlineRPM()
    {
        currentPistonStep += Time.deltaTime * 5.0f;

        float currentStep = Mathf.PingPong(currentPistonStep, 1.0f);

        if (currentStep >= 1.0f)
        {
            currentPistonStep = 0.0f;
        }

        return Mathf.Lerp(redLineMinRPM, redLineMaxRPM, currentStep);
    }
}
