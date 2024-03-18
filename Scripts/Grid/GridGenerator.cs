using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject objectPrefab; // GameObject to instantiate around the grid

    public int gridSizeX;
    public int gridSizeY;
    public float objectSpacing; // Spacing between the objects


    public bool generateGrid = false;

    public Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>();

    private void Start()
    {
        if (generateGrid)
        {
            ClearGrid();
            GenerateGrid();
        }
        else
        {
            InitializeExistingGrid();
        }
        AddObjectsAroundGrid();

        // foreach (var kvp in tiles)
        // {
        //     Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
        // }
    }

    private void GenerateGrid()
    {
        float tileWidth = tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float tileHeight = tilePrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = new Vector3(x * tileWidth, y * tileHeight, 0f);

                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.transform.SetParent(transform);

                // Add TileInfo component and set grid position
                TileInfo tileInfo = tile.AddComponent<TileInfo>();
                tileInfo.gridPosition = new Vector2Int(x, y);

                tiles.Add(new Vector2Int(x, y), tile);
            }
        }
        Debug.Log("Grid generated");
    }

    public TileInfo GetTileInfo(int x, int y)
    {
        Vector2Int gridCoordinates = new Vector2Int(x, y);
        return GetTileInfo(gridCoordinates);
    }

    public TileInfo GetTileInfo(Vector2Int gridCoordinates)
    {
        if (tiles.ContainsKey(gridCoordinates))
        {
            return tiles[gridCoordinates].GetComponent<TileInfo>();
        }
        else
        {
            Debug.LogWarning("No tile found at coordinates: " + gridCoordinates);
            return null;
        }
    }

    public TileInfo GetNeighborTile(Vector2Int gridCoordinates, Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return GetTileInfo(new Vector2Int(gridCoordinates.x - 1, gridCoordinates.y));
            case Direction.Right:
                return GetTileInfo(new Vector2Int(gridCoordinates.x + 1, gridCoordinates.y));
            case Direction.Up:
                return GetTileInfo(new Vector2Int(gridCoordinates.x, gridCoordinates.y + 1));
            case Direction.Down:
                return GetTileInfo(new Vector2Int(gridCoordinates.x, gridCoordinates.y - 1));
            default:
                return null;
        }
    }
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private void AddObjectsAroundGrid()
    {
        float tileWidth = objectPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float tileHeight = objectPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        // Calculate positions for top and bottom edges
        for (int x = 0; x < gridSizeX; x++)
        {
            Vector3 topPosition = new Vector3(x * tileWidth, gridSizeY * tileHeight, 0f);
            Vector3 bottomPosition = new Vector3(x * tileWidth, -1 * tileHeight, 0f);

            Instantiate(objectPrefab, topPosition, Quaternion.identity);
            Instantiate(objectPrefab, bottomPosition, Quaternion.identity);
        }

        // Calculate positions for left and right edges
        for (int y = 0; y < gridSizeY; y++)
        {
            Vector3 leftPosition = new Vector3(-1 * tileHeight, y * tileWidth, 0f);
            Vector3 rightPosition = new Vector3(gridSizeX * tileHeight, y * tileWidth, 0f);

            Instantiate(objectPrefab, leftPosition, Quaternion.identity);
            Instantiate(objectPrefab, rightPosition, Quaternion.identity);
        }
    }

    private void InitializeExistingGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            TileInfo tileInfo = child.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                // Get the grid position from the TileInfo component
                Vector2Int gridPosition = tileInfo.gridPosition;

                // Add the tile to the tiles dictionary
                tiles.Add(gridPosition, child);
            }
        }
        Debug.Log("Initialized existing grid");
    }

    private void ClearGrid()
    {
        // Destroy all existing tiles and clear the tiles dictionary
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(transform.GetChild(i).gameObject);
        }

        tiles.Clear();
        Debug.Log("Grid cleared");
    }
}
