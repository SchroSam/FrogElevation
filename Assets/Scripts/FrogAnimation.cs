using UnityEngine;

public class FrogAnimation : MonoBehaviour
{
    public Sprite sittingSprite;
    public Sprite jumpingSprite;

    private SpriteRenderer sr;
    private bool isGrounded = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sittingSprite;
    }

    void Update()
    {
        // Change sprite depending on grounded state
        if (isGrounded)
        {
            sr.sprite = sittingSprite;
        }
        else
        {
            sr.sprite = jumpingSprite;
        }
    }

    // Detect ground/platforms
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }
}
