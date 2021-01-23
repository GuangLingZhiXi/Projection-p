using System;
using UnityEngine;
//状态接口类





public class PlayerCtrl : MonoBehaviour
{
    public enum PlayerStandStates
    {
        Grounded,
        AtWall,
        AirDownWall,
        Air,
        Hang
    }

    public enum PlayerMovementStates
    {
        Idle,
        Run,
        Squad,
        Jump,
        Climb,
        Slide
    }

    public Animator PlayerAnimator;
    private Transform Playertransform;
    private Rigidbody PlayerRigidbody;
    private float h = 0;
    private float v = 0;
    public float speed = 0.05f;
    public PlayerStandStates PlayerStates;
    public PlayerMovementStates PlayerMovement;
    public Camera PlayerCamera;

    private Ray HitRayBody;
    private Ray HitRayHead;
    public Vector3 PlusPositionA = new Vector3(0, 1.0f, 0);
    public Vector3 PlusPositionB = new Vector3(0, 1.5f, 0);
    private RaycastHit HitDetectRayBody;
    private RaycastHit HitDetectRayHead;
    private int PlayerMovementValue;
        public float SquadHeight;
    private float StandHeight;
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
        PlayerStates = PlayerStandStates.Grounded;
        StandHeight = PlayerCollider.height;

    }

    // Update is called once per frame
    void Update()
    {



        HangCheck();

        Rotate();

        CharacterInput();

        if (PlayerAnimator)
        {
            switch (PlayerStates)
            {
                case PlayerStandStates.Grounded:
                    // 立っている時にできること。
                    // 例えば、走る、スライディングする、ジャンプする等
                    Run();
                    SlideWithSquad();
                    JumpAction();

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
                case PlayerStandStates.AtWall:
                    // 壁に接している時。
                    // 壁によじ登る、ジャンプする
                    if (v <= 0)
                    {
                        Run();
                    }
                    JumpUpWall();
                    //JumpBack();

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
                case PlayerStandStates.AirDownWall:
                    // 空中から落ちている時にできること
                    // 例えば、まだ壁があった場合は張り付く、もし無ければそのままGroundまで落ちる等
                    break;
                case PlayerStandStates.Air:
                    // 空中時にできること
                    // 例えば、壁があった場合は張り付く、等
                    
                    JumpAction();
                    break;
                case PlayerStandStates.Hang:
                    // 壁を掴んでいる時にできること
                    // 例えば、よじ登る、ジャンプする、壁から離れてAirDownWallに移行する等
                    //Run();
                    //Climb();
                    Fall();

                    PlayerAnimator.SetFloat("Jump", 0);
                    break;
            }
        }
    }

    private void Fall()
    {
        if (Input.GetKey(KeyCode.S))
        {
            //PlayerRigidbody.constraints = RigidbodyConstraints.
            PlayerRigidbody.constraints = RigidbodyConstraints.None;
            PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            PlayerRigidbody.useGravity = true;
            //Debug.Log("Hang");
            PlayerStates = PlayerStandStates.AirDownWall;
        }
    }

    private void JumpUpWall()
    {
        Vector3 ClimbDirection;
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Climb");
            ClimbDirection = new Vector3(0, 10000f, 0);
            PlayerRigidbody.AddForce(ClimbDirection);
            PlayerStates = PlayerStandStates.AirDownWall;
            PlayerAnimator.SetFloat("Jump", 2);
        }
    }

    private void JumpAction()
    {
        if (v > 0)
        {
            Vector3 direction = new Vector3(h, 0, v);
            Playertransform.Translate(direction * speed, Space.Self);
        }
 
        if (Input.GetKey(KeyCode.Space) && PlayerStates == PlayerStandStates.Grounded)
        {
            Debug.Log("Jump");
            Vector3 JumpDirection = new Vector3(0, 7000f, 0);
            PlayerRigidbody.AddForce(JumpDirection);

            PlayerAnimator.SetFloat("Jump", 1);
            PlayerStates = PlayerStandStates.Air;


        }
    }

    private void SlideWithSquad()
    {
        Vector3 direction = new Vector3(h, 0, v);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

                if (PlayerStates == PlayerStandStates.Grounded && v > 0)
                {
                    speed = 0.75f;
                    Playertransform.Translate(direction * speed * 1.5f, Space.Self);
                    PlayerCollider.height = SquadHeight;
                    PlayerAnimator.SetBool("Slide", true);
                //if (Input.GetKey(KeyCode.LeftShift))
                //{
                //    Debug.Log("1");
                //    speed = 0.025f;
                //    PlayerCollider.height = SquadHeight;
                //}
            }
            }

      
        else if (PlayerStates == PlayerStandStates.Air)
        {

        }

        else
        {
            speed = 0.05f;
        }
    }

    private void Run()
    {
        Vector3 direction = new Vector3(h, 0, v);
        Playertransform.Translate(direction * speed, Space.Self);

    }

 

    void HangCheck()
    {
        HitRayBody = new Ray(transform.position + PlusPositionA, transform.forward);

        HitRayHead = new Ray(transform.position + PlusPositionB, transform.forward);

        Debug.DrawRay(HitRayBody.origin, HitRayBody.direction, Color.blue);
        Debug.DrawRay(HitRayHead.origin, HitRayHead.direction, Color.yellow);
        //PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        
        if (Physics.Raycast(HitRayBody, out HitDetectRayBody,0.5f) && !Physics.Raycast(HitRayHead, out HitDetectRayHead))
        {
            PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
            PlayerStates = PlayerStandStates.Hang;
            PlayerRigidbody.useGravity = false;
            Vector3 direction = new Vector3(h, 0, 0);
            Playertransform.Translate(direction * speed * 0.5f, Space.Self);
            Debug.Log("Hang");

        } 
    }

  


    void CharacterInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    void Rotate()
    {
        
        PlayerCollider.height = StandHeight;
        //Debug.Log(this.transform.eulerAngles);
        Playertransform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        if(PlayerStates == PlayerStandStates.Hang)
        {
            //rotate制限
        }
      


        PlayerAnimator.SetBool("Slide", false);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Plane")
        {
            switch (PlayerStates)
            {

                case PlayerStandStates.Grounded:       
                    break;
                case PlayerStandStates.Air:
                    PlayerStates = PlayerStandStates.Grounded;
                    break;
                case PlayerStandStates.AirDownWall:
                    PlayerStates = PlayerStandStates.AtWall;
                    break;
                case PlayerStandStates.AtWall:
                    PlayerStates = PlayerStandStates.AtWall;
                    break;
                case PlayerStandStates.Hang:
                    PlayerStates = PlayerStandStates.AtWall;
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

                case PlayerStandStates.Grounded:
                    PlayerStates = PlayerStandStates.AtWall;
                    break;
                case PlayerStandStates.Air:
                    PlayerStates = PlayerStandStates.AirDownWall;
                    break;
                case PlayerStandStates.AirDownWall:
                    PlayerStates = PlayerStandStates.AirDownWall;
                    break;
                case PlayerStandStates.AtWall:
                    break;
                case PlayerStandStates.Hang:
                    PlayerStates = PlayerStandStates.AtWall;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            switch (PlayerStates)
            {

                case PlayerStandStates.Grounded:
                    PlayerStates = PlayerStandStates.Grounded;
                    break;
                case PlayerStandStates.Air:
                    PlayerStates = PlayerStandStates.Air;
                    break;
                case PlayerStandStates.AirDownWall:
                    PlayerStates = PlayerStandStates.AirDownWall;
                    break;
                case PlayerStandStates.AtWall:
                    PlayerStates = PlayerStandStates.Grounded;
                    break;
                case PlayerStandStates.Hang:
                    PlayerStates = PlayerStandStates.Hang;
                    break;

            }
        }
    }
}

