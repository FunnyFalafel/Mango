using UnityEngine;

public class PlayerMovementExperimental : MonoBehaviour
{
    public float playerSpeed = 2f;
    public float jumpPower = 10f;
    public Rigidbody2D rigidbody;

    private bool grounded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.linearVelocityX = Input.GetAxis("Horizontal");

    }

    
}
