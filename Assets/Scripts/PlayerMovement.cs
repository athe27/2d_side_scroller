using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 6.25f;
    public float acceleration = 1f;
    public float velPower = 0.9f;
    public float frictionAmount = 5f;
    public float jumpForce = 10f;
    public Vector2 groundedSize = new Vector2(1f, 0.5f);
    public LayerMask groundMask;

    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        calculateMovement(horizontal);
        calculateFriction(horizontal);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            jump();
        }
    }

    private void jump()
    {
        if (isGrounded())
        {
            Debug.Log("Jumping");
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void calculateMovement(float horizontal)
    {
        float targetVelocity = horizontal * walkSpeed;
        float currentVelocity = rigidbody2D.velocity.x;
        if (targetVelocity == 0f || (currentVelocity * targetVelocity > 0 && Mathf.Abs(currentVelocity) > Mathf.Abs(targetVelocity)))
        {
            return; // apply 0 force
        }
        float speedDif = targetVelocity - currentVelocity;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * acceleration, velPower) * Mathf.Sign(speedDif);
        rigidbody2D.AddForce(movement * Vector2.right);
    }

    private void calculateFriction(float horizontal)
    {
        if (horizontal == 0f && isGrounded())
        {
            float velocity = rigidbody2D.velocity.x;
            float amount = Mathf.Sign(velocity) * Mathf.Min(Mathf.Abs(velocity), frictionAmount);
            rigidbody2D.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    private bool isGrounded()
    {
        if (rigidbody2D.velocity.y != 0f) return false;

        Bounds bounds = boxCollider2D.bounds;
        Vector2 extents = bounds.extents;
        Vector2 center = (Vector2) bounds.center - Vector2.up * extents.y - Vector2.Scale(groundedSize, Vector2.up / 2);
        bool box = Physics2D.OverlapBox(center, groundedSize, 0, groundMask);
        return box;
    }

    void drawBox(Vector2 center, Vector2 size)
    {
        // Calculate the top-left and bottom-right corners of the OverlapBox
        Vector2 topLeft = center - size / 2;
        Vector2 bottomRight = center + size / 2;

        // Draw the lines that make up the edges of the OverlapBox
        Debug.DrawLine(new Vector3(topLeft.x, topLeft.y, 1), new Vector3(topLeft.x, bottomRight.y, 1), Color.green);
        Debug.DrawLine(new Vector3(topLeft.x, bottomRight.y, 1), new Vector3(bottomRight.x, bottomRight.y, 1), Color.green);
        Debug.DrawLine(new Vector3(bottomRight.x, bottomRight.y, 1), new Vector3(bottomRight.x, topLeft.y, 1), Color.green);
        Debug.DrawLine(new Vector3(bottomRight.x, topLeft.y, 1), new Vector3(topLeft.x, topLeft.y, 1), Color.green);
    }
}