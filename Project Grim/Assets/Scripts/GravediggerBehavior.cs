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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the Player collides with the Gravedigger
        if (collision.gameObject.CompareTag("Player"))
        {
            // Change the movement speed and start the timer
            moveSpeed = 5f;
            timer = true;
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
