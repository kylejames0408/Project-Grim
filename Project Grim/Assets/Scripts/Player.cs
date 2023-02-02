using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Unity GetComponents 
    Rigidbody2D rb;
    Animator animate;

    // Basic movement and player flipping
    float movementDir;
    float playerSpeed = 3f;
    float jumpHeight = 5f;
    float flipSprite;

    // Ability Variables
    bool grounded = true;
    float dashCooldown = 3f;

    // SerializedFields to input the ground layer as well as the player's feet position
    [SerializeField] Transform groundPoint;
    [SerializeField] LayerMask groundLayer;

    void Start()
    {
        // Set the sprite's direction as well as get the rigidbody and animator
        flipSprite = transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the movement axis and set it to the velocity. 
        // AddForce for this case makes the player slide, not what is intended.
        movementDir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(movementDir * playerSpeed, rb.velocity.y);
        //rb.AddForce(new Vector2(movementDir * playerSpeed, 0f), ForceMode2D.Impulse);

        // If the velocity isn't 0 (not moving), set the animator to move sprites
        if(rb.velocity != Vector2.zero)
        {
            animate.SetBool("Move", true);
        } else
        {
            animate.SetBool("Move", false);
        }

        // After moving, flip the character depending on direction
        Flip();

        // If the player pressed space while grounded, then jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            Jump();
        }

        // If the player pressed shift and the cooldown is at 0, then dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown == 0)
        {
            Dash();
            Debug.Log("Dash time!");
            //animate.SetBool("Dash", true);
        }

        // If the player presses R, then slash
        if (Input.GetKeyUp(KeyCode.R))
        {
            animate.SetTrigger("Slash");
        }
    }

    /// <summary>
    /// Adds force to the player upwards based on a jump height
    /// </summary>
    private void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    /// <summary>
    /// Flip the character sprites based on player horizontal direction
    /// </summary>
    private void Flip()
    {
        if (movementDir > 0)
        {
            transform.localScale = new Vector3(flipSprite, transform.localScale.y, transform.localScale.z);
        }
        if (movementDir < 0)
        {
            transform.localScale = new Vector3(-flipSprite, transform.localScale.y, transform.localScale.z);
        }
    }

    /// <summary>
    /// Have the player dash based on a cooldown
    /// </summary>
    private void Dash()
    {
        //add dash with a cooldown
    }

    /// <summary>
    /// Reset grounded to true when the player's feet hit the ground
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    /// <summary>
    /// Reset grounded to false when the player's feet leave the ground
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}
