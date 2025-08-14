using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    // References
    public GameObject platformPrefab;
    public Transform Player;
    public Camera mainCamera;

    // Platform with variance 
    public float minWidth = 1.0f;
    public float maxWidth = 5.0f;

    // Spawn distance
    public float spawnAheadDistance = 10.0f;

    // Intern
    private float lastSpawnX;
    private float lastSpawnY;
    private float lastPlatformWidth;
    private Queue<GameObject> platforms = new Queue<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the first spawn position to the player's position
        float firstWidth = Random.Range(minWidth, maxWidth);
        GameObject first = Instantiate(platformPrefab, Vector2.zero, Quaternion.identity, transform);
        first.transform.localScale = new Vector3(firstWidth, first.transform.localScale.y, 1f);

        platforms.Enqueue(first);
        lastSpawnX = 0f;
        lastSpawnY = 0f;
        lastPlatformWidth = firstWidth;

        for (int i = 0; i < 5; i++)
            SpawnNextPlatform();
    }


    // Update is called once per frame
    void Update()
    {
        // spawn new platforms on the right
        float cameraRightX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        while (lastSpawnX + lastPlatformWidth < cameraRightX + spawnAheadDistance)
        {
            SpawnNextPlatform();
        }

        // delete platforms on the left
        float cameraLeftX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        while (platforms.Count > 0 &&
               platforms.Peek().transform.position.x + (platforms.Peek().transform.localScale.x / 2f) < cameraLeftX)
        {
            Destroy(platforms.Dequeue());
        }
    }

    void SpawnNextPlatform()
    {
        // Random width for the new platform
        float width = Random.Range(minWidth, maxWidth);

        // Random horizontal gap from the last platform
        float gap = Random.Range(1f, 3f);

        // Random vertical difference from the last platform
        float dy = Random.Range(-2f, 2f);

        // Calculate new horizontal position
        float newX = lastSpawnX + lastPlatformWidth + gap;

        // Calculate vertical camera boundaries
        Vector2 camBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 camTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        float minY = camBottomLeft.y;
        float maxY = camTopRight.y;

        // Calculate new vertical position within camera bounds
        float newY = Mathf.Clamp(lastSpawnY + dy, minY, maxY);

        // Instantiate the platform at the new position and apply scale
        GameObject plat = Instantiate(platformPrefab, new Vector2(newX, newY), Quaternion.identity, transform);
        plat.transform.localScale = new Vector3(width, plat.transform.localScale.y, 1f);

        // Update last spawn position and width
        lastSpawnX = newX;
        lastSpawnY = newY;
        lastPlatformWidth = width;
        platforms.Enqueue(plat);
    }
}
