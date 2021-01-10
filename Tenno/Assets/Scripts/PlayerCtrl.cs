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

    private Ray HitRayBody;
    private Ray HitRayHead;
    public Vector3 PlusPositionA = new Vector3(0, 1.0f, 0);
    public Vector3 PlusPositionB = new Vector3(0, 1.5f, 0);
    private RaycastHit HitDetectRayBody;
    private RaycastHit HitDetectRayHead;
    private int PlayerMovementValue;
        public float Height;
    CapsuleCollider PlayerCollider;

    //[SerializeField] private bool IsGround;
    //private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerAnimator.SetBool();
        PlayerAnimator = GetComponent<Animator>();
        Playertransform = GameObject.Find("unitychan").GetComponent<Transform>();
        PlayerRigidbody = GameObject.Find("unitychan").GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();
        //speed = 0.1f;
        //IsGround = true;
        PlayerStates = States.Grounded;

    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 Position = ;

        
        RayTest();

        PlayAnim();       

        JumpOrClimb();

        Squad();

        Slide();
   
        PlayerMovements();
    }

   void Slide()
    {
        if(Input.GetKey(KeyCode.LeftShift)){
            Vector3 direction;
            switch (PlayerStates)
            {

                case States.Grounded:
                    direction = new Vector3(h, 0, v);
                    Playertransform.Translate(direction * speed * 1.5f, Space.Self);
                    PlayerAnimator.SetBool("Slide", true);
                    break;
                case States.Air:
                    //転げまわる
                    break;
                case States.AirDownWall:
                    break;
                case States.AtWall:
                    break;
                case States.Hang:
                    break;

            }
        }
       PlayerAnimator.SetBool("Slide", false);

    }

     void Squad()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            
            switch (PlayerStates)
            {

                case States.Grounded:
                    speed = speed * 0.5f;
                    PlayerCollider.height = Height;
                    break;
                case States.Air:
                    break;
                case States.AirDownWall:
                    break;
                case States.AtWall:
                    speed = speed * 0.5f;
                    PlayerCollider.height = Height;
                    break;
                case States.Hang:
                    PlayerStates = States.AirDownWall;
                    PlayerRigidbody.constraints = RigidbodyConstraints.None;
                    break;

            }
        }
    }

    void RayTest()
    {
        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        HitRayBody = new Ray(transform.position + PlusPositionA, transform.forward);

        HitRayHead = new Ray(transform.position + PlusPositionB, transform.forward);
        if (Physics.Raycast(HitRayBody, out HitDetectRayBody) && !Physics.Raycast(HitRayHead, out HitDetectRayHead))
        {
            //PlayerStates = States.Hang;
            ////PlayerRigidbody.useGravity = false;
            ////PlayerRigidbody.AddForce(new Vector3(0, 45, 0));
            //PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
            //Debug.Log("Hang");

            switch (PlayerStates)
            {

                case States.Grounded:
                    break;
                case States.Air:
                    PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    PlayerStates = States.Hang;
                    Debug.Log("Hang");
                    break;
                case States.AirDownWall:
                    PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    PlayerStates = States.Hang;
                    Debug.Log("Hang");
                    break;
                case States.AtWall:
                    PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    PlayerStates = States.Hang;
                    Debug.Log("Hang");
                    break;
                case States.Hang:
                    PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    PlayerStates = States.Hang;
                    Debug.Log("Hang");
                    break;

            }
        }
        
        //if (PlayerStates == States.AtWall || PlayerStates == States.Air||PlayerStates == States.AirDownWall)
        //{
        // 自分の頭の上にある二つのRayでHitRayBが接触してなくて、HitRayAが接触している状態
        //if (Physics.Raycast(HitRayBody, out HitDetectRayBody) &&! Physics.Raycast(HitRayHead, out HitDetectRayHead))
        //{
        //    PlayerStates = States.Hang;
        //    //PlayerRigidbody.useGravity = false;
        //    //PlayerRigidbody.AddForce(new Vector3(0, 45, 0));
        //    PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        //    Debug.Log("Hang");
        //}

        //}

        

        Debug.DrawRay(HitRayBody.origin, HitRayBody.direction, Color.blue);
        Debug.DrawRay(HitRayHead.origin, HitRayHead.direction, Color.yellow);
        CharacterInput();
    }

  


    void CharacterInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    void PlayerMovements()
    {
        Playertransform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        Vector3 direction;
        switch (PlayerStates)
        {
           
            case States.Grounded:
                direction = new Vector3(h, 0, v);
                Playertransform.Translate(direction * speed, Space.Self);
                break;
            case States.Air:
                direction = new Vector3(0, 0, v);
                Playertransform.Translate(direction * speed, Space.Self);
                break;
            case States.AirDownWall:
                break;
            case States.AtWall:
                if (v >0)
                {
                    direction = new Vector3(h, 0,0);
                }else direction = new Vector3(h, 0, v);

                Playertransform.Translate(direction * speed, Space.Self);
                break;
            case States.Hang:
                direction = new Vector3(h*0.5f, 0, 0);
                Playertransform.Translate(direction * speed, Space.Self);
                if (Input.GetKeyDown(KeyCode.W))
                {
                    //登る
                }
                if (Input.GetKey(KeyCode.S))
                {
                    PlayerStates = States.AirDownWall;
                    PlayerRigidbody.constraints = RigidbodyConstraints.None;
                }
                break;
     
        }
       
        // PlayerRigidbody.MovePosition(direction * speed);

        


       

     
    }

   

    void JumpOrClimb()
    {
        
           
            

            
        
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 ClimbDirection;
            //Vector3 direction;
            switch (PlayerStates)
            {

                case States.Grounded:
                    Debug.Log("Jump");
                    Vector3 JumpDirection = new Vector3(0, 7000f, 0);
                    PlayerRigidbody.AddForce(JumpDirection);

                    PlayerAnimator.SetFloat("Jump",1);
                    PlayerStates = States.Air;
                    break;
                case States.Air:
                    
                    break;
                case States.AirDownWall:
                    //Debug.Log("Climb");
                    //ClimbDirection = new Vector3(0, 5000f, 0);
                    //PlayerRigidbody.AddForce(ClimbDirection);
                    //PlayerStates = States.AirDownWall;
                    break;
                case States.AtWall:
                    Debug.Log("Climb");
                     ClimbDirection = new Vector3(0, 10000f, 0);
                    PlayerRigidbody.AddForce(ClimbDirection);
                    PlayerStates = States.AirDownWall;
                    PlayerAnimator.SetFloat("Jump", 2);
                    break;
                case States.Hang:

                    //登る
                    

                    break;

            }

           
        }

    }
    void PlayAnim()
    {
        if (PlayerAnimator)
        {
            switch (PlayerStates)
            {

                case States.Grounded:
                    if (v != 0)
                    {
                        PlayerAnimator.SetFloat("Speed", v);
                    }
                    if (h != 0)
                    {
                        PlayerAnimator.SetFloat("Speed", 1f);
                        PlayerAnimator.SetFloat("Direction", h);
                    }
                    if (v == 0 && h == 0)
                    {
                        PlayerAnimator.SetFloat("Speed", 0);
                        PlayerAnimator.SetFloat("Direction", 0);
                    }
                    break;
                case States.Air:
   
                    break;
                case States.AirDownWall:
                    
                    break;
                case States.AtWall:
                    if (v >= 0)
                    {
                        PlayerAnimator.SetFloat("Speed", 3);
                    }
                    if (h != 0)
                    {
                        PlayerAnimator.SetFloat("Speed", 1f);
                        PlayerAnimator.SetFloat("Direction", h);
                    }
                    break;
                case States.Hang:
                    PlayerAnimator.SetFloat("Jump", 0);
                    break;

            }
     


        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Plane")
        {
            switch (PlayerStates)
            {

                case States.Grounded:       
                    break;
                case States.Air:
                    PlayerStates = States.Grounded;
                    break;
                case States.AirDownWall:
                    PlayerStates = States.AtWall;
                    break;
                case States.AtWall:
                    break;
                case States.Hang:
                    PlayerStates = States.Grounded;
                    break;

            }

            //IsGround = true;
            PlayerAnimator.SetFloat("Jump", 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            switch (PlayerStates)
            {

                case States.Grounded:
                    PlayerStates = States.AtWall;
                    break;
                case States.Air:
                    PlayerStates = States.AirDownWall;
                    break;
                case States.AirDownWall:
                    PlayerStates = States.AirDownWall;
                    break;
                case States.AtWall:
                    break;
                case States.Hang:
                    PlayerStates = States.AtWall;
                    break;

            }
            //Debug.Log("1");
          
            // stop = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            switch (PlayerStates)
            {

                case States.Grounded:
                    PlayerStates = States.Grounded;
                    break;
                case States.Air:
                    PlayerStates = States.Air;
                    break;
                case States.AirDownWall:
                    PlayerStates = States.AirDownWall;
                    break;
                case States.AtWall:
                    PlayerStates = States.Grounded;
                    break;
                case States.Hang:
                    PlayerStates = States.Hang;
                    break;

            }
     

        }
    }


}
