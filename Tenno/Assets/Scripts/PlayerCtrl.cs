using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public enum States
    {
        Grounded,
        AtWall,
        AirDownWall,
        Air,
        Hang
    }
    public Animator PlayerAnimator;
    private Transform Playertransform;
    private Rigidbody PlayerRigidbody;
    private float h = 0;
    private float v = 0;
    public float speed = 0;
    public States PlayerStates;
    public Camera PlayerCamera;

    private Ray HitRayA;
    private Ray HitRayB;
    public Vector3 PlusPositionA = new Vector3(0, 1.0f, 0);
    public Vector3 PlusPositionB = new Vector3(0, 1.5f, 0);
    private RaycastHit HitDetectA;
    private RaycastHit HitDetectB;

    //[SerializeField] private bool IsGround;
    //private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerAnimator.SetBool();
        PlayerAnimator = GetComponent<Animator>();
        Playertransform = GameObject.Find("unitychan").GetComponent<Transform>();
        PlayerRigidbody = GameObject.Find("unitychan").GetComponent<Rigidbody>();
        //speed = 0.1f;
        //IsGround = true;
        PlayerStates = States.Grounded;
        
    }

    // Update is called once per frame
    void Update()
    {
       // Vector3 Position = ;
         HitRayA = new Ray(transform.position + PlusPositionA, transform.forward);

         HitRayB = new Ray(transform.position + PlusPositionB, transform.forward);
        if(Physics.Raycast(HitRayA, out HitDetectA, 0.3f) && !Physics.Raycast(HitRayB, out HitDetectB, 0.3f))
        {
            PlayerStates = States.Hang;
            PlayerRigidbody.useGravity = false;
            Debug.Log("Hang");
        }
        
       Debug.DrawRay(HitRayA.origin, HitRayA.direction, Color.blue);
        Debug.DrawRay(HitRayB.origin, HitRayB.direction, Color.yellow);
        CharacterInput();
        //if (stop == true)
        //{
        //    if (v < 0)//後退
        //    {
        //        stop = false;
        //    }
        //    if (v >= 0)
        //    {
        //        PlayerAnimator.SetFloat("Speed", 3f);//SPEEDが3になればW押しても動けなくなる
        //        Rotate();
        //    }
        //    if (h != 0)//水平移動
        //    {
        //        PlayerAnimator.SetFloat("Speed", 1f);
        //        PlayerAnimator.SetFloat("Direction", h);

        //        //  commit
        //        //
        //        Walk();
        //        WalkAnim();
                
        //    }



        //}

        Move();
 
        PlayAnim();

        Squad();

        Rotate();

        JumpOrClimb();

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

    void Move()
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

    void JumpOrClimb()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (PlayerStates == States.AtWall)
                {
                    Debug.Log("Climb");
                    Vector3 ClimbDirection = new Vector3(0, 15000f, 0);
                    PlayerRigidbody.AddForce(ClimbDirection);
                    PlayerStates = States.AirDownWall;
                }

            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (PlayerStates == States.Grounded || PlayerStates == States.AtWall)
            {
                Debug.Log("Jump");
                Vector3 JumpDirection = new Vector3(0, 10000f, 0);
                PlayerRigidbody.AddForce(JumpDirection);

                PlayerAnimator.SetBool("Jump", true);
                PlayerStates = States.Air;
            }

        }

    }
    void PlayAnim()
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
            if(PlayerStates == States.AirDownWall)
            {
                PlayerStates = States.AtWall;
            }else if(PlayerStates == States.Air)
            {
                PlayerStates = States.Grounded;
            }
            
            //IsGround = true;
            PlayerAnimator.SetBool("Jump", false);
        }



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Debug.Log("1");
            if(PlayerStates == States.Grounded)
            {
                PlayerStates = States.AtWall;
            }
           // stop = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            if (PlayerStates == States.AtWall)
            {
                PlayerStates = States.Grounded;
            }

        }
    }


}
