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
    public float speed = 0;
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
        // Vector3 Position = ;

        
        RayTest();

        PlayAnim();       

  

        Squad();

  
   
        PlayerMovements();

        CharacterInput();

    }



     void Squad()
    {
       
        //
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
            if(PlayerStates!= PlayerStandStates.Grounded)
            {
                PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                PlayerStates = PlayerStandStates.Hang;
                Debug.Log("Hang");
            }
          
        }        
        Debug.DrawRay(HitRayBody.origin, HitRayBody.direction, Color.blue);
        Debug.DrawRay(HitRayHead.origin, HitRayHead.direction, Color.yellow);
        
    }

  


    void CharacterInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    void PlayerMovements()
    {
        PlayerCollider.height = StandHeight;

        Playertransform.Rotate(0, Input.GetAxis("Mouse X"), 0);

        Vector3 direction = new Vector3(h, 0, v);
        Playertransform.Translate(direction * speed, Space.Self);

        if (Input.GetKey(KeyCode.LeftShift))
        {

            if (PlayerStates == PlayerStandStates.Grounded)
            {
                Playertransform.Translate(direction * speed * 1.5f, Space.Self);
                PlayerAnimator.SetBool("Slide", true);
            }
            else if (PlayerStates == PlayerStandStates.Air)
            {

            }

        }
        PlayerAnimator.SetBool("Slide", false);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = speed * 0.5f;
            PlayerCollider.height = SquadHeight;

        }

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 ClimbDirection;
            //Vector3 direction;
            switch (PlayerStates)
            {

                case PlayerStandStates.Grounded:
                    Debug.Log("Jump");
                    Vector3 JumpDirection = new Vector3(0, 7000f, 0);
                    PlayerRigidbody.AddForce(JumpDirection);

                    PlayerAnimator.SetFloat("Jump", 1);
                    PlayerStates = PlayerStandStates.Air;
                    break;
                case PlayerStandStates.Air:

                    break;
                case PlayerStandStates.AirDownWall:
                    //Debug.Log("Climb");
                    //ClimbDirection = new Vector3(0, 5000f, 0);
                    //PlayerRigidbody.AddForce(ClimbDirection);
                    //PlayerStates = States.AirDownWall;
                    break;
                case PlayerStandStates.AtWall:
                    Debug.Log("Climb");
                    ClimbDirection = new Vector3(0, 10000f, 0);
                    PlayerRigidbody.AddForce(ClimbDirection);
                    PlayerStates = PlayerStandStates.AirDownWall;
                    PlayerAnimator.SetFloat("Jump", 2);
                    break;
                case PlayerStandStates.Hang:

                    //登る


                    break;

            }
            
        //switch (PlayerStates)
        //{
           
        //    case PlayerStandStates.Grounded:
        //        //direction = new Vector3(h, 0, v);
        //        //Playertransform.Translate(direction * speed, Space.Self);
        //        Move();
        //        Jump();

        //        break;
        //    case PlayerStandStates.Air:
        //        //direction = new Vector3(0, 0, v);
        //        //Playertransform.Translate(direction * speed, Space.Self);

        //        break;
        //    case PlayerStandStates.AtWall:
        //        if (v >0)
        //        {
        //            direction = new Vector3(h, 0,0);
        //        }else direction = new Vector3(h, 0, v);

        //        Playertransform.Translate(direction * speed, Space.Self);
        //        break;
        //    case PlayerStandStates.Hang:
        //        direction = new Vector3(h*0.5f, 0, 0);
        //        Playertransform.Translate(direction * speed, Space.Self);
        //        if (Input.GetKeyDown(KeyCode.W))
        //        {
        //            //登る
        //        }
        //        if (Input.GetKey(KeyCode.S))
        //        {
        //            PlayerStates = PlayerStandStates.AirDownWall;
        //            PlayerRigidbody.constraints = RigidbodyConstraints.None;
        //        }
        //        break;
     
        //}
       
        // PlayerRigidbody.MovePosition(direction * speed);

        


       

     
    }

   



    }
    void PlayAnim()
    {
        if (PlayerAnimator)
        {
            switch (PlayerStates)
            {

                case PlayerStandStates.Grounded:
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
                case PlayerStandStates.Air:
   
                    break;
                case PlayerStandStates.AirDownWall:
                    
                    break;
                case PlayerStandStates.AtWall:
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
                case PlayerStandStates.Hang:
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

                case PlayerStandStates.Grounded:       
                    break;
                case PlayerStandStates.Air:
                    PlayerStates = PlayerStandStates.Grounded;
                    break;
                case PlayerStandStates.AirDownWall:
                    PlayerStates = PlayerStandStates.AtWall;
                    break;
                case PlayerStandStates.AtWall:
                    break;
                case PlayerStandStates.Hang:
                    PlayerStates = PlayerStandStates.Grounded;
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

