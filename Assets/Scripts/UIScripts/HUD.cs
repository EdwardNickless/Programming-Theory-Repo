using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text gearText;
    [SerializeField] TMP_Text speedometerText;
    [SerializeField] TMP_Text rpmText;

    private void Awake()
    {
        gearText.text = "Gear: 1";
        speedometerText.text = "0 mph";
    }

    public void UpdateHUD(int currentGear, float currentSpeed, float currentRPM)
    {
        UpdateGearText(currentGear);
        UpdateSpeedometerText(currentSpeed);
        UpdateRPMText(currentRPM);
    }

    private void UpdateGearText(int currentGear)
    {
        if (currentGear == 0)
        {
            gearText.text = "Gear: Neutral";
        }
        else if (currentGear == -1)
        {
            gearText.text = "Gear: Reverse";
        }
        else
        {
            gearText.text = "Gear: " + currentGear;
        }
    }

    private void UpdateSpeedometerText(float currentSpeed)
    {
        speedometerText.text = Mathf.RoundToInt(currentSpeed) + " mph";
    }

    private void UpdateRPMText(float currentRPM)
    {
        rpmText.text = Mathf.RoundToInt(currentRPM) + " rpm";
    }
}
