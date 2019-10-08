using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool GameOver;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GameOver = false;
    }

    void Update()
    {
        // TODO: Implement a proper Pause UI with quit button
        if (Input.GetButtonDown("Cancel"))
        {
            QuitGame();
        }
    }

    public void TriggerGameOver()
    {
        AudioLayersManager.Instance.Reset();
        AudioManager.Instance.PlaySoundEffect("GameOver");
        GameOver = true;
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
