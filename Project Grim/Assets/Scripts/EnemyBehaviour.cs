using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //Small Ghost Sprite Source: https://www.freeiconspng.com/img/36328
    //Tank Ghost Sprite Source: https://www.spriters-resource.com/mobile/demonhuntlgnd3/sheet/44052/ 
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float baseMoveSpeed = 1f;

    protected Rigidbody2D enemyBody;

    //[SerializeField] protected AudioSource deathSoundEffect;

    [SerializeField] protected int health = 1;

    protected SpriteRenderer spriteRenderer;
    protected CapsuleCollider2D capsuleCollider;

    // Start is called before the first frame update
    protected void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        Move();
    }

    protected void Move()
    {
        //enemy movement
        if (IsFacingRight())
        {
            //move right
            enemyBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            //move left
            enemyBody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    //returns true or false depending on the enemy's direction
    protected bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
        {
            //deathSoundEffect.Play();
            Destroy(gameObject);
          
        }
    }

    //makes the enemy turn upon exiting another box collider
    protected void OnTriggerExit2D(Collider2D collision)
    {
        //turn
        //Debug.Log("Turn");
        //Debug.Log(collision);

        if(collision.tag == "Ground")
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

    }
}
