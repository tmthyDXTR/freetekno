using UnityEngine;
// Import the Direction enum from the GridGenerator script
using static GridGenerator;
public class ItemMovement : MonoBehaviour
{
    public GridGenerator gridGenerator; // Reference to the GridGenerator script

    public TileInfo selectedTileInfo;

    private void Start()
    {
        // Initialize the selected tile info using an instance of GridGenerator
        gridGenerator = FindObjectOfType<GridGenerator>(); // Get the GridGenerator instance in the scene

        // Initialize the selected tile info
        selectedTileInfo = gridGenerator.GetTileInfo(0, 0);
    }

    private void Update()
    {
        // Check arrow key inputs
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Direction.Down);
        }
    }

    // Method to move the item in the specified direction
    private void Move(Direction direction)
    {
        // Get the neighbor tile info in the specified direction
        TileInfo neighborTile = gridGenerator.GetNeighborTile(selectedTileInfo.gridPosition, direction);

        // Check if the neighbor tile exists
        if (neighborTile != null)
        {
            // Move towards the neighbor tile's world position
            transform.position = neighborTile.transform.position;
            // Update the selected tile info
            selectedTileInfo = neighborTile;
        }
    }
}
