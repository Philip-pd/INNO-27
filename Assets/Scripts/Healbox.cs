using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healbox : MonoBehaviour
{
    PlayerStats playerStats;
    GameObject light;
    void Start()
    {
        light = GameObject.Find("LPindicator");
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision collisioninfo)
    {
        playerStats.lifepoints--;
    }
}
