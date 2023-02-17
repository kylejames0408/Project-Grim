using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironElement : MonoBehaviour
{

    //with these variables, we can just set the values per-prefab. Then when player collides we just apply whatever value it is

    [SerializeField] int damageToPlayer;
    [SerializeField] float standTimer;

    public int DamageToPlayer
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

    [SerializeField] public enum ElementType
    {
        Mud,
        Religious,
        DisappearingGround

    }


    [SerializeField] private ElementType type;

    public ElementType Type
    {
        get { return type; }
        set { type = value; }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //just count down for as long as player is on top of it
        if(type == ElementType.DisappearingGround && collision.gameObject.tag == "Player")
        {
            standTimer -= Time.deltaTime;

            if (standTimer <= 0)
            {
                Debug.Log("hello");
                Destroy(this.gameObject);
            }
        }
    }
}
