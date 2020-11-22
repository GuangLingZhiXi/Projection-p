using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    private Transform Playertransform;
    private Rigidbody PlayrRigidbody;
    private float h=0;
    private float v = 0;
    public float speed = 0;
    [SerializeField]  private bool IsinAir;

    // Start is called before the first frame update
    void Start()
    {
        Playertransform = GameObject.Find("unitychan").GetComponent<Transform>();
        PlayrRigidbody = GameObject.Find("unitychan").GetComponent<Rigidbody>();
        speed = 0.1f;
        IsinAir = false;
    
}

// Update is called once per frame
void Update()
    {
        h = Input.GetAxis("Horizontal") ;
        v = Input.GetAxis("Vertical") ;
        Vector3 direction = new Vector3(h, 0, v);
        
        this.transform.Translate(direction * speed);
        this.transform.Rotate(0,Input.GetAxis("Mouse X"),0);
        Jump();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == ("Plane"))
        {
            Debug.Log("aaaaaaaaaaaaa");
            IsinAir = false;
        }
    }
    void Jump()
    {
        Vector3 JumpDirection = new Vector3(0, 10.0f, 0);
        if (IsinAir == false&&Input.GetKey(KeyCode.Space))
        {

            IsinAir = true;
            PlayrRigidbody.AddForce(JumpDirection);

        }
    }
}
