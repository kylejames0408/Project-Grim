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

        if(attackCooldown >= 5)
        {
            attackCooldown = 0;
        }

        //the ghost will start to move if the player comes into range
        if (Vector2.Distance(transform.position, playerTransform.position) <= detectionRange)
        {
            playerInRange = true;
            enemyInAction = true;

            if(attackCooldown <= .75)
            {
                if (animate != null)
                    animate.SetBool("Attack", true);
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
                    SeePlayer(Mathf.Abs(transform.localScale.x) * -1, Mathf.Abs(transform.localScale.x), Vector3.left, Vector3.right);
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
            health -= 1;

            if(health == 0)
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
