using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarView : MonoBehaviour
{
    //Use: healthbar pointing at same direction as camara
    public Transform camera;

    // Update is called after each regular Update => otherwise weird behaiviour
    void LateUpdate()
    {
        transform.LookAt(transform.position + camera.forward);
    }
}
