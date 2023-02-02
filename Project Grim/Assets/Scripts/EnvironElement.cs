using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironElement : MonoBehaviour
{

    //with these variables, we can just set the values per-prefab. Then when player collides we just apply whatever value it is

    [SerializeField] float damageToPlayer;

    public float DamageToPlayer
    {
        get { return damageToPlayer; }
        set { damageToPlayer = value; }

    }

    [SerializeField] float speedChange; //if this is gonna be a percent (where technically 1 = 100%), it should be multiplied with the player's default speed
    public float SpeedChange
    {
        get { return speedChange; }
        set { speedChange = value; }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
