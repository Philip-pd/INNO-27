using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{

    [SerializeField] private Transform pfBullet;
    [SerializeField] private GameObject Enemy;
    public PlayerLogic PlayerLogic;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        PlayerLogic = GetComponent<PlayerLogic>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && PlayerLogic.Bullets > 0) //serialize this
        {
            //Reloading deactivated for testing purposes
            //PlayerLogic.Bullets--;
            Shoot();
        }
    }
    private void Shoot()
    {
        Vector3 dir = new Vector3(1f, 0f, 0f);
        Transform bulletTransform = Instantiate(pfBullet, player.transform.position + player.transform.forward * 1.5f, Quaternion.identity);
        bulletTransform.GetComponent<BulletLogic>().Setup(dir, Enemy);

    }
}