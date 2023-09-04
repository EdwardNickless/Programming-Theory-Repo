using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private VehicleBehaviour vehicle;
    [SerializeField] TMP_Text speedometerText;

    private TMP_Text gearText;

    private void Awake()
    {
        gearText = transform.Find("Gear").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        gearText.text = "N";
        speedometerText.text = "0 mph";
    }

    private void FixedUpdate()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        UpdateSpeedometerText(vehicle.CurrentSpeed);
        UpdateGearText(vehicle.CurrentGear);
    }

    public void UpdateSpeedometerText(float currentSpeed)
    {
        speedometerText.text = currentSpeed + " mph";
    }

    public void UpdateGearText(int currentGear)
    {
        if (currentGear == 0)
        {
            gearText.text = "N";
        }
        else if (currentGear == -1)
        {
            gearText.text = "R";
        }
        else
        {
            gearText.text = currentGear.ToString();
        }
    }
}
