using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    public Transform Head;
    public Transform Body;
    private Transform Cameratransform;

    private float RotationY = 0;

    public Vector3 CameraDir = new Vector3(0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        //  Cameratransform = GameObject.Find("Camera").GetComponent<Transform>();
        this.transform.position = Head.position;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {

        this.transform.position = Head.position + CameraDir;
        CameraRotate();

    }

    void CameraRotate()
    {

        RotationY += Input.GetAxis("Mouse Y");
        RotationY = Mathf.Clamp(RotationY, -65f, 90f);
        transform.localEulerAngles = new Vector3(-RotationY, Body.localEulerAngles.y, 0);

    }

}
