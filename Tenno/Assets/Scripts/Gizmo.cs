using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gizmo : MonoBehaviour
{
    //Gizmos.color = Color.yellow;
    // Start is called before the first frame update
    void Start()
    {
        //Gizmos.DrawWireSphere
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.05f);

    }
}
