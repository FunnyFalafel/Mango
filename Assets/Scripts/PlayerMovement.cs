using UnityEngine;

public class PlayerMovementexperimental : MonoBehaviour
{
    public float playerSpeed = 6f;
    public float jumpPower = 5f;
    public Rigidbody2D rbody;

    float moveInput;
    bool jumpPressed;

    bool jumping = false;
    float lastJump;
    bool bufferedJump = false;
    float bufferedJumpStart;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        lastJump = Time.fixedTime;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;
    }

    void FixedUpdate()
    {
        // horizontal
        rbody.linearVelocity = new Vector2(moveInput * playerSpeed, rbody.linearVelocity.y);

        // jump (use the buffered logic you already have)
        bool canJumpCooldown = (Time.fixedTime - lastJump) > 0.03f;

        if (((jumpPressed && CheckGround(true)) || (bufferedJump && CheckGround(false))) && canJumpCooldown)
        {
            bufferedJump = false;
            rbody.linearVelocity = new Vector2(rbody.linearVelocity.x, jumpPower);
            lastJump = Time.fixedTime;
            jumping = true;
        }

        jumpPressed = false;

        // variable jump height
        if (jumping)
        {
            if (Input.GetKey(KeyCode.Space) && rbody.linearVelocity.y > 2f)
                rbody.gravityScale = 1f;
            else
            {
                rbody.gravityScale = 2f;
                jumping = false;
            }
        }

        // buffered jump expiry
        if (bufferedJump && Time.fixedTime - bufferedJumpStart > 0.2f)
            bufferedJump = false;
    }

    bool CheckGround(bool manual)
    {
        float bonusOffset = 0.05f;

        RaycastHit2D lOffsetCheck = Physics2D.Raycast(transform.position + new Vector3(-0.35f, -0.3f, 0f), Vector2.down);
        RaycastHit2D rOffsetCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f, -0.3f, 0f), Vector2.down);

        if (lOffsetCheck.collider != null && lOffsetCheck.collider.CompareTag("ground") && lOffsetCheck.distance < 0.1f)
            bonusOffset = 0f;
        if (rOffsetCheck.collider != null && rOffsetCheck.collider.CompareTag("ground") && rOffsetCheck.distance < 0.1f)
            bonusOffset = 0f;

        RaycastHit2D leftCheck  = Physics2D.Raycast(transform.position + new Vector3(-0.30f - bonusOffset, -0.61f, 0f), Vector2.down);
        RaycastHit2D rightCheck = Physics2D.Raycast(transform.position + new Vector3( 0.30f + bonusOffset, -0.61f, 0f), Vector2.down);

        if (leftCheck.collider != null && leftCheck.collider.CompareTag("ground"))
        {
            if (leftCheck.distance < 0.1f) return true;
            if (leftCheck.distance < 0.4f && manual)
            {
                bufferedJump = true;
                bufferedJumpStart = Time.fixedTime;
            }
        }

        if (rightCheck.collider != null && rightCheck.collider.CompareTag("ground"))
        {
            if (rightCheck.distance < 0.1f) return true;
            if (rightCheck.distance < 0.4f && manual)
            {
                bufferedJump = true;
                bufferedJumpStart = Time.fixedTime;
            }
        }

        return false;
    }
}

