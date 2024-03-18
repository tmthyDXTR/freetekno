using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // Variables to store item information
    public string itemName;
    public int itemCost;

    // Method to initialize item information
    public void InitializeInfo(string name, int cost)
    {
        itemName = name;
        itemCost = cost;
    }
}
