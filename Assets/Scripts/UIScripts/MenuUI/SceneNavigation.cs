using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneNavigation : MonoBehaviour
{
    public void StartRace()
    {
        SceneManager.LoadScene(GameManager.Scenes.Race.ToString());
    }

    public virtual void ChooseCar()
    {
        SceneManager.LoadScene(GameManager.Scenes.CarSelection.ToString());
    }

    public void ViewHighScores()
    {
        SceneManager.LoadScene(GameManager.Scenes.HighScores.ToString());
    }

    public virtual void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
