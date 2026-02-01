using UnityEngine;

public class PlayerMovementExperimental : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    public float playerSpeed = 10f;
    public float jumpPower = 10f;
    public float worldOffset = 100f;
    public Rigidbody2D rbody;
    [SerializeField] private Animator anim;

    public float dashLen = 2f;
    public float dashDuration = .15f;
    public float dashCD = 1.5f;
    public GameObject cinemachineCamera;

    private bool jumping = false;
    private float lastJump;
    private bool bufferedJump = false;
    private float bufferedJumpStart;
    private bool inFuture = false;
    private float lastWarp;
    private Vector3 shadowPos;
    private Vector3 checkpoint;
    private bool dashEnabled = false;
    private float lastDash;
    private float dashSpeed;
    private bool midDash = false;
    private Vector2 storedVelocity;
    private Vector3 dashDir;
    private bool bufferCam = false;
    private float camBufferTime;
    float lastDir = 1f; // 1 = right, -1 = left
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lastJump = Time.fixedTime;
        lastWarp = Time.fixedTime;
        checkpoint = transform.position;
        lastDash = Time.fixedTime;
        dashSpeed = dashLen / dashDuration;
        camBufferTime = Time.fixedTime;
        
    }

    // Update is called once per frame
    void Update()
    {      
        bool groundedNow = CheckGround(false); // !!!false so it doesn't buffer jumps
        Vector2 v = rbody.linearVelocity;      // if linearVelocity gives issues, use rbody.velocity

        float h = Input.GetAxisRaw("Horizontal");  

        anim.SetFloat("YVel", v.y);
        anim.SetBool("Grounded", groundedNow);
        anim.SetFloat("Speed", groundedNow ? Mathf.Abs(h) : 0f);


        //Debug.Log(FindObjectsOfType<Collider2D>().Length);

        //Cam jank
        if (bufferCam && Time.fixedTime-camBufferTime>.025f)
        {
            cinemachineCamera.SetActive(true);
        }

        //Dash stuff
        if (midDash)
        {
            if (Time.fixedTime - lastDash < dashDuration)
            {
                //Debug.Log(Time.fixedTime - lastDash);
                rbody.linearVelocity = dashDir * dashSpeed;
                return;
            }
            else
            {
                //Debug.Log("Dash is over.");
                midDash = false;
                //rbody.linearVelocity = storedVelocity;
            }
        }

        if (h > 0.01f)
        {
            sr.flipX = true; // facing right
            lastDir = -1f;
        }
        else if (h < -0.01f)
        {
            sr.flipX = false;  // facing left
            lastDir = 1f;
        }
        // else: do nothing -> keeps last direction while idle


        //rbody.linearVelocityX = h*playerSpeed;

        v.x = h*playerSpeed;
        rbody.linearVelocity = v;

        if((Input.GetKeyDown("space") && CheckGround(true) && Time.fixedTime-lastJump>0.03f) || (bufferedJump && CheckGround(false) && Time.fixedTime - lastJump > 0.03f))
        {
            //Debug.Log("Jump!");
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

        // replaced with mouse tracking
        /*
        if(Input.GetKeyDown(KeyCode.J) && Time.fixedTime - lastWarp > 0.4f)
        {
            Debug.Log("Attempting to warp.");
            Warp();
        } */

        if(Input.GetKeyDown(KeyCode.K) && dashEnabled && Time.fixedTime - lastDash > dashCD)
        {
            //Debug.Log("Dash started! time sice last dash: "+(Time.fixedTime - lastDash));
            //storedVelocity = rbody.linearVelocity;
            midDash = true;
            lastDash = Time.fixedTime;
            dashDir = shadowPos - transform.position;
            if (inFuture) dashDir += Vector3.right * worldOffset;
            dashDir = dashDir/dashDir.magnitude;
        }
    }


    bool CheckGround(bool manual)
    {
        float bonusOffset = 0.05f;
        Debug.DrawRay(transform.position + new Vector3(-0.30f - bonusOffset, -0.61f, 0f), Vector2.down * 0.6f, Color.red);
        Debug.DrawRay(transform.position + new Vector3( 0.30f + bonusOffset, -0.61f, 0f), Vector2.down * 0.6f, Color.red);

        RaycastHit2D lOffsetCheck = Physics2D.Raycast(transform.position + new Vector3(-0.19f, -0.3f, 0f), Vector2.down);
        RaycastHit2D rOffsetCheck = Physics2D.Raycast(transform.position + new Vector3(0.19f, -0.3f, 0f), Vector2.down);
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


        RaycastHit2D leftCheck = Physics2D.Raycast(transform.position + new Vector3(-0.18f - bonusOffset, -0.37f, 0f), Vector2.down);
        RaycastHit2D rightCheck = Physics2D.Raycast(transform.position + new Vector3(0.12f + bonusOffset, -0.37f, 0f), Vector2.down);
        try
        {
            if (leftCheck.collider.gameObject.tag == "ground" && leftCheck.distance < 0.2f) return true;
            else if (leftCheck.collider.gameObject.tag == "ground" && leftCheck.distance < 0.45f && manual)
            {
                //Debug.Log("jump buffered.");
                bufferedJump = true;
                bufferedJumpStart = Time.fixedTime;
            }
        } catch { }
        try
        {
            if (rightCheck.collider.gameObject.tag == "ground" && rightCheck.distance < 0.2f) return true;
            else if (rightCheck.collider.gameObject.tag == "ground" && rightCheck.distance < 0.45f && manual)
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

    public void Warp()
    {
        Debug.Log("Shifted worlds.");
        lastWarp = Time.fixedTime;
        if(inFuture) {
            cinemachineCamera.SetActive(false);
            transform.position -= new Vector3(worldOffset, 0f, 0f);
            bufferCam = true;
            camBufferTime=Time.fixedTime;
            inFuture = false;
        }
        else
        {
            cinemachineCamera.SetActive(false);
            transform.position += new Vector3(worldOffset, 0f, 0f);
            bufferCam = true;
            camBufferTime = Time.fixedTime;
            inFuture = true;
        }
    }

    public void SetCheckpoint(Vector3 spot)
    {
        Debug.Log("Changing checkpoint.");
        checkpoint = spot;
        transform.position = spot;
    }

    public void Die()
    {
        transform.position = checkpoint;
        if (checkpoint.x > worldOffset / 2f) inFuture = true;
        else inFuture = false;
    }

    public bool AddShadow(Vector3 position)
    {
        if(dashEnabled)
        {
            Debug.LogError("Trying to add shadow to a player that already has one.");
            return false;
        }
        shadowPos = position;
        dashEnabled = true;
        return true;
    }

    public void RemoveShadow()
    {
        shadowPos = new Vector3();
        dashEnabled = false;
    }

    
}
