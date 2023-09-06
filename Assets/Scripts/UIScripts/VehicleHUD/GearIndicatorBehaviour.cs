using TMPro;
using UnityEngine;

public class GearIndicatorBehaviour : MonoBehaviour
{
    [SerializeField] private VehicleBehaviour vehicle;

    private TMP_Text gearText;
    private int lastKnownGear;

    private void Awake()
    {
        gearText = GetComponent<TMP_Text>();
    }

    private void FixedUpdate()
    {
        UpdateGearText(vehicle.CurrentGear);
    }
    
    public void UpdateGearText(int currentGear)
    {
        if(currentGear == lastKnownGear)
        {
            return;
        }
        
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
        lastKnownGear = currentGear;
    }
}
