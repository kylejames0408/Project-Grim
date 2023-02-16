using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteraction : EnemyBehaviour
{
    //player object
    public Transform playerTransform;
    public bool playerInRange = false;
    private bool enemyInAction = false;
    [SerializeField] public Animator animate;

    ////how close you can be to ghost before it sees the player
    public float detectionRange;

    //transform - scale for directions
    private float leftDirX;
    private float rightDirX;

    private float attackCooldown = 0;

    //enemy invulnerability
    private bool canBeHit = true;
    private float invulnCounter = 0;

    //different enemy actions when they see the player
    public enum EnemyAction
    {
        Chase,
        Flee
    }

    //defaults to fleeing when seeing the player
    [SerializeField] public EnemyAction enemyAction = EnemyAction.Flee;

    // Start is called before the first frame update
    //private void Start()
    //{
    //    //target = GameObject.FindGameObjectWithTag("Player").transform;
    //    rightDirX = Mathf.Abs(transform.localScale.x);
    //    leftDirX = Mathf.Abs(transform.localScale.x) *-1;
    //}

    // Update is called once per frame
    new void Update()
    {
        //Debug.Log(isChasing);

        attackCooldown += Time.deltaTime;
        spriteRenderer.color = Color.white;
        capsuleCollider.enabled = false;

        if (attackCooldown >= 4)
        {
            attackCooldown = 0;
        }

        //after the period of invulnerability is over, the enemy can be hit again
        if (invulnCounter >= .75f)
        {
            invulnCounter = .75f;
            canBeHit = true;
        }

        //changes sprite color to red and starts counter for invulnerability after an enemy has been hit
        if (canBeHit == false)
        {
            invulnCounter += Time.deltaTime;
            spriteRenderer.color = Color.red;
        }
     
        //the ghost will start to move if the player comes into range
        if (Vector2.Distance(transform.position, playerTransform.position) <= detectionRange)
        {
            playerInRange = true;
            enemyInAction = true;

            if(attackCooldown <= 1)
            {
        
                if (animate != null)
                    animate.SetBool("Attack", true);
                    capsuleCollider.enabled = true;
                    //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Health -= 1;
            }
            else
            {
                if (animate != null)
                    animate.SetBool("Attack", false);
            }
        }

        else
        {
            playerInRange = false;
            if (animate != null)
                animate.SetBool("Attack", false);
        }

        //if (playerInRange == false)
        //{
        //    Move();
        //}

        //enemy performs an action when they see the player
        //if (playerInRange)
        if (enemyInAction)
        {
            switch (enemyAction)
            {
                case EnemyAction.Chase:
                    if(canBeHit)
                    {
                        SeePlayer(Mathf.Abs(transform.localScale.x) * -1, Mathf.Abs(transform.localScale.x), Vector3.left, Vector3.right);
                    }
                
                    break;
                case EnemyAction.Flee:
                    SeePlayer(Mathf.Abs(transform.localScale.x) * -1, Mathf.Abs(transform.localScale.x), Vector3.right, Vector3.left);
                    break;
            }
        }
    }

    //the enemy moves in a certain direction when they see the player
    //the floats determine what direction it will face
    //the vectors determine where it moves
    private void SeePlayer(float xDirectionOne, float xDirectionTwo, Vector3 vecDirectionOne, Vector3 vecDirectionTwo)
    {
        //if player is to the left
        if (transform.position.x > playerTransform.position.x)
        {
            transform.localScale = new Vector2(-xDirectionOne, transform.localScale.y);
            transform.position += vecDirectionOne * moveSpeed * Time.deltaTime;
        }

        //if player is on the right
        if (transform.position.x < playerTransform.position.x)
        {
            transform.localScale = new Vector2(-xDirectionTwo, transform.localScale.y);
            transform.position += vecDirectionTwo * moveSpeed * Time.deltaTime;
        }
    }

    new protected void OnTriggerExit2D(Collider2D collision)
    {
        //makes enemy stop at a ledge
        if (collision.tag == "Ground")
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            moveSpeed = 0;
        }
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        //moves the enemy when not at a ledge
        if (collision.tag == "Ground")
        {
            moveSpeed = baseMoveSpeed;
        }

        if (collision.tag == "Attack")
        {
    
            if(canBeHit)
            {
                health -= 1;
                canBeHit = false;
                invulnCounter = 0;
                spriteRenderer.color = Color.red;
            }
         
            if (health == 0)
            {
                if (animate != null)
                {
                    animate.SetBool("Dead", true);
                }

                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SoulsCollected += 1;
                Destroy(gameObject);
            }
        }
    }
}
