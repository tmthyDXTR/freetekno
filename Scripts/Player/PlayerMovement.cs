using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    
    void Start()
    {
        // Get the Rigidbody2D component attached to the player GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // Check for key presses for WASD keys
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        // Calculate the movement direction based on input
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        // Apply force to the Rigidbody based on the movement direction and speed
        rb.velocity = movement * moveSpeed;
    }
}
