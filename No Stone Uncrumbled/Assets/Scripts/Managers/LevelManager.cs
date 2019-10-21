using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public static List<GameObject> ObjectsList;
    public static bool LevelGenerated;
    public static bool SpawningRocks;

    [Header("Player")]
    public Vector2 playerSpawnPosition;
    public GameObject playerPrefab;
    public GameObject playerStatisticsUI;

    [Header("Map")]
    public string levelContainerName = "Level Elements";
    public int maxRandomPositionAttempts = 20;

    public Tilemap groundTilemap;
    public Tile[] safeTiles;

    public GameObject sandstonePrefab;
    public GameObject granitePrefab;

    private GameObject levelContainer;
    private GameObject playerGO = null;

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

        LevelGenerated = false;
        SpawningRocks = false;

        ObjectsList = new List<GameObject>();

        levelContainer = new GameObject(levelContainerName);
        levelContainer.transform.parent = groundTilemap.transform.parent;
    }

    public void GenerateLevel()
    {
        if (!playerGO)
        {
            SpawnPlayer();
        }

        int rocksAmount = Random.Range(
            GameSettings.RockSpawnAmountRange.min,
            GameSettings.RockSpawnAmountRange.max
        );

        SpawnRocks(rocksAmount);

        LevelGenerated = true;
    }

    public void ClearLevel()
    {
        foreach (GameObject obj in ObjectsList)
        {
            Rock rock = obj.GetComponent<Rock>();

            if (rock)
            {
                Destroy(rock.gameObject);
            }
        }

        LevelGenerated = false;
    }

    public void SpawnRocks(int rocksAmount)
    {
        StartCoroutine(SpawnRocksCoroutine(rocksAmount));
    }

    public void SpawnPlayer()
    {
        playerGO = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        ObjectsList.Add(playerGO);

        playerStatisticsUI.SetActive(true);
    }

    private IEnumerator SpawnRocksCoroutine(int rocksAmount)
    {
        SpawningRocks = true;

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

        SpawningRocks = false;
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
                if (attemptCount > maxRandomPositionAttempts)
                {
                    return randomPosition;
                }
            }
            else
            {
                continue;
            }

            // Check if the position does not collide with any other dynamically generated object
            foreach (GameObject obj in ObjectsList)
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
}
