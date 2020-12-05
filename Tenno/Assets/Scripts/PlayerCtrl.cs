using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Animator PlayerAnimator;
    private Transform Playertransform;
    private Rigidbody PlayerRigidbody;
    private float h = 0;
    private float v = 0;
    public float speed = 0;
    [SerializeField] private bool IsGround;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerAnimator.SetBool();
        PlayerAnimator = GetComponent<Animator>();
        Playertransform = GameObject.Find("unitychan").GetComponent<Transform>();
        PlayerRigidbody = GameObject.Find("unitychan").GetComponent<Rigidbody>();
        //speed = 0.1f;
        IsGround = true;

    }

    // Update is called once per frame
    void Update()
    {
        CharacterInput();
        if (stop == true)
        {
            if (v < 0)//後退
            {
                Walk();
                WalkAnim();
                Rotate();
            }
            if (v >= 0)
            {
                PlayerAnimator.SetFloat("Speed", 3f);//SPEEDが3になればW押しても動けなくなる
                Rotate();
            }
            if (h != 0)//水平移動
            {
                PlayerAnimator.SetFloat("Speed", 1f);
                PlayerAnimator.SetFloat("Direction", h);

                //  commit
                Walk();
                WalkAnim();
                
            }



        }
        else
        {
            Walk();
            WalkAnim();
            Rotate();
        }


        Squad();

        Rotate();

        Jump();

    }

     void Rotate()
    {
        Playertransform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }

    void CharacterInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        //Debug.Log(h);
    }

    void Walk()
    {



        Vector3 direction = new Vector3(h, 0, v);
        // PlayerRigidbody.MovePosition(direction * speed);

        Playertransform.Translate(direction * speed,Space.Self);

        


    }

    void Squad()
    {
        if (Input.GetKey(KeyCode.C))
        {

        }
    }

    void WalkAnim()
    {
        if (PlayerAnimator)
        {
            if (v != 0)
            {
                PlayerAnimator.SetFloat("Speed", v);
              
            }
            if (h != 0)
            {
                PlayerAnimator.SetFloat("Speed",1f);
                PlayerAnimator.SetFloat("Direction",h);
            }
            if (v == 0 && h == 0)
            {
                PlayerAnimator.SetFloat("Speed", 0);
                PlayerAnimator.SetFloat("Direction", 0);
            }
         

        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Plane")
        {
            IsGround = true;
            PlayerAnimator.SetBool("Jump", false);
        }



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Debug.Log("1");

            stop = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            //Debug.Log("1");

            stop = false;

        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && IsGround == true)
        {

            Vector3 JumpDirection = new Vector3(0, 10.0f, 0);
            PlayerRigidbody.AddForce(JumpDirection);
            IsGround = false;
            PlayerAnimator.SetBool("Jump", true);

            //PlayrRigidbody.AddForce(JumpDirection);



        }
    }
}
