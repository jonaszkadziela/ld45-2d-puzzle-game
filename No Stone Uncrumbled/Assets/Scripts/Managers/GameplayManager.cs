using System.Collections;
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

    [Header("Player")]
    public Vector2 playerSpawnPosition;
    public GameObject playerPrefab;
    public GameObject playerStatisticsUI;

    [Header("Map")]
    public Tilemap groundTilemap;
    public Tile[] safeTiles;

    public GameObject sandstonePrefab;
    public GameObject granitePrefab;

    [HideInInspector] public List<GameObject> objectsList;

    private GameObject levelContainer;
    private GameObject playerGO = null;
    private int maxAttemptCount = 20;
    private bool spawningRocks = false;

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
        objectsList = new List<GameObject>();

        levelContainer = new GameObject("Level Elements");
        levelContainer.transform.parent = groundTilemap.transform.parent;

        CurrentRound = 0;

        NewRound();
    }

    void Update()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        if (LevelGenerated)
        {
            if (!spawningRocks && objectsList.Count < GameSettings.RockSpawnMoreThreshold)
            {
                int rocksAmount = Random.Range(
                    GameSettings.RockSpawnAmount.min - GameSettings.RockSpawnMoreThreshold,
                    GameSettings.RockSpawnAmount.max - GameSettings.RockSpawnMoreThreshold
                );
                StartCoroutine(SpawnRocks(rocksAmount));
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
        foreach (GameObject obj in objectsList)
        {
            Rock rock = obj.GetComponent<Rock>();

            if (rock)
            {
                Destroy(rock.gameObject);
            }
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
        RoundStartTime = Time.time;

        if (CurrentRound > 1)
        {
            AudioManager.Instance.PlaySoundEffect("Puzzle-New");
            AudioLayersManager.Instance.Unmute("Gameplay-Loop");
        }

        GenerateLevel();
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

    private Vector3Int GetRandomSpawnPosition()
    {
        Vector3Int randomPosition;
        int attemptCount = 0;
        bool validPosition = false;

        do
        {
            attemptCount++;
            randomPosition = new Vector3Int(
                Random.Range(groundTilemap.cellBounds.min.x, groundTilemap.cellBounds.max.x),
                Random.Range(groundTilemap.cellBounds.min.y, groundTilemap.cellBounds.max.y),
                groundTilemap.origin.z
            );

            // Check if the position is within a safe tile
            foreach (Tile tile in safeTiles)
            {
                if (groundTilemap.HasTile(randomPosition) &&
                    groundTilemap.GetTile(randomPosition) == tile)
                {
                    validPosition = true;
                    break;
                }
            }
            if (validPosition)
            {
                // If we have tried too many times, do not make any further checks
                // Just return a position which is within a safe tile
                if (attemptCount > maxAttemptCount)
                {
                    return randomPosition;
                }
            }
            else
            {
                continue;
            }

            // Check if the position does not collide with any other dynamically generated object
            foreach (GameObject obj in objectsList)
            {
                Collider2D objCollider = obj.GetComponent<Collider2D>();
                Vector2Int randomPosition2D = new Vector2Int(randomPosition.x, randomPosition.y);

                if (objCollider.OverlapPoint(randomPosition2D))
                {
                    validPosition = false;
                    break;
                }
            }
        }
        while (!validPosition);

        return randomPosition;
    }

    private void GenerateLevel()
    {
        if (!playerGO)
        {
            SpawnPlayer();
        }

        int rocksAmount = Random.Range(
            GameSettings.RockSpawnAmount.min,
            GameSettings.RockSpawnAmount.max
        );

        StartCoroutine(SpawnRocks(rocksAmount));

        LevelGenerated = true;
    }

    private void SpawnPlayer()
    {
        playerGO = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        objectsList.Add(playerGO);

        playerStatisticsUI.SetActive(true);
    }

    private IEnumerator SpawnRocks(int rocksAmount)
    {
        spawningRocks = true;

        for (int i = 0; i < rocksAmount; i++)
        {
            Vector3Int randomPosition = GetRandomSpawnPosition();
            GameObject prefab = sandstonePrefab;

            if (GameSettings.GranitePercentage >= Random.Range(0f, 1f))
            {
                prefab = granitePrefab;
            }

            GameObject rockGO = Instantiate(prefab, randomPosition, Quaternion.identity);
            rockGO.transform.parent = levelContainer.transform;

            yield return 0;
        }

        spawningRocks = false;
    }
}
