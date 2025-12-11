using UnityEngine;
using System.Collections.Generic;

public class PlatformGeneratorGrid : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    //public int rows = 20;                 // how many vertical rows
    public int cols = 5;                  // how many possible x slots per row
    public float xSpacing = 3f;           // horizontal distance between slots
    //public float ySpacing = 2f;           // vertical distance between rows

    //[Header("Spawning Rules")]
    //public int minPlatformsPerRow = 1;
    //public int maxPlatformsPerRow = 2;

    [Header("Position Offset")]
    public float startY = 0f;             // starting vertical position
    public float startX = -6f;            // where slot 0 begins
    public float maxHeight;
    public float minDistance = 1.5f;
    public float frogOffset = 15;

    [Header("Powerups")]
    public GameObject power1;
    public GameObject power2;
    public GameObject power3;
    private List<GameObject> powerUps;
    public int powerChance = 4;

    //private List<int> lastRowColumns = new List<int>();  // columns used in previous row
    private List<Vector3> allPlatformPositions = new List<Vector3>();  // all platforms ever spawned

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Frog");
        powerUps = new List<GameObject>{power1, power2, power3};
        GeneratePlatforms();
    }

    // public void GeneratePlatformsOld()
    // {
    //     for (int row = 0; row < rows; row++)
    //     {
    //         int platformsToSpawn = Random.Range(minPlatformsPerRow, maxPlatformsPerRow + 1);

    //         // Create a list of available columns excluding last row
    //         List<int> availableCols = new List<int>();
    //         for (int c = 0; c < cols; c++)
    //         {
    //             if (!lastRowColumns.Contains(c))
    //                 availableCols.Add(c);
    //         }

    //         // Safety check: if availableCols < platformsToSpawn, adjust
    //         platformsToSpawn = Mathf.Min(platformsToSpawn, availableCols.Count);

    //         // Track columns used this row
    //         List<int> currentRowColumns = new List<int>();

    //         float nextYPos = 0;

    //         for (int i = 0; i < platformsToSpawn; i++)
    //         {
    //             int chosenIndex = Random.Range(0, availableCols.Count);
    //             int col = availableCols[chosenIndex];
    //             availableCols.RemoveAt(chosenIndex);

    //             currentRowColumns.Add(col);

    //             float xPos = startX + (col * xSpacing);
    //             float yPos = startY + (row * ySpacing);

    //             Instantiate(platformPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);

    //             if(nextYPos < yPos)
    //                 nextYPos = yPos;
    //         }

    //         startY = nextYPos;

    //         // Remember columns for next row
    //         lastRowColumns = currentRowColumns;
    //     }
    // }

    public void GeneratePlatforms()
    {
        // float minDistance = 1.5f; // Minimum distance between platforms
        
        // Generate platforms in groups of 5
        for (int i = 0; i < 5; i++)
        {
            float xPos = 0;
            float yPos = 0;
            bool validPosition = false;
            int attempts = 0;
            int maxAttempts = 50;
            
            // Try to find a valid position that doesn't overlap with existing platforms
            while (!validPosition && attempts < maxAttempts)
            {
                // Random x position within available slots with larger offset
                int randomCol = Random.Range(0, cols);
                xPos = startX + (randomCol * xSpacing) + Random.Range(-1.2f, 1.2f);
                
                // Smaller y spacing for tighter vertical clustering
                float yOffset = Random.Range(-0.2f, 0.2f);
                yPos = startY + (i * 0.8f) + yOffset;
                
                // Check if this position is valid (not too close to any platform ever spawned)
                validPosition = true;
                for (int j = allPlatformPositions.Count - 1; j >= 0; j--)
                {
                    float distance = Vector3.Distance(new Vector3(xPos, yPos, 0), allPlatformPositions[j]);
                    if (distance < minDistance)
                    {
                        Debug.Log($"Rerolling, attempts: {attempts}");
                        validPosition = false;
                        break;
                    }
                }
                
                attempts++;
            }
            
            // Instantiate the platform
            Vector3 platformPos = new Vector3(xPos, yPos, 0);
            Instantiate(platformPrefab, platformPos, Quaternion.identity);
            allPlatformPositions.Add(platformPos);

            // only remembers (and only checks) the 15 most recent platforms
            if(allPlatformPositions.Count > 15)
                allPlatformPositions.RemoveAt(0);

            // random powerups gen
            if(Random.Range(1, powerChance + 1) == 1) // 1 in {powerChance} chance of spawning any powerup at all
            {
                int powerUp = Random.Range(0, 3);
                Vector3 powerUpPos = new Vector3(platformPos.x + Random.Range(-0.8f, 0.8f), platformPos.y + Random.Range(-0.8f, 0.8f), 0);
                Instantiate(powerUps[powerUp], powerUpPos, Quaternion.identity);
            }
            
            // Update startY to the highest platform created
            if (yPos > startY)
                startY = yPos;
        }
    }

    public void FixedUpdate()
    {
        if(player.transform.position.y + frogOffset >= startY && player.transform.position.y < maxHeight - frogOffset)
        {
            Debug.Log($"startY: {startY}");
            GeneratePlatforms();
        }
    }
}
