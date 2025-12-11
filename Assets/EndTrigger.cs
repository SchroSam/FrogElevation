using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject winScreen;

    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        winScreen.SetActive(true);
    }
}
