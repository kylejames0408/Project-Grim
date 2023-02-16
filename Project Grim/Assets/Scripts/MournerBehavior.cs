using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GravediggerBehavior inherits from EnemyBehaviour and is the script necessary for the Gravedigger GameObject.
/// 
/// SPRITE SOURCE: https://legnops.itch.io/red-hood-character
/// </summary>
public class MournerBehavior : EnemyBehaviour
{
    #region Fields
    Animator animate;
    [SerializeField] GameObject prayerBeam;
    bool praying = false;
    float timeRemaining = 3f; // The time for praying & running before praying
    float totalTime;
    #endregion

    #region UnityMethods
    new void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
    }

    new void Update()
    {
        // Move the character
        Move();

        // Pray when necessary
        Pray();

        // Beam Updates
        if (praying)
        {
            prayerBeam.transform.localScale = new Vector3((totalTime-timeRemaining)/totalTime * 5, 20, 1);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        //// If the Player collides with the Gravedigger while dashing
        //if (collision.gameObject.CompareTag("Player") && praying)
        //{
        //    Player p = collision.gameObject.GetComponent<Player>();

        //    // Reduce player health & respawn
        //    p.Health--;
        //    p.gameObject.transform.position = p.checkpointSystem.RespawnPoint().position;
        //}
    }
    #endregion

    #region Methods
    /// <summary>
    /// Counts down the timer for praying and controls praying.
    /// </summary>
    private void Pray()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            praying = !praying;

            if (praying)
            {
                moveSpeed = 0f;
                animate.SetBool("Walk", false);
                animate.SetBool("Pray", true);
                prayerBeam.SetActive(true);
            }
            else
            {
                moveSpeed = 1f;
                animate.SetBool("Pray", false);
                animate.SetBool("Walk", true);
                prayerBeam.SetActive(false);
            }

            timeRemaining = Random.Range(3f, 10f);
            totalTime = timeRemaining;
        }
    }
    #endregion
}
