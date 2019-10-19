using UnityEngine;
using UnityEngine.Tilemaps;

public class FogManager : MonoBehaviour
{
    public Tilemap referenceTilemap;
    public Tilemap darkOverlay;
    public Tilemap transparentOverlay;

    public Tile darkTile;
    public Tile transparentTile;

    void Start()
    {
        darkOverlay.origin = referenceTilemap.origin;
        darkOverlay.size = referenceTilemap.size;

        transparentOverlay.origin = referenceTilemap.origin;
        transparentOverlay.size = referenceTilemap.size;

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
