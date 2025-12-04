using UnityEngine;
using System.Collections.Generic;

public class PlatformGeneratorGrid : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public int rows = 20;                 // how many vertical rows
    public int cols = 5;                  // how many possible x slots per row
    public float xSpacing = 3f;           // horizontal distance between slots
    public float ySpacing = 2f;           // vertical distance between rows

    [Header("Spawning Rules")]
    public int minPlatformsPerRow = 1;
    public int maxPlatformsPerRow = 2;

    [Header("Position Offset")]
    public float startY = 0f;             // starting vertical position
    public float startX = -6f;            // where slot 0 begins

    private List<int> lastRowColumns = new List<int>();  // columns used in previous row

    void Start()
    {
        GeneratePlatforms();
    }

    void GeneratePlatforms()
    {
        for (int row = 0; row < rows; row++)
        {
            int platformsToSpawn = Random.Range(minPlatformsPerRow, maxPlatformsPerRow + 1);

            // Create a list of available columns excluding last row
            List<int> availableCols = new List<int>();
            for (int c = 0; c < cols; c++)
            {
                if (!lastRowColumns.Contains(c))
                    availableCols.Add(c);
            }

            // Safety check: if availableCols < platformsToSpawn, adjust
            platformsToSpawn = Mathf.Min(platformsToSpawn, availableCols.Count);

            // Track columns used this row
            List<int> currentRowColumns = new List<int>();

            for (int i = 0; i < platformsToSpawn; i++)
            {
                int chosenIndex = Random.Range(0, availableCols.Count);
                int col = availableCols[chosenIndex];
                availableCols.RemoveAt(chosenIndex);

                currentRowColumns.Add(col);

                float xPos = startX + (col * xSpacing);
                float yPos = startY + (row * ySpacing);

                Instantiate(platformPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }

            // Remember columns for next row
            lastRowColumns = currentRowColumns;
        }
    }
}
