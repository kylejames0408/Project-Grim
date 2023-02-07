using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GravediggerBehavior inherits from EnemyBehaviour and is the script necessary for the Gravedigger GameObject.
/// 
/// SPRITE SOURCE: https://ozanoyunbozan.itch.io/grumpy-mayor-dude
/// </summary>
public class GravediggerBehavior : EnemyBehaviour
{
    #region Fields
    bool timer = false; // Whether the timer has started
    float timeRemaining = 3f; // The time that the Gravedigger runs around before disappearing
    #endregion

    #region UnityMethods
    new void Update()
    {
        // Move the character
        Move();

        // Check if the timer is running
        if (timer)
        {
            // Countdown the clock
            Countdown();
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        // If the Player collides with the Gravedigger
        if (collision.gameObject.CompareTag("Player"))
        {
            // If the player dashes
            if (collision.gameObject.GetComponent<Player>().isDashing)
            {
                // Change the movement speed and start the timer
                moveSpeed = 5f;
                timer = true;
            }
            // If the player does not dash
            else
            {
                if (!timer) // if the gravedigger isn't already running away
                {
                    // Reduce player health
                    collision.gameObject.GetComponent<Player>().Health -= 1;
                    // Debug.Log($"Player Health: {collision.gameObject.GetComponent<Player>().Health}");

                    // Remove Gravedigger
                    gameObject.SetActive(false);

                    // Change gameObject to headstone and play bell sound
                    // TODO: Headstone Change
                    // TODO: Bell Sound
                }
            }
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Counts down the timer to remove the Gravedigger.
    /// </summary>
    private void Countdown()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
