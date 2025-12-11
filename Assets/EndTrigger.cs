using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject winScreen;
    private bool won = false;

    void Start()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(!won && FindFirstObjectByType<FrogAnimation>().isGrounded)
        {
            winScreen.SetActive(true);
            won = true;
            Time.timeScale = 0f;
        }
    }
}
