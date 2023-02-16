using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Unity GetComponents 
    Rigidbody2D rb;
    private BoxCollider2D coll;
    Animator animate;
    [SerializeField] Checkpoint checkpointSystem;

    // Player statistics
    public int Health { get; set; }
    public int SoulsCollected { get; set; }
    [SerializeField] bool dead = false;

    // Basic movement and player flipping
    float movementDir;
    [SerializeField] float playerBaseSpeed;// = 3f;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpHeight;// = 5f;
    float flipSprite;

    // Ability Variables
    bool grounded = true;
    bool inMud = false;
    bool canAttack = true;
    bool canDash = true;
    public bool isDashing = false;
    float dashTime = 0.2f;
    public float dashCooldown = 1.5f;
    int jumpCount;

    // SerializedFields to input the ground layer as well as the player's feet position
    [SerializeField] Transform groundPoint;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform attackPoint;

    // Sound Effects
    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource dashSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource playerDeathSoundEffect;

    void Start()
    {
        // Set the sprite's direction as well as get the rigidbody and animator
        flipSprite = transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        animate = GetComponent<Animator>();
        playerSpeed = playerBaseSpeed;
        Health = 3;
        SoulsCollected = 0;
        jumpCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the movement axis and set it to the velocity. 
        // AddForce for this case makes the player slide, not what is intended.
        movementDir = Input.GetAxisRaw("Horizontal");

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

        if (IsGrounded())
        {
            jumpCount = 2;
        }

        // If the player pressed space while grounded, then jump
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && jumpCount > 0 && !inMud) {
            Jump();
            jumpCount -= 1;
            jumpSoundEffect.Play();
        }

        // If the player pressed shift and the cooldown is at 0, then dash
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetMouseButtonUp(1)) && canDash)
        { 
            StartCoroutine(Dash());
            //Debug.Log("Dash time!");

            dashSoundEffect.Play();
        }
       
        // If the player presses R, then slash
        if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R)) && canAttack)
        {
            StartCoroutine(Attack());

            attackSoundEffect.Play();
        }
    }

    /// <summary>
    /// SOURCE: https://gist.github.com/bendux/aa8f588b5123d75f07ca8e69388f40d9
    /// </summary>
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(movementDir * playerSpeed, rb.velocity.y);
        //rb.AddForce(new Vector2(movementDir * playerSpeed, 0f), ForceMode2D.Impulse);
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
    /// SOURCE: https://gist.github.com/bendux/aa8f588b5123d75f07ca8e69388f40d9
    /// </summary>
    private IEnumerator Dash()
    {
        // Set dash and dashing booleans
        canDash = false;
        isDashing = true;
        animate.SetBool("Dash", true);
        // Track the original gravity
        float originalGravity = rb.gravityScale;
        // Turn off gravity's affect while dashing
        rb.gravityScale = 0f;
        // Set the dash velocity
        rb.velocity = new Vector2(transform.localScale.x * 5f, 0f);

        // Wait until done dashing
        yield return new WaitForSeconds(dashTime);
        // Return original gravity
        rb.gravityScale = originalGravity;
        // Return to normal dash state
         animate.SetBool("Dash", false);
        isDashing = false;

        // Wait until the cooldown finishes
        yield return new WaitForSeconds(dashCooldown);
        // Allow the player to dash again
        canDash = true;
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        attackPoint.gameObject.SetActive(true);
        animate.SetTrigger("Slash");

        yield return new WaitForSeconds(0.25f);

        attackPoint.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }

    /// <summary>
    /// Reset grounded to true when the player's feet hit the ground
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        
        /*
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
        */

        if(collision.gameObject.tag == "Environment")
        {
            EnvironElement environment = collision.gameObject.GetComponent<EnvironElement>();

            Debug.Log("hit environment");

            switch(environment.Type)
            {
                case EnvironElement.ElementType.Mud:
                    playerSpeed *= environment.SpeedChange; //the player speed variable will need a default value in the future. right now the player will just be permanently slower every time they walk on mud
                                                            //also, undo this in the OnCollisionExit2D
                    inMud = true;
                    break;

                case EnvironElement.ElementType.Religious:
                    Health -= environment.DamageToPlayer;
                    Debug.Log(Health);
                    break;



            }
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

        if (collision.gameObject.tag == "Environment") //reset player's speed when exiting mud
        {
            playerSpeed = playerBaseSpeed;
            inMud = false;
        }
    }

    //new grounding
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            checkpointSystem.passedCheckpoint(collision);
        }


        if (collision.gameObject.tag == "Ghost")
        {
            if (animate.GetBool("Dash") != true)
            {
                //dead = true;
                //playerDeathSoundEffect.Play();
                //rb.transform.position = new Vector3(checkpointSystem.RespawnPoint().position.x, checkpointSystem.RespawnPoint().position.y + 1f, checkpointSystem.RespawnPoint().position.z);
                //dead = false;
            }
        }

        if (collision.gameObject.tag == "PrayerLight" && !isDashing)
        {
            // Reduce player health & respawn
            Health--;
            rb.transform.position = new Vector3(checkpointSystem.RespawnPoint().position.x, checkpointSystem.RespawnPoint().position.y + 1f, checkpointSystem.RespawnPoint().position.z);
        }
        
        if (collision.gameObject.tag == "DeathBox")
        {
            dead = true;
            rb.transform.position = new Vector3(checkpointSystem.RespawnPoint().position.x, checkpointSystem.RespawnPoint().position.y + 1f, checkpointSystem.RespawnPoint().position.z);
            dead = false;
        }

        if (collision.gameObject.tag == "Environment")
        {
            EnvironElement environment = collision.gameObject.GetComponent<EnvironElement>();

            Debug.Log("hit environment");

            if (environment.Type == EnvironElement.ElementType.Religious)
            {

                    Health -= environment.DamageToPlayer;
                    Debug.Log(Health);



            }
        }
    }
}
