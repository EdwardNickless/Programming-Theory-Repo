using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelectionNavigation : SceneNavigation
{
    public override void Exit()
    {
        SceneManager.LoadScene(GameManager.Scenes.StartMenu.ToString());
    }

    public void NextCar()
    {
        Debug.Log("Next Car Clicked");
    }

    public void PreviousCar()
    {
        Debug.Log("Previous Car Clicked");
    }

    public override void ChooseCar()
    {
        // TODO: Implement Car Selection Logic before scene load
        SceneManager.LoadScene(GameManager.Scenes.StartMenu.ToString());
    }
}
