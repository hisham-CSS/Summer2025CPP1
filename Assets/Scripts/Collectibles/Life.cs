using UnityEngine;

public class Life : Pickup
{
    Rigidbody2D rb;

    private int xVel = -4;
    public override void OnPickup(GameObject player) => player.GetComponent<PlayerController>().lives++;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(xVel, 4); // Set the initial velocity to move downwards
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(xVel, rb.linearVelocityY);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Wall"))
        {
            xVel *= -1; // Reverse the horizontal velocity when hitting a wall
        }
    }
}
