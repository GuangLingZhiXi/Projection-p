using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    private Transform Playertransform;
    private Rigidbody PlayerRigidbody;
    private float h=0;
    private float v = 0;
    public float speed = 0;
    [SerializeField]  private bool IsGround;

    // Start is called before the first frame update
    void Start()
    {
        Playertransform = GameObject.Find("unitychan").GetComponent<Transform>();
        PlayerRigidbody = GameObject.Find("unitychan").GetComponent<Rigidbody>();
        //speed = 0.1f;
        IsGround = true;
    
}

// Update is called once per frame
void Update()
    {
        h = Input.GetAxis("Horizontal") ;
        v = Input.GetAxis("Vertical") ;
        Vector3 direction = new Vector3(h, 0, v);
  

        this.transform.Translate(direction * speed );
        this.transform.Rotate(0,Input.GetAxis("Mouse X"),0);

        Jump();

    }
     void OnCollisionEnter(Collision collision)
    {
        
            IsGround = true;
        
        
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && IsGround == true)
        {

            Vector3 JumpDirection = new Vector3(0, 10.0f, 0);
            PlayerRigidbody.AddForce(JumpDirection);
            IsGround = false;


            //PlayrRigidbody.AddForce(JumpDirection);



        }
    }
}
