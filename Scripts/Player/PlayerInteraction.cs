using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    // Reference to the Canvas
    public Canvas canvas;
    
    // Dictionary to store objects and their associated text windows
    public Dictionary<GameObject, GameObject> objectTextWindows = new Dictionary<GameObject, GameObject>();

    // Prefab for the text window
    public GameObject textWindowPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the one you want to interact with
        if (other.CompareTag("Interactable"))
        {
            // Add the object to the dictionary with its associated text window
            GameObject textWindow = InstantiateTextWindow(other.gameObject);
            objectTextWindows.Add(other.gameObject, textWindow);

            // Perform interaction with the object
            Debug.Log("Interacting with object: " + other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collided object is in the dictionary
        if (objectTextWindows.ContainsKey(other.gameObject))
        {
            // Destroy the associated text window
            Destroy(objectTextWindows[other.gameObject]);
            
            // Remove the object from the dictionary
            objectTextWindows.Remove(other.gameObject);

            // Perform any other actions you need when an object exits the trigger
        }
    }

    // Instantiate the text window prefab as a child of the Canvas and set its position
    private GameObject InstantiateTextWindow(GameObject targetObject)
    {
        GameObject textWindow = Instantiate(textWindowPrefab, canvas.transform);
        textWindow.transform.position = targetObject.transform.position;
        TextMeshProUGUI textMesh = textWindow.GetComponent<TextMeshProUGUI>();
        textMesh.text = targetObject.name;
        return textWindow;
    }
}
