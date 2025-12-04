using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int score = 0;

    void Awake()
    {
        
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPoints(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        //update UI HERE

    }

    public int GetScore()
    {
        return score;
    }
}
