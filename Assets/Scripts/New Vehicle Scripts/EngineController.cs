using UnityEngine;

public class EngineController : MonoBehaviour
{
    [SerializeField] private EngineData engineData;
    [SerializeField] private HUD headsUpDisplay;
    [SerializeField] AnimationCurve enginePower;

    private float minRPM;
    private float currentRPM;
    private float maxRPM;
    private float accelerationInput;
    private float currentPistonStep;

    public float CurrentRPM { get { return currentRPM; } set {  currentRPM = value; } }

    public int GetPistonCount()
    {
        return engineData.PistonCount;
    }

    private void Start()
    {
        minRPM = engineData.MinRPM;
        maxRPM = engineData.MaxRPM;
    }

    private void Update()
    {
        accelerationInput = Input.GetAxisRaw("Throttle");
        if (accelerationInput < 0.01f)
        {
            CurrentRPM = DecreaseRPM();
        }
        else
        {
            CurrentRPM = IncreaseRPM();
        }
        headsUpDisplay.UpdateHUD(0, 0, currentRPM);
    }

    private float IncreaseRPM()
    {
        if (CurrentRPM >= maxRPM - engineData.RedLineRange)
        {
            return RedlineRPM();
        }
        return CurrentRPM + (accelerationInput * enginePower.Evaluate(currentRPM));
    }

    private float DecreaseRPM()
    {
        if (CurrentRPM <= minRPM + engineData.IdleRange)
        {
            return IdleRPM();
        }
        return CurrentRPM - (enginePower.Evaluate(currentRPM));
    }

    private float IdleRPM()
    {
        currentPistonStep += Time.deltaTime;

        float idleMinRPM = minRPM - engineData.IdleRange;
        float idleMaxRPM = minRPM + engineData.IdleRange;

        float currentStep = Mathf.PingPong(currentPistonStep, 1.0f);

        if (currentStep >= 1.0f)
        {
            currentPistonStep = 0.0f;
        }

        return Mathf.Lerp(idleMinRPM, idleMaxRPM, currentStep);
    }

    private float RedlineRPM()
    {
        currentPistonStep += Time.deltaTime * 2.5f;

        float redLineMin = maxRPM - (engineData.RedLineRange);
        float redLineMax = maxRPM + (engineData.RedLineRange);

        float currentStep = Mathf.PingPong(currentPistonStep, 1.0f);

        if (currentStep >= 1.0f)
        {
            currentPistonStep = 0.0f;
        }

        return Mathf.Lerp(redLineMin, redLineMax, currentStep);
    }
}
