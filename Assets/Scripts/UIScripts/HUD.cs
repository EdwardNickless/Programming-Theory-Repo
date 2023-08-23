using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text gear;

    private void Awake()
    {
        gear.text = "Gear: 1";
    }

    public void UpdateGear(int currentGear)
    {
        gear.text = "Gear: " + currentGear;
    }
}
