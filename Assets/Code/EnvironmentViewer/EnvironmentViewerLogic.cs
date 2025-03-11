using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentViewerLogic : MonoBehaviour
{
    public Tilemap tilemap; // Assign in Inspector
    public TileBase tile;   // Assign a tile in Inspector

    private string environmentId;
    private Environment2D environment = new()
    {
        name = "name",
        maxWidth = 40,
        maxHeight = 40
    };

    void Start()
    {
        environmentId = SceneData.id;  // Retrieve the ID

        // Create the tilemap based on the environment size
        GenerateTilemap();
    }

    void GenerateTilemap()
    {
        tilemap.ClearAllTiles(); // Ensure it's empty before generating

        for (int x = 0; x < environment.maxWidth; x++)
        {
            for (int y = 0; y < environment.maxHeight; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        Debug.Log("Tilemap generated: " + environment.maxWidth + " x " + environment.maxHeight);
    }
}
