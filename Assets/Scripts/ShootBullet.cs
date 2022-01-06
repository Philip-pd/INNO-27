using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public GameObject[] players;
    [SerializeField] private Transform pfBullet;
    [SerializeField] private GameObject Enemy;
    public PlayerLogic PlayerLogic;
    public GameObject thisplayer;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player != gameObject)
            {
                Enemy = player;
                break;
            }
        }
        PlayerLogic = gameObject.GetComponent<PlayerLogic>();
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown("space") && PlayerLogic.Bullets > 0 && PlayerLogic.isHuman == true) //serialize this
            {
                PlayerLogic.Bullets--;
                Shoot();
            }
    }
    private void Shoot()
    {
        Vector3 dir = new Vector3(1f, 0f, 0f);
        Transform bulletTransform = Instantiate(pfBullet, gameObject.transform.position + gameObject.transform.forward * 1.5f, Quaternion.identity);
        bulletTransform.GetComponent<BulletLogic>().Setup(dir, Enemy, gameObject);

    }
}