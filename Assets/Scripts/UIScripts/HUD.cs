using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text gearText;
    [SerializeField] TMP_Text speedometerText;
    [SerializeField] TMP_Text rpmText;

    [SerializeField] private VehicleBehaviour vehicle;

    private void Awake()
    {
        gearText.text = "Gear: N";
        speedometerText.text = "0 mph";
        rpmText.text = "0 rpm";
    }

    private void FixedUpdate()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        UpdateSpeedometerText(vehicle.CurrentSpeed);
        UpdateRPMText(vehicle.CurrentRPM);
        UpdateGearText(vehicle.CurrentGear);
    }

    public void UpdateSpeedometerText(float currentSpeed)
    {
        speedometerText.text = currentSpeed + " mph";
    }

    public void UpdateRPMText(float currentRPM)
    {
        rpmText.text = Mathf.RoundToInt(currentRPM) + " rpm";
    }

    public void UpdateGearText(int currentGear)
    {
        if (currentGear == 0)
        {
            gearText.text = "Gear: N";
        }
        else if (currentGear == -1)
        {
            gearText.text = "Gear: R";
        }
        else
        {
            gearText.text = "Gear: " + currentGear;
        }
    }
}
