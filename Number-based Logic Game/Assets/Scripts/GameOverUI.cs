using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject statisticsUI;

    public TextMeshProUGUI roundsCompletedValue;

    void Update()
    {
        if (GameManager.GameOver)
        {
            if (!gameOverUI.activeSelf)
            {
                statisticsUI.SetActive(false);
                gameOverUI.SetActive(true);

                roundsCompletedValue.text = GameplayManager.CurrentRound.ToString();
            }
        }
        else
        {
            statisticsUI.SetActive(true);
            gameOverUI.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {

    }
}
