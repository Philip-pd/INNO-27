using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{

    [SerializeField] private Transform pfBullet;
    [SerializeField] private GameObject Enemy;
    public PlayerLogic PlayerLogic;
    // Start is called before the first frame update
    void Start()
    {
        PlayerLogic = GetComponent<PlayerLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")&& PlayerLogic.Bullets > 0) //serialize this
        {
            PlayerLogic.Bullets--;
            Shoot();
        }
    }
    private void Shoot()
    {
        Vector3 dir = new Vector3(-1f, 0f, 0f);
        Transform bulletTransform = Instantiate(pfBullet, transform.position- new Vector3(1f,0,0),Quaternion.identity) ;
        //dir = Object Position - Gun Position Normalized)
        bulletTransform.GetComponent < BulletLogic>().Setup(dir,Enemy);

    }
}
