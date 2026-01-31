using UnityEngine;

public class PlayerMovementExperimental : MonoBehaviour
{
    public float playerSpeed = 10f;
    public float jumpPower = 10f;
    public Rigidbody2D rbody;

    private bool jumping = false;
    private float lastJump;
    private bool bufferedJump = false;
    private float bufferedJumpStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        lastJump = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        rbody.linearVelocityX = Input.GetAxis("Horizontal")*playerSpeed;
        if((Input.GetKeyDown("space") && CheckGround(true) && Time.fixedTime-lastJump>0.03f) || (bufferedJump && CheckGround(false) && Time.fixedTime - lastJump > 0.03f))
        {
            //Debug.Log("Jump!");

            /*
            RaycastHit2D lOffsetCheck = Physics2D.Raycast(transform.position + new Vector3(-0.35f, -0.3f, 0f), Vector2.down);
            RaycastHit2D rOffsetCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f, -0.3f, 0f), Vector2.down);
            try
            {
                if (lOffsetCheck.collider.gameObject.tag == "ground" && lOffsetCheck.distance < 0.1f)
                    Debug.Log("Wall to the left");
                    transform.position += new Vector3(0f, 0.1f, 0f);
            }
            catch { }
            try
            {
                if (rOffsetCheck.collider.gameObject.tag == "ground" && rOffsetCheck.distance < 0.1f)
                    Debug.Log("Wall to the right");
                transform.position -= new Vector3(0f, 0.1f, 0f);
            }
            catch { }*/

            bufferedJump = false;
            rbody.linearVelocityY = jumpPower;
            lastJump = Time.fixedTime;
            jumping = true;
        }

        //variable jump height
        if (jumping)
        {
            if(Input.GetKey("space") && rbody.linearVelocityY > 2f)
            {
                //Debug.Log("Extending jump.");
                rbody.gravityScale = 1f;
            }
            else
            {
                //Debug.Log("Back to reality, oh there goes gravity. "+Input.GetKey("space"));
                rbody.gravityScale = 2f;
                jumping= false;
            }
        }

        if (bufferedJump && Time.fixedTime - bufferedJumpStart > 0.2f)
        {
            bufferedJump = false;
            Debug.Log("Buffered jump expired.");
        }
    }

    bool CheckGround(bool manual)
    {
        float bonusOffset = 0.05f;

        RaycastHit2D lOffsetCheck = Physics2D.Raycast(transform.position + new Vector3(-0.35f, -0.3f, 0f), Vector2.down);
        RaycastHit2D rOffsetCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f, -0.3f, 0f), Vector2.down);
        try
        {
            if (lOffsetCheck.collider.gameObject.tag == "ground" && lOffsetCheck.distance < 0.1f)
                //Debug.Log("removing offset.");
                bonusOffset = 0f;
        } catch { }
        try
        {
            if (rOffsetCheck.collider.gameObject.tag == "ground" && rOffsetCheck.distance < 0.1f)
                //Debug.Log("removing offset.");
            bonusOffset = 0f;
        }
        catch { }


        RaycastHit2D leftCheck = Physics2D.Raycast(transform.position + new Vector3(-0.30f - bonusOffset, -0.61f, 0f), Vector2.down);
        RaycastHit2D rightCheck = Physics2D.Raycast(transform.position + new Vector3(0.30f + bonusOffset, -0.61f, 0f), Vector2.down);
        try
        {
            if (leftCheck.collider.gameObject.tag == "ground" && leftCheck.distance < 0.1f) return true;
            else if (leftCheck.collider.gameObject.tag == "ground" && leftCheck.distance < 0.4f && manual)
            {
                //Debug.Log("jump buffered.");
                bufferedJump = true;
                bufferedJumpStart = Time.fixedTime;
            }
        } catch { }
        try
        {
            if (rightCheck.collider.gameObject.tag == "ground" && rightCheck.distance < 0.1f) return true;
            else if (rightCheck.collider.gameObject.tag == "ground" && rightCheck.distance < 0.4f && manual)
            {
                //Debug.Log("jump buffered.");
                bufferedJump = true;
                bufferedJumpStart = Time.fixedTime;
            }
        } catch { }
        return false;
        
        //if (leftCheck.collider.gameObject.tag == "ground" || rightCheck.collider.gameObject.tag == "ground") return true;
        //return false;
    }


}
