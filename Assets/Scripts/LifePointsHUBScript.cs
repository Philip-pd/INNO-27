using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePointsHUBScript : MonoBehaviour
{
    public PlayerLogic _player;
    public PlayerLogic _oponent;
    public Text playerLifeHUD;
    public Text oponentLifeHUD; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        playerLifeHUD.text = _player.HP.ToString();
        oponentLifeHUD.text = _oponent.HP.ToString();
    }
}
