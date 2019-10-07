using UnityEngine;
using UnityEngine.Tilemaps;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    public Tilemap groundTileMap;
    public RangeInt groundPlayAreaX;
    public RangeInt groundPlayAreaY;
    public Tile[] safeTiles;

    public GameObject map;
    public GameObject stonePrefab;

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
        }
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
                    GenerateLevel();
                }
            break;

            case GameState.GameOver:
            default:
            break;
        }
    }

    void GenerateLevel()
    {
        int stonesAmount = Random.Range(GameSettings.Instance.stoneSpawnRate.min, GameSettings.Instance.stoneSpawnRate.max);
        GameObject levelContainer = new GameObject("Level Elements");
        levelContainer.transform.parent = map.transform;

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
                    Stone stone = stoneGO.GetComponent<Stone>();

                    int randomNumber = Random.Range(GameSettings.Instance.stoneNumberRange.min, GameSettings.Instance.stoneNumberRange.max);
                    stone.initialNumber = randomNumber;
                    stoneGO.transform.parent = levelContainer.transform;
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
