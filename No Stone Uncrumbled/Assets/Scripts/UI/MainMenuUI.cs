using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void Play()
    {
        SceneFade.Instance.FadeTo("Game");
    }

    public void QuitGame()
    {
        SceneFade.Instance.FadeOut();
        Invoke("ApplicationQuit", SceneFade.Instance.fadeDuration);
    }

    private void ApplicationQuit()
    {
        Application.Quit();
    }
}
