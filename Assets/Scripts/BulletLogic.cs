using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{

    private Vector3 direction;
    private int bounces;
    private GameObject target;

    [SerializeField]
    int maxbounce;
    [SerializeField]
    float speed;

    public void Setup(Vector3 dir,GameObject t)
    {
        this.direction = dir;
        this.target = t;
        this.bounces = this.maxbounce;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += this.direction * this.speed * Time.deltaTime;
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer==LayerMask.NameToLayer("Wall"))
            if (bounces > 0)   
            {
                bounces--;
                Bounce(collision.contacts[0].normal);
                return;
            }else
            {
                Destroy(gameObject);
            }
            
        if (collision.gameObject == target)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log(collision.gameObject.name);
        return;
      
    }
    private void Bounce(Vector3 collisionNormal)
    {
        this.direction = Vector3.Reflect(this.direction.normalized, collisionNormal);
        Debug.Log("Out Direction: " + this.direction);
    }
}
