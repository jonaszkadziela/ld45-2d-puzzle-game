using System;
using UnityEngine;
using TMPro;

public class StatisticsUI : MonoBehaviour
{
    public string initialTargetText = "Target";

    public GameObject statisticsLeftPanel;
    public GameObject statisticsRightPanel;

    public TextMeshProUGUI energyValueText;
    public TextMeshProUGUI moneyValueText;

    public TextMeshProUGUI roundValueText;
    public TextMeshProUGUI roundTimeValueText;
    public TextMeshProUGUI numberValueText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI targetValueText;

    void Update()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        if (PlayerController.Instance)
        {
            statisticsLeftPanel.SetActive(true);

            energyValueText.text = ((int)PlayerController.Instance.energy).ToString();
            moneyValueText.text = PlayerController.Instance.money.ToString();
        }
        else
        {
            statisticsLeftPanel.SetActive(false);
        }

        statisticsRightPanel.SetActive(true);

        roundValueText.text = GameplayManager.CurrentRound.ToString();

        float roundTime = Time.time - GameplayManager.RoundStartTime;
        string timeFormat = ((int)roundTime / 3600) > 0 ? @"hh\:mm\:ss" : @"mm\:ss";

        TimeSpan roundTimeSpan = TimeSpan.FromSeconds(roundTime);
        roundTimeValueText.text = roundTimeSpan.ToString(timeFormat);

        numberValueText.text = GameplayManager.CurrentNumber.ToString();

        targetText.text = initialTargetText + $" (+/- { GameplayManager.TargetNumberMargin })";
        targetValueText.text = GameplayManager.TargetNumber.ToString();
    }
}
