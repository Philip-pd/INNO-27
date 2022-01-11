using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] public int HP;
    [SerializeField] float RechargeRate;
    [SerializeField] int CDinFrames;
    public bool isHuman = false;
    public int maxbullets;
    float rechargepercent; //Add same thing for shooting cooldown should roughly be able to shoot once every 1-2 second(s) and get a new shot every 3-5
    int CD;
    public int Bullets { get; set; }
    public HealthBar _healthbar;
    public GameObject endScreen;


    // Start is called before the first frame update
    void Start()
    {
        this.Bullets = maxbullets;
        if (_healthbar)
        {
            _healthbar.setMaxHealth(HP);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (this.Bullets < maxbullets)
        {
            rechargepercent += RechargeRate;
        }
        else { rechargepercent = 0; }
        if (rechargepercent >= 100)
        {
            rechargepercent = 0;
            if (this.Bullets < maxbullets)
            {
                this.Bullets++;
            }
        }
    }
    public void Damage()
    {
        HP -= 20;
        if(_healthbar)
            _healthbar.SetHealth(HP);
        Debug.Log("HP:" + HP);
        if (HP <= 0)
        {
            Debug.Log("player dead");
            endScreen.gameObject.SetActive(true);
        }
    }
}
