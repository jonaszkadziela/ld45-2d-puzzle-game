using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool GameOver;

    public GameOverUI gameOverUI;

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
        SavesManager.Save();
        gameOverUI.Show();
    }

    public void QuitGame()
    {
        SavesManager.Save();
        SceneFade.Instance.FadeOut();
        Invoke("ApplicationQuit", SceneFade.Instance.fadeDuration);
    }

    private void ApplicationQuit()
    {
        Application.Quit();
    }
}
