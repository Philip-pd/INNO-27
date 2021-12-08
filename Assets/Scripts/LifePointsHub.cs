using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePointsHub : MonoBehaviour
{
    PlayerStats playerStats;
    public Text PlayLifePoints;  
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayLifePoints.text = playerStats.lifepoints.ToString("R");
    }
}
