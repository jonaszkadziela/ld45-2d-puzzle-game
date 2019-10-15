using UnityEngine;
using UnityEngine.Tilemaps;

public class FogManager : MonoBehaviour
{
    public Tilemap darkOverlay;
    public Tilemap transparentOverlay;
    public Tilemap groundTilemap;

    public Tile darkTile;
    public Tile transparentTile;

    void Start()
    {
        darkOverlay.origin = groundTilemap.origin;
        darkOverlay.size = groundTilemap.size;

        transparentOverlay.origin = groundTilemap.origin;
        transparentOverlay.size = groundTilemap.size;

        foreach (Vector3Int pos in darkOverlay.cellBounds.allPositionsWithin)
        {
            darkOverlay.SetTile(pos, darkTile);
        }
        foreach (Vector3Int pos in darkOverlay.cellBounds.allPositionsWithin)
        {
            transparentOverlay.SetTile(pos, transparentTile);
        }
    }
}
