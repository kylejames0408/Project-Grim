using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    // Start is called before the first frame update

    public int health;
    public int maxHearts = 3;

    public GameObject soulsCollected;

    public Image[] hearts;
    public Sprite fullH, emptyH;

    public Slider healthBar;

    void Start()
    {
        //health = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Health;
        //soulsCollected = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SoulsCollected;

        healthBar = GetComponent<Slider>();
        healthBar.maxValue = 3;
        healthBar.value = 3;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullH;
            } else
            {
                hearts[i].sprite = emptyH;
            }

            if (i < maxHearts)
            {
                hearts[i].enabled = true;
            } else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void updateSoulCount(int souls)
    {
        soulsCollected.GetComponent<TMPro.TextMeshProUGUI>().text = souls.ToString();
    }

    public void SetHealth(int val)
    {
        healthBar.value = val;
    }
}
