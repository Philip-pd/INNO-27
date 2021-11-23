using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] int HP;
    [SerializeField] float RechargeRate;
    [SerializeField] int CDinFrames;
    float rechargepercent; //Add same thing for shooting cooldown should roughly be able to shoot once every 1-2 second(s) and get a new shot every 3-5
    int CD;
    public int Bullets { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {       
        this.Bullets = 3;
    }

    // Update is called once per frame
    void Update()
    {

        if(this.Bullets<3)
        { 
            rechargepercent += RechargeRate;
        }
        else { rechargepercent = 0; }
        if (rechargepercent>=100)
        {
            rechargepercent = 0;
            if(this.Bullets<3)
            {
                this.Bullets++;
            }
        }
    }
    void Damage()
    {
        HP--;
        if(HP<=0)
        {
            //loose
        }
    }
}
