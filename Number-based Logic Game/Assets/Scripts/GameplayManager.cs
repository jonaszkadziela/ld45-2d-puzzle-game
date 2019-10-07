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

    public static int CurrentRound = 0;
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
    public GameObject stonePrefab;
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

    private void NewRound()
    {
        TargetNumber = Random.Range(
            GameSettings.TargetNumber.min,
            GameSettings.TargetNumber.max
        );
        TargetNumberMargin = Random.Range(
            GameSettings.TargetMargin.min,
            GameSettings.TargetMargin.max
        );

        CurrentRound++;
        CurrentNumber = 0;

        LevelGenerated = false;
    }

    private void CompletedRound()
    {
        GameSettings.TargetNumber += GameSettings.TargetNumberIncrease;
        GameSettings.TargetMargin -= GameSettings.TargetMarginDecrease;

        GameSettings.TargetMargin.min = Mathf.Max(GameSettings.TargetMargin.min, GameSettings.MinTargetMargin.min);
        GameSettings.TargetMargin.max = Mathf.Max(GameSettings.TargetMargin.max, GameSettings.MinTargetMargin.max);

        NewRound();
    }

    private void GenerateLevel(int stonesAmount)
    {
        if (!LevelGenerated)
        {
            levelContainer = new GameObject("Level Elements");
            levelContainer.transform.parent = map.transform;
        }

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
                    GameObject stoneGO = Instantiate(stonePrefab, randomPosition, Quaternion.identity);
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
