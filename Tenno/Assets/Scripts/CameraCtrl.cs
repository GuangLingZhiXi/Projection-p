using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    private Transform Cameratransform;

    // Start is called before the first frame update
    void Start()
    {
        Cameratransform = GameObject.Find("Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(-Input.GetAxis("Mouse Y"), 0 ,0 );
    }
}
