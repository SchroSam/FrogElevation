using UnityEngine;

public class FrogAnimation : MonoBehaviour
{
    public Sprite sittingSprite;
    public Sprite jumpingSprite;

    public AudioClip jumpSound;
    private AudioSource audioSource;

    private SpriteRenderer sr;
    public bool isGrounded = true;
    private Rigidbody2D rb;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        sr.sprite = sittingSprite;
    }

    void Update()
    {
        // If frog just jumped (velocity > 0 AND was grounded)
        if (isGrounded && rb.linearVelocity.y > 0.1f)
        {
            // Play jump sound ONCE
            if (jumpSound != null)
                audioSource.PlayOneShot(jumpSound);

            isGrounded = false;
        }

        // Update sprite
        if (isGrounded)
            sr.sprite = sittingSprite;
        else
            sr.sprite = jumpingSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }
}
