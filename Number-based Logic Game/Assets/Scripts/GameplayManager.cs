using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    public static int CurrentRound = 0;
    public static int CurrentNumber = 0;
    public static int TargetNumber;
    public static int TargetNumberMargin;
    public static float RoundStartTime;

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
    private bool levelGenerated;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        stonesList = new List<GameObject>();
    }

    void Update()
    {
        switch (GameManager.State)
        {
            case GameState.NumericPuzzle:
            break;

            case GameState.CollectingNumbers:
                if (!levelGenerated)
                {
                    int stonesAmount = Random.Range(
                        GameSettings.Instance.stoneSpawnAmount.min,
                        GameSettings.Instance.stoneSpawnAmount.max
                    );
                    GenerateLevel(stonesAmount);

                    Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);

                    CurrentRound++;
                    RoundStartTime = Time.time;
                    TargetNumberMargin = Random.Range(
                        GameSettings.Instance.targetMargin.min,
                        GameSettings.Instance.targetMargin.max
                    );
                }
                else
                {
                    if (stonesList.Count < GameSettings.Instance.stoneSpawnMoreThreshold)
                    {
                        int stonesAmount = Random.Range(
                            GameSettings.Instance.stoneSpawnAmount.min - GameSettings.Instance.stoneSpawnMoreThreshold,
                            GameSettings.Instance.stoneSpawnAmount.max - GameSettings.Instance.stoneSpawnMoreThreshold
                        );
                        GenerateLevel(stonesAmount);
                    }
                }
            break;

            case GameState.GameOver:
            default:
            break;
        }
    }

    void GenerateLevel(int stonesAmount)
    {
        if (!levelGenerated)
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
                        GameSettings.Instance.stoneInitialNumberRange.min,
                        GameSettings.Instance.stoneInitialNumberRange.max
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

        levelGenerated = true;
    }
}
