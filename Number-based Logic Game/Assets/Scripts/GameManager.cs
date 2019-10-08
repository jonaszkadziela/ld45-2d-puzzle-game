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

    public void TriggerGameOver()
    {
        AudioLayersManager.Instance.Reset();
        AudioManager.Instance.PlaySoundEffect("GameOver");
        GameOver = true;
    }

    public void RestartGame()
    {
        SceneFade.Instance.FadeTo("GameMap");
    }
}
