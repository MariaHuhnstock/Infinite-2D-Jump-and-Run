using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variables 
    public float MovementSpeed = 5f;
    public float JumpForce = 5f; 

    // Reference to the Rigidbody2D
    private Rigidbody2D rb;

    public Camera mainCamera;

    // Called once when the object is initialized
    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    // Called once per frame
    void Update()
    {
        // Get horizontal input (A/D keys or left/right arrows)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Read the current velocity to preserve the vertical movement
        Vector2 currentVelocity = rb.linearVelocity;

        // Set the new velocity: apply horizontal input, keep current vertical speed
        rb.linearVelocity = new Vector2(horizontalInput * MovementSpeed, currentVelocity.y);

        // Check if the player is almost not moving vertically (basic ground check)
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            // Apply a jump by setting the vertical velocity to the jump force
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }

        // Game Over check: did the player fall below the camera view?
        Vector3 screenBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        if (transform.position.y < screenBottomLeft.y - 1f)
        {
            Debug.Log("Game Over");
        }
    }

}
