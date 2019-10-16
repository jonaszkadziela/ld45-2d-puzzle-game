using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public static bool LevelGenerated;

    public Tilemap groundTileMap;
    public RangeInt groundPlayAreaX;
    public RangeInt groundPlayAreaY;
    public Tile[] safeTiles;

    public Vector2 playerSpawnPosition;
    public GameObject playerPrefab;
    public GameObject playerStatisticsUI;

    public GameObject map;
    public GameObject sandstonePrefab;
    public GameObject granitePrefab;

    [HideInInspector] public List<GameObject> rocksList;

    private GameObject levelContainer;

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
        rocksList = new List<GameObject>();

        levelContainer = new GameObject("Level Elements");
        levelContainer.transform.parent = map.transform;

        CurrentRound = 0;
        NewRound();

        InitializePlayer();
    }

    void Update()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        if (!LevelGenerated)
        {
            int rocksAmount = Random.Range(
                GameSettings.RockSpawnAmount.min,
                GameSettings.RockSpawnAmount.max
            );

            GenerateLevel(rocksAmount);

            RoundStartTime = Time.time;
        }
        else
        {
            if (rocksList.Count < GameSettings.RockSpawnMoreThreshold)
            {
                int rocksAmount = Random.Range(
                    GameSettings.RockSpawnAmount.min - GameSettings.RockSpawnMoreThreshold,
                    GameSettings.RockSpawnAmount.max - GameSettings.RockSpawnMoreThreshold
                );
                GenerateLevel(rocksAmount);
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
        LevelGenerated = false;

        foreach (GameObject rock in rocksList)
        {
            Destroy(rock);
        }

        TargetNumber = Random.Range(
            GameSettings.TargetNumberRange.min,
            GameSettings.TargetNumberRange.max
        );
        TargetNumberMargin = Random.Range(
            GameSettings.TargetMarginRange.min,
            GameSettings.TargetMarginRange.max
        );

        CurrentRound++;
        CurrentNumber = 0;

        if (CurrentRound > 1)
        {
            AudioManager.Instance.PlaySoundEffect("Puzzle-New");
            AudioLayersManager.Instance.Unmute("Gameplay-Loop");
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

    private void InitializePlayer()
    {
        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        playerStatisticsUI.SetActive(true);
    }

    private void GenerateLevel(int rocksAmount)
    {
        for (int i = 0; i < rocksAmount; i++)
        {
            bool tileGenerated = false;

            Vector3Int randomPosition = Vector3Int.zero;

            randomPosition.x = Random.Range(groundPlayAreaX.min, groundPlayAreaX.max);
            randomPosition.y = Random.Range(groundPlayAreaY.min, groundPlayAreaY.max);

            foreach (Tile tile in safeTiles)
            {
                if (groundTileMap.GetTile(randomPosition) == tile)
                {
                    GameObject prefab = sandstonePrefab;

                    if (GameSettings.GranitePercentage >= Random.Range(0f, 1f))
                    {
                        prefab = granitePrefab;
                    }

                    GameObject rockGO = Instantiate(prefab, randomPosition, Quaternion.identity);
                    rockGO.transform.parent = levelContainer.transform;
                    rocksList.Add(rockGO);

                    tileGenerated = true;

                    break;
                }
            }

            if (!tileGenerated)
            {
                i--;
            }
        }

        LevelGenerated = true;
    }
}
