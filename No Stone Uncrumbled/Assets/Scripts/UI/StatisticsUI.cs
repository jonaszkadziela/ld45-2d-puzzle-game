using System;
using UnityEngine;
using TMPro;

public class StatisticsUI : MonoBehaviour
{
    [Header("Left Panel")]
    public TextMeshProUGUI energyValueText;
    public TextMeshProUGUI moneyValueText;

    [Header("Right Panel")]
    public TextMeshProUGUI roundValueText;
    public TextMeshProUGUI roundTimeValueText;
    public TextMeshProUGUI numberValueText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI targetValueText;

    private string initialTargetText = "Target";

    void Update()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        // Left Panel
        energyValueText.text = Mathf.RoundToInt(PlayerController.Instance.energy).ToString();
        moneyValueText.text = PlayerController.Instance.money.ToString();
        
        // Right Panel
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
