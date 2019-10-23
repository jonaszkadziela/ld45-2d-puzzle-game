using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject statisticsUI;

    public TextMeshProUGUI roundsCompletedValue;

    public void Show()
    {
        statisticsUI.SetActive(false);
        gameOverUI.SetActive(true);

        roundsCompletedValue.text = GameplayManager.CurrentRound.ToString();
    }

    public void Menu()
    {
        SceneFade.Instance.FadeTo(SceneEnum.MainMenu);
    }

    public void Restart()
    {
        SceneFade.Instance.FadeTo(SceneEnum.Game);
    }
}
