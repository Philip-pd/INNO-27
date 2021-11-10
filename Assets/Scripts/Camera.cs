using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private Vector3 offsetvector;
    void Start()
    {
        offsetvector = new Vector3(0, 20, -10);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offsetvector;
    }

    internal object ScreenPointToRay(Vector3 mousePosition)
    {
        throw new NotImplementedException();
    }
}
