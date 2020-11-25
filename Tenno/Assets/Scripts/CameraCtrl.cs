using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    private Transform Cameratransform;
    private float RotationY = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cameratransform = GameObject.Find("Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (>)
        //{
   

       CameraRotate();
    }
    void CameraRotate()
    {
        Debug.Log(this.transform.eulerAngles);
        RotationY += Input.GetAxis("Mouse Y") ;
        // xRotation -= mouseY;
        RotationY = Mathf.Clamp(RotationY, -71f, 90f);
        //if (this.transform.eulerAngles.x > 72 && this.transform.eulerAngles.x < 180)
        //{
  
        //    return;
        //}
        //if (this.transform.eulerAngles.x < 270 && this.transform.eulerAngles.x > 180)
        //{
 
        //    return;
        //}
        //else
        //{
            transform.localEulerAngles = new Vector3(-RotationY, 0, 0);
       // }
            //this.transform.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);
            
        
    }
}
