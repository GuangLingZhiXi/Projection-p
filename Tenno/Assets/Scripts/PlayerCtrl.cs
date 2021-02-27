using System;
using System.Collections;
using UnityEngine;
//状态接口类





public class PlayerCtrl : MonoBehaviour
{


    //特殊类
    public Animator PlayerAnimator;
    public Animator CameraAnimator;
    private Transform Playertransform;
    private Rigidbody PlayerRigidbody;
    private CapsuleCollider PlayerCollider;
    public Camera PlayerCamera;

    //位移类
    public Transform ValutEndPosition;
    public Transform HangEndPosition;
    public Transform ClimbEndPosition;
    public Transform RollEndPosition;

    //碰撞检测类
    public ActionDetect HangDetect;
    public ActionDetect ValutDetect;
    public ActionDetect ClimbDetect;
    public ActionDetect HangLimitDetect;
    public ActionDetect GroundDetect;

    //判断变量
    private bool IsInGround;
    private bool CanParkour;
    private bool Vlaut;
    private bool Hang;
    private bool Climb;
    private bool Slide;
    private bool HangIdle;
    private bool Roll;
    private bool HardLand;
    private bool CanJump;

    //位移变量
    private float ParkourTime;
    private float ParkourCostTime;
    private float h = 0;
    private float v = 0;
    private float MoveSpeed;
    private float AirMoveSpeed;
    public float SquadHeight;
    public float StandHeight;
    private float SafeHeight;
    private float LandSpeed;

    private Vector3 ParkourStartPosition;
    private Vector3 ParkourEndPosition;


    // Start is called before the first frame update
    void Start()
    { 
        //初始化各类变量
        Playertransform = GameObject.Find("Player").GetComponent<Transform>();
        PlayerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();

        CanParkour = false;
        Vlaut = false;
        IsInGround = true;
        HangIdle = false;
        Roll = false;
        HardLand = false;
        CanJump = false;

        MoveSpeed = 5;
        AirMoveSpeed = 6.5f;
        StandHeight = PlayerCollider.height;
        ParkourTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnimator.SetFloat("LandSpeed", LandSpeed);

        LandSpeed = PlayerRigidbody.velocity.magnitude;
        Debug.Log(LandSpeed);

        Ray FootRay = new Ray(GroundDetect.transform.position, -GroundDetect.transform.up);
        RaycastHit FootRayHit;
        
        if (Physics.Raycast(FootRay,out FootRayHit))
        {
            
                //Debug.Log(FootRayHit.distance);
            //Debug.Log("碰撞对象: " + FootRayHit.collider.name);
            Debug.DrawLine(FootRay.origin, FootRayHit.point, Color.red);
       
        }
        

        //固定刚体旋转
        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        if (PlayerAnimator)
        {

            //基础运动
            CharacterInput();

            Run();

            NormalJump();

            Squad();

            Rotate();

            GroundCheck();

            //各类跑酷判定
            //翻越判定：翻越触发&&悬挂未触发&&未攀爬&&未翻越&&触地&&跑酷时间结束&&按下按键&&向前位移
            if (ValutDetect.CollisonHappen && !HangLimitDetect.CollisonHappen && !HangDetect.CollisonHappen && !Climb && !Vlaut && IsInGround&& !HangIdle && ParkourTime == 0 && Input.GetKeyDown(KeyCode.Space) && v > 0)
            {

                Vlaut = true;

            }
            //悬挂判定：悬挂触发&&触地&&未悬挂&&未攀爬&&未翻越&&按下按键
            if (HangDetect.CollisonHappen && IsInGround && !Hang && !Climb && !Vlaut && Input.GetKeyDown(KeyCode.Space))
            {

                Hang = true;

            }
            //攀爬判定：攀爬触发&&悬挂未触发&&未翻越&&跑酷时间结束&&向前位移
            if (ClimbDetect.CollisonHappen && !HangLimitDetect.CollisonHappen&& !HangDetect.CollisonHappen && !Vlaut && ParkourTime == 0 && Input.GetKeyDown(KeyCode.Space)&& v > 0)
            {

                Climb = true;
                
            }
            //下落判定：翻越触发&&攀爬触发&&未触地&&未悬挂&&跑酷时间结束&&按下按键
            if (ValutDetect.CollisonHappen && ClimbDetect.CollisonHappen && !IsInGround && !Hang && ParkourTime == 0 && Input.GetKeyDown(KeyCode.S))
            {

                //解除悬挂
                PlayerRigidbody.constraints = RigidbodyConstraints.None;
                PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                PlayerAnimator.SetBool("Hang Idle", false);
                PlayerRigidbody.useGravity = true;
                HangIdle = false;


            }
            //翻滚判定：未触地&&翻滚未触发
            if (PlayerAnimator.GetFloat("LandSpeed")>=5)
            {
                if (FootRayHit.distance < 0.1f)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        Roll = true;
                    }
                    else
                    {
                        HardLand = true;
                        PlayerAnimator.SetBool("Fall", false);
                        PlayerAnimator.SetBool("HardLand", true);
                        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                        StartCoroutine(Landing());
                    }
                   

                }
                //Roll = true;



            }

            //滑铲判定：未攀爬&&未悬挂&&触地&&未滑铲&&按下按键&&向前位移
            if (!Climb && !Hang && !Vlaut && IsInGround && !Slide && Input.GetKeyDown(KeyCode.LeftShift) && v > 0)
            {

                Sliding();

            }
            //站立
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {

                StandUp();

            }

            //跑酷动作
            Parkour();

        }

    }


    IEnumerator Landing()
    {
        //PlayerAnimator.SetBool("CanJump", false);
        yield return new WaitForSeconds(2f);
        HardLand = false;
        PlayerAnimator.SetBool("HardLand", false);
        PlayerRigidbody.constraints = RigidbodyConstraints.None;
        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;   

    }



    //跑酷动作
    private void Parkour()
    {


        if (CameraAnimator)
        {

            if (Vlaut)
            {

                Debug.Log("Vlaut");
                Vlaut = false;
                PlayerRigidbody.isKinematic = true;
                CanParkour = true;
                ParkourCostTime = 0.5f;
                ParkourStartPosition = transform.position;
                ParkourEndPosition = ValutEndPosition.position;
                CameraAnimator.CrossFade("Vault", 0.1f);
                PlayerAnimator.SetBool("Vault", true);
               // PlayerAnimator.SetFloat("LandSpeed", 0);

            }
            if (Hang)
            {

                Debug.Log("Hang");
                Hang = false;
                PlayerRigidbody.isKinematic = true;
                CanParkour = true;
                ParkourCostTime = 0.4f;
                ParkourStartPosition = transform.position;
                ParkourEndPosition = HangEndPosition.position;
                PlayerAnimator.SetBool("Hang", true);
                //PlayerAnimator.SetFloat("LandSpeed", 0);
                //悬挂成功
                if (!HangLimitDetect.CollisonHappen)
                {

                    HangIdle = true;
                    Debug.Log("Hang Idle");
                    PlayerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    PlayerRigidbody.useGravity = false;
                    //PlayerAnimator.SetBool("Jump", false);  
                    PlayerAnimator.SetBool("Hang Idle", true);
                    //PlayerAnimator.SetFloat("LandSpeed", 0);

                }


            }
            if (Climb)
            {

                Debug.Log("Climb");
                Climb = false;
                PlayerRigidbody.isKinematic = true;
                CanParkour = true;
                ParkourCostTime = 0.6f;
                ParkourStartPosition = transform.position;
                ParkourEndPosition = ClimbEndPosition.position;
                PlayerRigidbody.constraints = RigidbodyConstraints.None;
                PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                PlayerRigidbody.useGravity = true;
                PlayerAnimator.SetBool("Climb", true);
                PlayerAnimator.SetBool("Hang Idle", false);
                CameraAnimator.CrossFade("Climb", 0.1f);
                //PlayerAnimator.SetFloat("LandSpeed", 0);
                HangIdle = false;

            }
            if (Roll)
            {
                Debug.Log("Roll");
                Roll = false;
                PlayerRigidbody.isKinematic = true;
                CanParkour = true;
                ParkourCostTime = 0.6f;
                ParkourStartPosition = transform.position;
                ParkourEndPosition = RollEndPosition.position;
                PlayerAnimator.SetBool("Roll", true);
                CameraAnimator.CrossFade("Roll", 0.1f);
                //PlayerAnimator.SetFloat("LandSpeed", 0);

            }

        }
        //跑酷位移
        if (CanParkour && ParkourTime < 1f)
        {

            ParkourTime += Time.deltaTime / ParkourCostTime;
            transform.position = Vector3.Lerp(ParkourStartPosition, ParkourEndPosition, ParkourTime);
            //PlayerAnimator.SetFloat("LandSpeed", 0);

            //跑酷时间重置
            if (ParkourTime >= 1f)
            {

                CanParkour = false;
                ParkourTime = 0f;
                PlayerRigidbody.isKinematic = false;
               // PlayerAnimator.SetBool("Climb", false);
                PlayerAnimator.SetBool("Hang", false);
                PlayerAnimator.SetBool("Climb", false);
                PlayerAnimator.SetBool("Vault", false);
                PlayerAnimator.SetBool("Roll", false);
                PlayerAnimator.SetBool("CanJump", false);
                StartCoroutine(Landing());
                //PlayerAnimator.SetBool("Hang Idle", false);

            }

        }

    }

    //跳跃
    private void NormalJump()
    {

        if (IsInGround && !Slide && !CanParkour)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                Vector3 JumpDirection = new Vector3(0, 150f, 0);
                PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, 0f, PlayerRigidbody.velocity.z);
                PlayerRigidbody.AddForce(JumpDirection, ForceMode.Impulse);

            }

        }

    }

    //站立
    private void StandUp()
    {

        Slide = false;
        PlayerCollider.height = StandHeight;
        Vector3 center = PlayerCollider.center;
        center.y = 0.7f;
        PlayerCollider.center = center;
        MoveSpeed = 5f;
        PlayerAnimator.SetBool("Slide", false);
        PlayerAnimator.SetBool("Crouch", false);
        CameraAnimator.SetBool("Crouch", false);

    }

    //下蹲
    private void Squad()
    {

        if (!HardLand)
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {

                MoveSpeed = 2.5f;
                Vector3 center = PlayerCollider.center;
                center.y = 0.25f;
                PlayerCollider.center = center;
                PlayerCollider.height = SquadHeight;
                PlayerAnimator.SetBool("Crouch", true);
                CameraAnimator.SetBool("Crouch", true);
                //PlayerCamera.transform.Translate(0, 0, 1);

            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {

                StandUp();

            }

        }


    }

    //滑铲
    private void Sliding()
    {

        Slide = true;
        PlayerCollider.height = SquadHeight;
        Vector3 center = PlayerCollider.center;
        center.y = 0.25f;
        PlayerCollider.center = center;
        PlayerRigidbody.AddForce(transform.forward * 6, ForceMode.VelocityChange);
        PlayerAnimator.SetBool("Slide", true);

    }


    //位移
    private void Run()
    {

        if (HardLand)
        {

        }
        else
        {

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

                PlayerAnimator.SetFloat("v", v);
                PlayerAnimator.SetFloat("h", h);

            }
            else if (!IsInGround)
            {

                //通过给刚体的 指定的方向 施加速度，得以控制角色运动
                PlayerRigidbody.MovePosition(PlayerRigidbody.position + MainDirection * AirMoveSpeed * Time.deltaTime);

            }

        }
            //通过transform.TransformDirection()求出相对与物体前方方向的世界方向
            
            
    }

    //位移变量
    void CharacterInput()
    {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

    }

    //转向
    void Rotate()
    {

        Playertransform.Rotate(0, Input.GetAxis("Mouse X"), 0);

    }

    //着陆判断
    private void GroundCheck()
    {

        if (GroundDetect)
        {

            if (GroundDetect.CollisonHappen)
            {

                //Debug.Log(PlayerRigidbody.velocity.magnitude);
                IsInGround = true;
                //PlayerAnimator.SetFloat("LandSpeed", LandSpeed);
                PlayerAnimator.SetBool("CanJump", false);
                PlayerAnimator.SetBool("Fall", false);

            }
            else
            {

                IsInGround = false;
                if (!CanParkour && !Slide&& !HangIdle)
                {
                    
                    PlayerAnimator.SetBool("CanJump",true);

                }
                

            }

        }

    }

}

