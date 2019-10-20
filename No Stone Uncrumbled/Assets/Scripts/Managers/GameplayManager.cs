using UnityEngine;

public enum NumberOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
};

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    public static float RoundStartTime;
    public static int CurrentRound;
    public static int CurrentNumber;
    public static int TargetNumber;
    public static int TargetNumberMargin;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CurrentRound = 0;

        NewRound();
    }

    void Update()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        if (LevelManager.LevelGenerated)
        {
            if (!LevelManager.SpawningRocks && LevelManager.ObjectsList.Count < GameSettings.RockSpawnMoreThreshold)
            {
                int rocksAmount = Random.Range(
                    GameSettings.RockSpawnAmount.min - GameSettings.RockSpawnMoreThreshold,
                    GameSettings.RockSpawnAmount.max - GameSettings.RockSpawnMoreThreshold
                );
                LevelManager.Instance.SpawnRocks(rocksAmount);
            }
        }
    }

    public void ChangeCurrentNumber(NumberOperation operation, int number)
    {
        if (GameManager.GameOver)
        {
            return;
        }

        switch (operation)
        {
            case NumberOperation.Addition:
                CurrentNumber += number;
            break;

            case NumberOperation.Subtraction:
                CurrentNumber -= number;
            break;

            case NumberOperation.Multiplication:
                CurrentNumber *= number;
            break;

            case NumberOperation.Division:
                CurrentNumber /= number;
            break;
        }

        if (Mathf.Abs(TargetNumber - CurrentNumber) <= TargetNumberMargin)
        {
            CompletedRound();
        }
    }

    private void CompletedRound()
    {
        GameSettings.TargetNumberRange += GameSettings.TargetNumberIncrease;
        GameSettings.TargetMarginRange -= GameSettings.TargetMarginDecrease;

        GameSettings.TargetMarginRange.min = Mathf.Max(
            GameSettings.TargetMarginRange.min,
            GameSettings.MinTargetMarginRange.min
        );
        GameSettings.TargetMarginRange.max = Mathf.Max(
            GameSettings.TargetMarginRange.max,
            GameSettings.MinTargetMarginRange.max
        );

        DetermineReward();
        NewRound();
    }

    private void DetermineReward()
    {
        float roundTime = Time.time - RoundStartTime;
        int onTargetFactor = 1 - Mathf.Abs(TargetNumber - CurrentNumber) / TargetNumberMargin;

        PlayerController.Instance.initialEnergy = PlayerController.Instance.energy;
        PlayerController.Instance.distanceMoved = 0f;

        PlayerController.Instance.money += Mathf.Max(
            GameSettings.MoneyRewardRange.min,
            GameSettings.MoneyRewardRange.max - GameSettings.MoneyRewardDecreasePerMinute * (int)roundTime / 60
        );
        PlayerController.Instance.initialEnergy += Mathf.Max(
            GameSettings.EnergyRewardRange.min,
            GameSettings.EnergyRewardRange.max * onTargetFactor
        );
    }

    private void NewRound()
    {
        LevelManager.Instance.ClearLevel();

        TargetNumber = Random.Range(
            GameSettings.TargetNumberRange.min,
            GameSettings.TargetNumberRange.max
        );
        TargetNumberMargin = Random.Range(
            GameSettings.TargetMarginRange.min,
            GameSettings.TargetMarginRange.max
        );

        CurrentNumber = 0;
        CurrentRound++;
        RoundStartTime = Time.time;

        if (CurrentRound > 1)
        {
            AudioManager.Instance.PlaySoundEffect("Puzzle-New");
            AudioLayersManager.Instance.Unmute("Gameplay-Loop");
        }

        LevelManager.Instance.GenerateLevel();
    }
}
