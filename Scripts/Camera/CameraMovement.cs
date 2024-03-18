using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera mainCamera; // Reference to the Camera component

    public float moveSpeed = 10f;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 5f;

    void Start()
    {
        // Get the Camera component attached to the same GameObject
        mainCamera = GetComponent<Camera>();
        
        // Check if the Camera component was found
        if (mainCamera == null)
        {
            Debug.LogError("Camera component not found on the GameObject!");
        }
    }

    void Update()
    {
        // Camera movement
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // Check for key presses for arrow keys
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput = -1f;
        }

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Camera zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newSize = mainCamera.orthographicSize - scrollInput * zoomSpeed;
        newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        mainCamera.orthographicSize = newSize;
    }
}
