using UnityEngine;

[System.Serializable]
public enum PowerType { Launch, Points, PointMult }

public class PowerUp : MonoBehaviour
{
    public PowerType powerType;
    public float launchForce = 100f;
    public float pointVal = 5f;

    // ----------- Movement variables -----------
    public bool moveAround = false;           // Turn on/off movement
    public float moveSpeed = 2f;              // How fast the bug moves
    public float changeDirectionTime = 1f;    // Seconds before changing direction
    public Vector2 moveBounds = new Vector2(5f, 5f); // Optional movement bounds

    private Vector2 moveDirection;
    private float moveTimer = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        if (moveAround)
            ChooseNewDirection();
    }

    void Update()
    {
        if (moveAround)
            HandleMovement();
    }

    private void HandleMovement()
    {
        // Move the bug
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Keep it within bounds
        Vector3 offset = transform.position - startPos;
        if (Mathf.Abs(offset.x) > moveBounds.x) moveDirection.x *= -1;
        if (Mathf.Abs(offset.y) > moveBounds.y) moveDirection.y *= -1;

        // Change direction occasionally
        moveTimer += Time.deltaTime;
        if (moveTimer >= changeDirectionTime)
        {
            ChooseNewDirection();
            moveTimer = 0f;
        }
    }

    private void ChooseNewDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Tongue")
        {
            switch (powerType)
            {
                case PowerType.Launch:
                    collision.transform.parent.GetComponent<Rigidbody2D>().AddForceY(launchForce);
                    break;

                case PowerType.Points:
                    FindFirstObjectByType<PlayerController>().AddPoints(pointVal);
                    break;

                case PowerType.PointMult:
                    FindFirstObjectByType<PlayerController>().PointMult();
                    break;
            }

            collision.transform.parent.GetComponent<PlayerController>().GrabbedItem();
            Destroy(gameObject);
        }
    }
}
