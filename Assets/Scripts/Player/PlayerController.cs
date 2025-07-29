using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // this transform is used to check if the player is grounded only needed if the ground check position is generated and a seperate object
    //private Transform groundCheckPos;


    //[SerializeField] private bool isGrounded = false;
    private LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;
    private GroundCheck groundCheck;

    [SerializeField] private int maxJumpCount = 2; // Maximum number of jumps allowed (e.g., double jump)
    private int jumpCount = 1;

    [SerializeField] private float groundCheckRadius = 0.02f; // Radius for ground check, adjust as necessary
    private float initialGroundCheckRadius;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("Ground");

        if (groundLayer == 0)
        {
            Debug.LogWarning("Ground layer not set. Please set the Ground layer in the LayerMask. Groundcheck not created");
            return;
        }
        groundCheck = new GroundCheck(col, groundLayer, groundCheckRadius);
        initialGroundCheckRadius = groundCheckRadius;

        // Initialize ground check position if using a separate GameObject for ground checking
        //GameObject newObj = new GameObject("GroundCheck");
        //newObj.transform.SetParent(transform);
        //newObj.transform.localPosition = Vector3.zero; // Set to the player's position
        //groundCheckPos = newObj.transform;

    }

    // Update is called once per frame
    void Update()
    {
        float hValue = Input.GetAxisRaw("Horizontal");
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
        SpriteFlip(hValue);
        //Debug.Log("Ground Check Position: " + groundCheckPos);

        rb.linearVelocityX = hValue * 5f; // Adjust speed as necessary
        groundCheck.CheckIsGrounded();

        if (!currentState.IsName("Fire") && Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Fire");
        }
        if (currentState.IsName("Fire"))
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse); // Adjust jump force as necessary
            jumpCount++;
            //Debug.Log("Jump Count: " + jumpCount);
        }

        if (groundCheck.IsGrounded)
            jumpCount = 1; // Reset jump count when grounded

        // Update animator parameters
        anim.SetFloat("hValue", Mathf.Abs(hValue));
        anim.SetBool("isGrounded", groundCheck.IsGrounded);
        //Debug.Log($"Ground Check Radius from Player object: {groundCheckRadius}");
        if (initialGroundCheckRadius != groundCheckRadius)
            groundCheck.UpdateGroundCheckRadius(groundCheckRadius);
    }

    void SpriteFlip(float hValue)
    {
        //if (hValue < 0)
        //    sr.flipX = true;
        //else if (hValue > 0)
        //    sr.flipX = false;
        if (hValue != 0) sr.flipX = (hValue < 0); // Simplified sprite flipping logic

        //if ((hValue > 0 && sr.flipX) || (hValue < 0 && !sr.flipX))
        //    sr.flipX = !sr.flipX; // Flip sprite only when direction changes
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.name} detected. Grounded: {groundCheck.IsGrounded}");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
