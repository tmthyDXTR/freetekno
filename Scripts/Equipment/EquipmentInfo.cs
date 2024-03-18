using UnityEngine;
using System.Collections.Generic;

public static class EquipmentInfo
{
    // Dictionary to store information about each equipment item
    public static Dictionary<string, EquipmentItemInfo> items = new Dictionary<string, EquipmentItemInfo>()
    {
        {
            "Small Mixer",
            new EquipmentItemInfo
            {
                cost = 60,
                prefab = Resources.Load<GameObject>("Prefabs/Equipment/mix_table"),
                description = "This is the first item description."
            }
        },
        {
            "Cheap PC Speakers",
            new EquipmentItemInfo
            {
                cost = 40,
                prefab = Resources.Load<GameObject>("Prefabs/Item2Prefab"),
                description = "This is the second item description."
            }
        },
        {
            "Portable Charging Station",
            new EquipmentItemInfo
            {
                cost = 60,
                prefab = Resources.Load<GameObject>("Prefabs/Item3Prefab"),
                description = "This is the third item description."
            }
        },
        // Add more items as needed
    };
}

// Class to store information about each equipment item
public class EquipmentItemInfo
{
    public int cost; // Cost of the item
    public GameObject prefab; // Prefab of the item
    public string description; // Description of the item
}