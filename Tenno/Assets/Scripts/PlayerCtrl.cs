using System;
using UnityEngine;
//状态接口类





public class PlayerCtrl : MonoBehaviour
{


    public Animator PlayerAnimator;
    public Animator CameraAnimator;
    private Transform Playertransform;
    private Rigidbody PlayerRigidbody;
    private CapsuleCollider PlayerCollider;
    public Camera PlayerCamera;

    public Transform ValutEndPosition;
    public Transform HangEndPosition;
    public Transform ClimbEndPosition;

    public ActionDetect HangDetect;
    public ActionDetect ValutDetect;
    public ActionDetect ClimbDetect;
    public ActionDetect HangLimitDetect;
    public ActionDetect RollDetect;
    public ActionDetect GroundDetect;

    private bool IsInGround;
    private bool CanParkour;
    private bool Vlaut;
    private bool Hang;
    private bool Climb;
    private bool Slide;

    private float ParkourTime;
    private float ParkourCostTime;
    private float h = 0;
    private float v = 0;
    private float MoveSpeed;
    private float AirMoveSpeed;
    public float SquadHeight;
    public float StandHeight;
    private float Stamina;

    private Vector3 ParkourStartPosition;
    private Vector3 ParkourEndPosition;


    // Start is called before the first frame update
    void Start()
    { 
        //PlayerAnimator.SetBool();
        PlayerAnimator = GetComponent<Animator>();
        Playertransform = GameObject.Find("Player").GetComponent<Transform>();
        PlayerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();

        CanParkour = false;
        Vlaut = false;
        IsInGround = true;

        MoveSpeed = 5;
        AirMoveSpeed = 6.5f;
        StandHeight = PlayerCollider.height;
        ParkourTime = 0;
        Stamina = 1;

    }

    // Update is called once per frame
    void Update()
    {

        GroundCheck();

        Rotate();

        CharacterInput();

        Run();

        Squad();

        NormalJump();

        
        if (ValutDetect.CollisonHappen && !HangDetect.CollisonHappen && !Climb && !Vlaut && IsInGround && ParkourTime == 0 && Input.GetKeyDown(KeyCode.Space) && v > 0)
        {

            Vlaut = true;

        }
        if (HangDetect.CollisonHappen&& IsInGround&& !Hang && !Climb&& !Vlaut && Input.GetKeyDown(KeyCode.Space))
        {

            Hang = true;

        }
        if (ClimbDetect.CollisonHappen && !HangDetect.CollisonHappen && !Vlaut && ParkourTime == 0 && v > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                Climb = true;

            }
        }
        if (ValutDetect.CollisonHappen&&ClimbDetect.CollisonHappen && !IsInGround && !Hang && ParkourTime == 0 && Input.GetKeyDown(KeyCode.S))
        {

            PlayerRigidbody.constraints = RigidbodyConstraints.None;
            PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            PlayerRigidbody.useGravity = true;

        }
        if(!IsInGround&& !RollDetect.CollisonHappen)
        {

            //Debug.Log("CanRoll");
            if (IsInGround&&RollDetect.CollisonHappen && Input.GetKeyDown(KeyCode.C))
            {

                Debug.Log("Roll");

            }

        }
        if (!Climb && !Hang && !Vlaut && IsInGround && !Slide && Input.GetKeyDown(KeyCode.LeftShift) && v > 0)
        {

            Sliding();

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            StandUp();

        }

        Parkour();

    }

    private void Sliding()
    {

        Slide = true;
        PlayerCollider.height = SquadHeight;
        PlayerRigidbody.AddForce(transform.forward * 7, ForceMode.VelocityChange);

    }

    private void StandUp()
    {

        Slide = false;
        PlayerCollider.height = StandHeight;
        MoveSpeed = 5f;

    }

    private void Parkour()
    {
        if (CameraAnimator)
        {

            if (Vlaut)
            {

                Vlaut = false;
                PlayerRigidbody.isKinematic = true;
                CanParkour = true;
                ParkourCostTime = 0.5f;
                ParkourStartPosition = transform.position;
                ParkourEndPosition = ValutEndPosition.position;
                CameraAnimator.CrossFade("Vault", 0.1f);

            }
            if (Hang)
            {

                Hang = false;
                PlayerRigidbody.isKinematic = true;
                CanParkour = true;
                ParkourCostTime = 0.4f;
                ParkourStartPosition = transform.position;
                ParkourEndPosition = HangEndPosition.position;
                if (!HangLimitDetect.CollisonHappen)
                {
                    PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    PlayerRigidbody.useGravity = false;
                }


            }
            if (Climb)
            {

                Climb = false;
                PlayerRigidbody.isKinematic = true;
                CanParkour = true;
                ParkourCostTime = 0.6f;
                ParkourStartPosition = transform.position;
                ParkourEndPosition = ClimbEndPosition.position;
                PlayerRigidbody.constraints = RigidbodyConstraints.None;
                PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                PlayerRigidbody.useGravity = true;
                CameraAnimator.CrossFade("Climb", 0.1f);

            }

        }
        if (CanParkour && ParkourTime < 1f)
        {

            ParkourTime += Time.deltaTime / ParkourCostTime;
            transform.position = Vector3.Lerp(ParkourStartPosition, ParkourEndPosition, ParkourTime);

            if (ParkourTime >= 1f)
            {

                CanParkour = false;
                ParkourTime = 0f;
                PlayerRigidbody.isKinematic = false;

            }

        }

    }

    private void NormalJump()
    {

        if (IsInGround && !Slide)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                Vector3 JumpDirection = new Vector3(0, 150f, 0);
                PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, 0f, PlayerRigidbody.velocity.z);
                PlayerRigidbody.AddForce(JumpDirection, ForceMode.Impulse);

            }

        }

    }

    private void Squad()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {

            MoveSpeed = 2.5f;
            PlayerCollider.height = SquadHeight;

        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {

            StandUp();

        }

    }

    private void Run()
    {
        
            //通过transform.TransformDirection()求出相对与物体前方方向的世界方向
            Vector3 WordDirectionForward = transform.TransformDirection(Vector3.forward);
            Vector3 ForwardDirection = v * WordDirectionForward;//物体前后移动的方向
                                                                //求出相对于物体右方方向的世界方向
            Vector3 WorldDirectionRight = transform.TransformDirection(Vector3.right);
            Vector3 RightDirection = h * WorldDirectionRight;//物体左右移动的方向

            //再将前后左右需要移动的方向都加起来 得出 最终要移动的方向
            Vector3 MainDirection = ForwardDirection + RightDirection;

        if (IsInGround)
        {

            //通过给刚体的 指定的方向 施加速度，得以控制角色运动
            PlayerRigidbody.MovePosition(PlayerRigidbody.position + MainDirection * MoveSpeed * Time.deltaTime);

        }
        else if (!IsInGround)
        {

            //通过给刚体的 指定的方向 施加速度，得以控制角色运动
            PlayerRigidbody.MovePosition(PlayerRigidbody.position + MainDirection * AirMoveSpeed * Time.deltaTime);

        }
            
    }

    void CharacterInput()
    {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

    }

    void Rotate()
    {

        Playertransform.Rotate(0, Input.GetAxis("Mouse X"), 0);

    }

    private void GroundCheck()
    {

        if (GroundDetect)
        {

            if (GroundDetect.CollisonHappen)
            {

                IsInGround = true;

            }
            else
            {

                IsInGround = false;

            }

        }

    }

}

