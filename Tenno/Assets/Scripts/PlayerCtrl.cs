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
        HitRayBody = new Ray(transform.position + PlusPositionA, transform.forward);

        HitRayHead = new Ray(transform.position + PlusPositionB, transform.forward);


        if (PlayerStates == States.AtWall || PlayerStates == States.Air)
        {
            // 自分の頭の上にある二つのRayでHitRayBが接触してなくて、HitRayAが接触している状態
            if (Physics.Raycast(HitRayBody, out HitDetectRayBody, 0.3f) && !Physics.Raycast(HitRayHead, out HitDetectRayHead))
            {
                PlayerStates = States.Hang;
                PlayerRigidbody.useGravity = false;
                //PlayerRigidbody.AddForce(new Vector3(0, 45, 0));
                PlayerRigidbody.velocity = Vector3.zero;
                Debug.Log("Hang");
            }
        }

        if (PlayerStates == States.Hang)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerStates = States.AirDownWall;
                //PlayerRigidbody.isKinematic = false;
                 PlayerRigidbody.useGravity = true;
            }
        }

        Debug.DrawRay(HitRayBody.origin, HitRayBody.direction, Color.blue);
        Debug.DrawRay(HitRayHead.origin, HitRayHead.direction, Color.yellow);
        CharacterInput();
 

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

        Playertransform.Translate(direction * speed, Space.Self);




    }

    void Squad()
    {
        if (Input.GetKey(KeyCode.C))
        {

        }
    }

    void JumpOrClimb()
    {
        
            if (Input.GetKey(KeyCode.W)&&Input.GetKey(KeyCode.Space))
            {
                if (PlayerStates == States.AtWall)
                {
                    Debug.Log("Climb");
                    Vector3 ClimbDirection = new Vector3(0, 1f, 0);
                    Playertransform.transform.Translate(ClimbDirection);
                    //PlayerRigidbody.AddForce(ClimbDirection);
                    PlayerStates = States.AirDownWall;
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
                PlayerAnimator.SetFloat("Speed", 1f);
                PlayerAnimator.SetFloat("Direction", h);
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
            if (PlayerStates == States.AirDownWall)
            {
                //Todo: AtWall以外もある→Airの場合も後で実装
                PlayerStates = States.AtWall;
            }
            else if (PlayerStates == States.Air)
            {
                PlayerStates = States.Grounded;
            }
            else
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
            if (PlayerStates == States.Grounded)
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
            //Todo:AtWallから離れたときはAirになる場合もある
            if (PlayerStates == States.AtWall)
            {
                PlayerStates = States.Grounded;
            }

        }
    }


}
