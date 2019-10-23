using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject statisticsUI;

    public TextMeshProUGUI currentRoundValue;
    public TextMeshProUGUI highScoreValue;

    public float animateScoreDelay = 1.5f;

    void OnEnable()
    {
        statisticsUI.SetActive(false);

        currentRoundValue.text = "0";
        highScoreValue.text = "0";

        Invoke("AnimateScore", animateScoreDelay);
    }

    public void Menu()
    {
        SceneFade.Instance.FadeTo(SceneEnum.MainMenu);
    }

    public void Restart()
    {
        SceneFade.Instance.FadeTo(SceneEnum.Game);
    }

    private void AnimateScore()
    {
        UIManager.Instance.AnimatedCounter(currentRoundValue, GameplayManager.CurrentRound);
        UIManager.Instance.AnimatedCounter(highScoreValue, GameplayManager.HighScore);
    }
}
