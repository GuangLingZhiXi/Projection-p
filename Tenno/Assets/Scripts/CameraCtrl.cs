using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    public Transform Head;
    public Transform Body;
    private Transform Cameratransform;
    private float RotationY = 0;
    public Vector3 CameraDir=new Vector3(0,0,0);



    // Start is called before the first frame update
    void Start()
    {
        //  Cameratransform = GameObject.Find("Camera").GetComponent<Transform>();
        this.transform.position = Head.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (>)
        //{



    }


    private void LateUpdate()
    {
        this.transform.position = Head.position+ CameraDir;
        CameraRotate();
    }

    void CameraRotate()
    {
        //Debug.Log(this.transform.eulerAngles);
        RotationY += Input.GetAxis("Mouse Y");
        /*xRotation -= mouseY;*/
        RotationY = Mathf.Clamp(RotationY, -65f, 90f);
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
        //Debug.Log(Body.rotation.y);
        transform.localEulerAngles = new Vector3(-RotationY, Body.localEulerAngles.y, 0);
        // }
        //this.transform.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);


    }
}
