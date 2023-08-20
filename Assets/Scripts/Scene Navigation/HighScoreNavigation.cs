using UnityEngine.SceneManagement;

public class HighScoreNavigation : SceneNavigation
{
    public override void Exit()
    {
        SceneManager.LoadScene(GameManager.Scenes.StartMenu.ToString());
    }
}
