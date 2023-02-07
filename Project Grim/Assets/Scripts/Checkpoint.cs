using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
    
{
    // Start is called before the first frame update
    Transform respawnPosition;
    int state = 1;
    [SerializeField] GameObject[] checkpoints;

    [SerializeField] Sprite checkpointOff;
    [SerializeField] Sprite checkpointActivated;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void passedCheckpoint(Collider2D collision)
    {
        state = int.Parse(collision.gameObject.name);
        Debug.Log("Checkpoint " + state + " passed!");
        collision.GetComponent<SpriteRenderer>().sprite = checkpointActivated;

        foreach(GameObject s in checkpoints)
        {
            if (int.Parse(s.name) != state)
            {
                s.GetComponent<SpriteRenderer>().sprite = checkpointOff;
            }
        }
    }

    public Transform RespawnPoint()
    {
        return checkpoints[state-1].transform.GetChild(0);
    }
}
