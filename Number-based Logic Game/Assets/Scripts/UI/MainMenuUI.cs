using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private bool musicIsPlaying = false;

    void Update()
    {
        if (AudioLayersManager.Instance && !musicIsPlaying)
        {
            AudioLayersManager.Instance.Unmute("MainMenuLoop");
            musicIsPlaying = true;
        }
    }

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
