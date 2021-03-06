using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimCtrl : MonoBehaviour
{
    public Transform Head;
    //public Transform Body;
    private Transform Cameratransform;

    private float RotationY = 0;

    public Vector3 CameraDir = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {

        this.transform.position = Head.position;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftControl))
        {
            //Debug.Log("roll");
            //this.transform.localEulerAngles = new Vector3(-360, 0, 0);
        }
        //this.transform.position = Head.position + CameraDir;

    }

    private void LateUpdate()
    {

        //this.transform.position = Head.position + CameraDir;
        //this.transform.rotation = Body.transform.rotation;
        //CameraRotate();

    }

}
