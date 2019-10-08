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

    public static int CurrentRound;
    public static int CurrentNumber;
    public static int TargetNumber;
    public static int TargetNumberMargin;
    public static float RoundStartTime;

    public static bool LevelGenerated;

    public Tilemap groundTileMap;
    public RangeInt groundPlayAreaX;
    public RangeInt groundPlayAreaY;
    public Tile[] safeTiles;

    public Vector2 playerSpawnPosition;

    public GameObject map;
    public GameObject sandStonePrefab;
    public GameObject boulderPrefab;
    public GameObject playerPrefab;

    public List<GameObject> stonesList;

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
        stonesList = new List<GameObject>();

        levelContainer = new GameObject("Level Elements");
        levelContainer.transform.parent = map.transform;

        CurrentRound = 0;
        NewRound();
    }

    void Update()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        if (!LevelGenerated)
        {
            int stonesAmount = Random.Range(
                GameSettings.StoneSpawnAmount.min,
                GameSettings.StoneSpawnAmount.max
            );

            GenerateLevel(stonesAmount);
            Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);

            RoundStartTime = Time.time;
        }
        else
        {
            if (stonesList.Count < GameSettings.StoneSpawnMoreThreshold)
            {
                int stonesAmount = Random.Range(
                    GameSettings.StoneSpawnAmount.min - GameSettings.StoneSpawnMoreThreshold,
                    GameSettings.StoneSpawnAmount.max - GameSettings.StoneSpawnMoreThreshold
                );
                GenerateLevel(stonesAmount);
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

        foreach (GameObject stone in stonesList)
        {
            Destroy(stone);
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
            AudioManager.Instance.PlaySoundEffect("NewPuzzle");
            AudioLayersManager.Instance.Unmute("GameplayLoop");
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

    private void GenerateLevel(int stonesAmount)
    {
        for (int i = 0; i < stonesAmount; i++)
        {
            bool tileGenerated = false;

            Vector3Int randomPosition = Vector3Int.zero;

            randomPosition.x = Random.Range(groundPlayAreaX.min, groundPlayAreaX.max);
            randomPosition.y = Random.Range(groundPlayAreaY.min, groundPlayAreaY.max);

            foreach (Tile tile in safeTiles)
            {
                if (groundTileMap.GetTile(randomPosition) == tile)
                {
                    GameObject prefab = sandStonePrefab;

                    if (GameSettings.BoulderPercentage >= Random.Range(0f, 1f))
                    {
                        prefab = boulderPrefab;
                    }

                    GameObject stoneGO = Instantiate(prefab, randomPosition, Quaternion.identity);
                    stoneGO.transform.parent = levelContainer.transform;
                    stonesList.Add(stoneGO);

                    Stone stone = stoneGO.GetComponent<Stone>();

                    int randomNumber = Random.Range(
                        GameSettings.StoneInitialNumberRange.min,
                        GameSettings.StoneInitialNumberRange.max
                    );
                    stone.initialNumber = randomNumber;

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
