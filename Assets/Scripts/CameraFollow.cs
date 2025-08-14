using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Reference to the player's Transform 
    public Transform player;

    // minimum and maximum positions for the camera
    private float xMin;
    public float minY = -5f;
    public float maxY = 5f;

    // Called once at the beginning when the script starts
    void Start()
    {
        // Set the left boundary to the camera's starting X position
        // This prevents the camera from moving left beyond this point
        xMin = transform.position.x;
    }

    // Called after all Update() calls in the same frame
    // Used here to ensure the camera updates after the player has moved
    void LateUpdate()
    {
        // Store the current X position of the camera
        float camX = transform.position.x;

        // Only follow the player if they move to the right (forward)
        // This prevents the camera from moving backward
        if (player.position.x > camX)
        {
            camX = player.position.x;
        }

        // Make sure the camera never moves left beyond the initial xMin boundary
        camX = Mathf.Max(camX, xMin);

        // Apply the new camera position
        // Update both X and Y based on player position
        float camY = Mathf.Clamp(player.position.y, minY, maxY);

        // apply new camera position
        transform.position = new Vector3(camX, camY, transform.position.z);
    }
}
