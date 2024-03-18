using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SetupUIManager : MonoBehaviour
{
    // Reference to the player movement script
    public PlayerMovement playerMovementScript;

    // Reference to the camera movement script
    public CameraMovement cameraMovementScript;
    // Reference to the setup panel
    public GameObject setupPanel;

    // Reference to the prefab for the text item
    public GameObject textItemPrefab;

    // Reference to the budget display text
    public TextMeshProUGUI budgetText;

    // Remaining budget
    public int remainingBudget = 500;

    // List of text items
    private List<GameObject> textItems = new List<GameObject>();

    // Index of currently selected text item
    private int selectedIndex = -1;

    // Reference to the currently instantiated item
    private GameObject currentSelectedEquipment;

    void Start()
    {
        ToggleMovementScripts(false);
        UpdateBudgetDisplay();

        // Dynamically create text items for each equipment item
        foreach (KeyValuePair<string, EquipmentItemInfo> entry in EquipmentInfo.items)
        {
            CreateTextItem(entry.Key, entry.Value.cost);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if setup panel is active
        if (setupPanel.activeSelf)
        {
            // Move selection up
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Deselect current selection
                if (selectedIndex >= 0)
                    DeselectItem(selectedIndex);

                selectedIndex = Mathf.Max(0, selectedIndex - 1);
                SelectItem(selectedIndex);
            }

            // Move selection down
            if (Input.GetKeyDown(KeyCode.S))
            {
                // Deselect current selection
                if (selectedIndex >= 0)
                    DeselectItem(selectedIndex);

                selectedIndex = Mathf.Min(textItems.Count - 1, selectedIndex + 1);
                SelectItem(selectedIndex);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlaceItem();
            }
        }
    }

    // Method to select a text item
    private void SelectItem(int index)
    {
        if (index >= 0 && index < textItems.Count)
        {
            // Change text color
            textItems[index].GetComponent<TextMeshProUGUI>().color = Color.green;

            // Get the ItemInfo component of the selected item
            ItemInfo itemInfo = textItems[index].GetComponent<ItemInfo>();

            // Check if ItemInfo component exists
            if (itemInfo != null)
            {
                // Call OnEquipmentSelected with the selected item's name and cost
                string equipmentName = itemInfo.itemName;
                int cost = itemInfo.itemCost;
                OnEquipmentSelected(equipmentName, cost);
            }
        }
    }


    // Method to deselect a text item
    private void DeselectItem(int index)
    {
        if (index >= 0 && index < textItems.Count)
        {
            // Restore default text color
            textItems[index].GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    // Method to create a text item for an equipment item
    private void CreateTextItem(string equipmentName, int cost)
    {
        // Find the SetupPanel under the SetupCanvas
        Transform setupPanel = transform.Find("SetupPanel");

        // Check if setupPanel is found
        if (setupPanel == null)
        {
            Debug.LogError("SetupPanel not found!");
            return;
        }

        // Instantiate text item under the SetupPanel
        GameObject textItem = Instantiate(textItemPrefab, setupPanel);

        // Check if textItem is instantiated
        if (textItem == null)
        {
            Debug.LogError("Text item not instantiated!");
            return;
        }

        // Set the text and button click event as before
        TextMeshProUGUI itemText = textItem.GetComponent<TextMeshProUGUI>();
        itemText.text = $"{equipmentName} - ${cost}";

        // Add a script with specific item info
        ItemInfo itemInfo = textItem.AddComponent<ItemInfo>();
        itemInfo.InitializeInfo(equipmentName, cost);

        // Adjust position of text item to create a vertical list
        RectTransform textItemRect = textItem.GetComponent<RectTransform>();
        textItemRect.anchoredPosition = new Vector2(0, -textItemRect.sizeDelta.y * setupPanel.childCount + 200);

        // Add the text item to the list
        textItems.Add(textItem);

    }



    // Method to handle equipment selection
    public void OnEquipmentSelected(string equipmentName, int cost)
    {
        if (remainingBudget >= cost)
        {
            // UpdateBudgetDisplay();

            // Get the prefab associated with the selected item
            GameObject itemPrefab = EquipmentInfo.items[equipmentName].prefab;

            // Destroy the currently instantiated item if it exists
            if (currentSelectedEquipment != null)
            {
                Destroy(currentSelectedEquipment);
            }

            // Instantiate the new prefab on your grid
            if (itemPrefab != null)
            {
                Vector3 spawnPosition = new Vector3(0, 0, 0); // Define a method to determine spawn position on your grid
                currentSelectedEquipment = Instantiate(itemPrefab, spawnPosition, Quaternion.identity, GameObject.Find("Equipment").transform);

                // Add ItemInfo component to the instantiated object
                ItemInfo itemInfo = currentSelectedEquipment.AddComponent<ItemInfo>();
                itemInfo.InitializeInfo(equipmentName, cost);

                // Deactivate the Rigidbody component
                Rigidbody2D itemRigidbody = currentSelectedEquipment.GetComponent<Rigidbody2D>();
                if (itemRigidbody != null)
                {
                    itemRigidbody.simulated = false;
                }
                else
                {
                    Debug.LogWarning($"Rigidbody component not found on {equipmentName} prefab!");
                }
            }
            else
            {
                Debug.LogError($"Prefab for {equipmentName} not found!");
            }
        }
        else
        {
            Debug.Log("Not enough budget to purchase equipment!");
        }
    }

    // Method to update the budget display text
    private void UpdateBudgetDisplay()
    {
        budgetText.text = "Budget: $" + remainingBudget.ToString();
    }

    // Method to place the selected item
    private void PlaceItem()
    {
        if (currentSelectedEquipment != null)
        {
            // Get the item info of the selected item
            ItemInfo itemInfo = currentSelectedEquipment.GetComponent<ItemInfo>();
            if (itemInfo != null)
            {
                string equipmentName = itemInfo.itemName;
                int cost = itemInfo.itemCost;

                // Check if the player has enough budget
                if (remainingBudget >= cost)
                {
                    // Deduct the cost from the remaining budget
                    remainingBudget -= cost;
                    UpdateBudgetDisplay();

                    // Log the purchase
                    Debug.Log($"Purchased {equipmentName} for ${cost}");

                    // Set currentSelectedEquipment to null
                    currentSelectedEquipment = null;

                }
                else
                {
                    Debug.Log("Not enough budget to purchase equipment!");
                }
            }
            else
            {
                Debug.LogWarning("ItemInfo component not found on selected item!");
            }
        }
        else
        {
            Debug.LogWarning("No item selected to place!");
        }
    }

    // Method to toggle player and camera movement scripts
    public void ToggleMovementScripts(bool enableScripts)
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = enableScripts;

        if (cameraMovementScript != null)
            cameraMovementScript.enabled = enableScripts;
    }

    // Method to handle opening the setup panel
    public void OpenSetupPanel()
    {
        setupPanel.SetActive(true);
        ToggleMovementScripts(false); // Disable scripts when setup panel is active
    }

    // Method to handle closing the setup panel
    public void CloseSetupPanel()
    {
        setupPanel.SetActive(false);
        ToggleMovementScripts(true); // Re-enable scripts when setup panel is closed
    }
}
