using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public Transform player;              // The frog
    public GameObject platformPrefab;     // Platform prefab
    public float verticalSpacing = 2f;    // Distance between platforms
    public int initialPlatforms = 10;     // Platforms to start with
    public float xPadding = 0.5f;         // Safe padding so platforms don't spawn offscreen

    private float highestY;               // Highest platform generated so far
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // Start by spawning a small stack
        highestY = player.position.y;
        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnNextPlatform();
        }
    }

    void Update()
    {
        // If the player gets within 6 units of the top, generate more
        if (player.position.y + 6f > highestY)
        {
            SpawnNextPlatform();
        }
    }

    void SpawnNextPlatform()
    {
        highestY += verticalSpacing;

        // Determine the horizontal bounds based on camera view
        float heightAtY = Mathf.Abs(cam.transform.position.z - highestY);
        float halfWidth = heightAtY * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * cam.aspect;

        float minX = cam.transform.position.x - halfWidth + xPadding;
        float maxX = cam.transform.position.x + halfWidth - xPadding;

        float xPos = Random.Range(minX, maxX);

        Vector3 spawnPos = new Vector3(xPos, highestY, 0);
        Instantiate(platformPrefab, spawnPos, Quaternion.identity);
    }
}
