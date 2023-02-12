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

    [SerializeField] GameObject checkpointScreen;
    [SerializeField] GameObject cpCheckNum;
    [SerializeField] GameObject cpTitle;
    [SerializeField] GameObject cpSouls;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateTextScreen(int state, int soulsCollected)
    {
        cpCheckNum.GetComponent<TMPro.TextMeshProUGUI>().text = "CHECKPOINT #" + state;
        cpSouls.GetComponent<TMPro.TextMeshProUGUI>().text = "Souls Collected: " + soulsCollected + "/30";
        switch (state)
        {
            case 2:
                cpTitle.GetComponent<TMPro.TextMeshProUGUI>().text = "Haunted Farms";
                break;
            case 3:
                cpTitle.GetComponent<TMPro.TextMeshProUGUI>().text = "The Crypts";
                break;
            default:
                cpTitle.GetComponent<TMPro.TextMeshProUGUI>().text = "GraveDigger Camp";
                break;
        }

        StartCoroutine(DisplayCheckpointScreen());
    }

    private IEnumerator DisplayCheckpointScreen()
    {
        checkpointScreen.SetActive(true);

        yield return new WaitForSeconds(4.0f);

        checkpointScreen.SetActive(false);
    }

    public void passedCheckpoint(Collider2D collision)
    {
        state = int.Parse(collision.gameObject.name);
        
        Debug.Log("Checkpoint " + state + " passed!");
        updateTextScreen(state, GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SoulsCollected);

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
