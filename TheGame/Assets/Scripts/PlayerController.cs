using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Jumping")]

    public float lowJumpHeight;
    public float highJumpHeight;
    public float highTime;
    public float lowTime;
    public int overGroundJumpingFrames;
    public int afterGroundJumpingFrames;
    public int beforeWallJumpingFrames;
    public int afterWallJumpingFrames;
    public float wallJumpSpeed;
    public float wallJumpNoMoveTime;
    public float wallFriction;

    float gravityScaler = 1f;

    bool grounded;
    bool rememberGrounded;

    int overGroundJumpingTimer = 0;
    int afterGroundJumpingTimer = 0;
    int beforeWallJumpingTimer = 0;
    float highJumpTimer = 0;

    bool touchingRightWall = false;
    bool touchingLeftWall = false;
    bool rememberOrientation = true;
    int afterWallJumpingTimer;
    float noMoveTimer;


    [Header("Running")]
    [Space]

    public float runningSpeed;
    public float runningAccTime;
    public float runningDecTime;

    public float crouchingSpeed;

    bool facingRight = true;
    float currentMaxSpeed;
    Vector3 runningDecVector;
    Vector3 speedVector;
    Vector3 runningAccVector;
    bool canMove = true;
    

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool>{}

    public BoolEvent OnCrouchEvent;
    private bool wasCrouching = false;
    private bool isCrouching = false;

    [Header("Collision")]
    [Space]

    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private Transform CeilingCheck;
    [SerializeField] private Transform LeftCheck;
    [SerializeField] private Transform RightCheck;
    [SerializeField] private Collider2D CrouchDisableCollider;
    Rigidbody2D rigb;

    const float GroundedRadius = 0.1f;
    const float CeilingRadius = 0.05f;
    const float SideRadius = 0.05f;



    /// ///


    private void Awake()
    {
        rigb = GetComponent<Rigidbody2D>();
        rigb.freezeRotation = true;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    //jumping

    void Jump()
    {
        if ((!grounded) && (!rememberGrounded) && (highJumpTimer > highJumpHeight-lowJumpHeight) && !(touchingLeftWall || touchingRightWall))
        {
            return;
        }
        if (!isCrouching)
        {
            if (touchingLeftWall)
            {
                return;
            }
            if (touchingRightWall)
            {
                return;
            }
        }

        rigb.velocity = new Vector2(0f, 2* lowJumpHeight / highTime);
    }

    void WallJump()
    {
        if (grounded || (!(touchingLeftWall || touchingRightWall) && (afterWallJumpingTimer <= 0)))
        {
            return;
        }

        noMoveTimer = wallJumpNoMoveTime;
        if (rememberOrientation)
        {
            speedVector = new Vector3(-wallJumpSpeed, speedVector.y, 0);
        }
        else
        {
            speedVector = new Vector3(wallJumpSpeed, speedVector.y, 0);
        }
        rigb.velocity = new Vector2(0f, 2 * lowJumpHeight / highTime);
    }

    void GravityController(float scale = 1f)
    {
        if (rigb.velocity.y <= 0)
        {
            rigb.gravityScale = (1 / 9.8f) * 2 * lowJumpHeight / (lowTime * lowTime) * scale;
            return;
        }
        rigb.gravityScale = (1 / 9.8f) * 2 * lowJumpHeight / (highTime * highTime) * scale;
    }

    void JumpingTimer()
    {
        if (overGroundJumpingTimer > 0)
        {
            overGroundJumpingTimer--;
        }

        if (beforeWallJumpingTimer > 0)
        {
            beforeWallJumpingTimer--;
        }

        if (afterWallJumpingTimer > 0)
        {
            afterWallJumpingTimer--;
        }

        if (afterGroundJumpingTimer > 0)
        {
            rememberGrounded = true;
            afterGroundJumpingTimer--;
        }
        else
        {
            rememberGrounded = false;
        }



        if (noMoveTimer > 0)
        {
            canMove = false;
            noMoveTimer -= Time.deltaTime;
        }
        else
        {
            canMove = true;
        }

        
    }

    void GroundTouching()
    {
        bool wasGrounded = grounded;
        grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                {
                    if (overGroundJumpingTimer > 0)
                    {
                        Jump();
                    }
                    OnLandEvent.Invoke();
                }

            }
        }
        if (wasGrounded && (!grounded) && (rigb.velocity.y <= 0))
        {
            afterGroundJumpingTimer = afterGroundJumpingFrames;
        }
        if (grounded)
        {
            highJumpTimer = 0f;
        }
        else
        {
            highJumpTimer += 2 * lowJumpHeight / highTime * Time.deltaTime;
        }
    }

    //moving

    void Movement()
    {
        rigb.velocity = new Vector3(0, rigb.velocity.y,0);
        if ((Input.GetKey("a") == Input.GetKey("d")) && canMove)
        {

            if (Mathf.Abs(speedVector.x) < runningDecVector.x * Time.fixedDeltaTime)
            {
                speedVector = Vector3.zero;
            }
            else if (speedVector.x < 0)
            {
                speedVector += runningDecVector * Time.fixedDeltaTime;
            }
            else if (speedVector.x > 0)
            {
                speedVector -= runningDecVector * Time.fixedDeltaTime;
            }
        }
        else if (Input.GetKey("a") && canMove)
        {
            facingRight = false;
            if (speedVector.x > -currentMaxSpeed)
            {
                speedVector -= runningAccVector * Time.fixedDeltaTime;
            }
        }
        else if (Input.GetKey("d") && canMove)
        {
            facingRight = true;
            if (speedVector.x < currentMaxSpeed)
            {
                speedVector += runningAccVector * Time.fixedDeltaTime;
            }
        }

        //wall touch

        bool touchLeft = (Physics2D.OverlapCircle(LeftCheck.position, SideRadius, WhatIsGround) && speedVector.x < 0);
        bool touchRight = (Physics2D.OverlapCircle(RightCheck.position, SideRadius, WhatIsGround) && speedVector.x > 0);

        if (touchLeft || touchRight)
        {
            if (rigb.velocity.y < 0)
            {
                gravityScaler = 1 / wallFriction;
            }
            else
            {
                gravityScaler = 1;
            }

            speedVector = Vector3.zero;
            if (beforeWallJumpingTimer > 0)
            {
                WallJump();
            }
            rememberOrientation = (touchRight && !touchLeft);
        }
        else
        {
            gravityScaler = 1;
        }

        touchingLeftWall = touchLeft;
        touchingRightWall = touchRight;

        if (touchLeft || touchRight)
        {
            afterWallJumpingTimer = afterWallJumpingFrames;
        }

        //end

        transform.position += runningSpeed * speedVector * Time.fixedDeltaTime;
    }

    //crouching

    void Crouching(bool crouch)
    {
        if (wasCrouching &&!crouch)
            if (Physics2D.OverlapCircle(CeilingCheck.position, CeilingRadius, WhatIsGround))
            {
                crouch = true;
            }

        if (crouch)
        {
            if (!wasCrouching)
            {
                wasCrouching = true;
                OnCrouchEvent.Invoke(true);
                currentMaxSpeed = crouchingSpeed;
            }
            CrouchDisableCollider.enabled = false;
        }
        else
        {
            if (wasCrouching)
            {
                wasCrouching = false;
                OnCrouchEvent.Invoke(false);
                currentMaxSpeed = runningSpeed;
            }
            CrouchDisableCollider.enabled = true;
        }
        
    }


    ///


    void Start()
    {
        runningAccVector = new Vector3(2 / (runningAccTime * runningAccTime), 0, 0);
        runningDecVector = new Vector3(2 / (runningDecTime * runningDecTime), 0, 0);
        currentMaxSpeed = runningSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Jump();
            WallJump();
            overGroundJumpingTimer = overGroundJumpingFrames;
            beforeWallJumpingTimer = beforeWallJumpingFrames;
        }

        if (Input.GetKey("space"))
        {
            if (!grounded)
            {
                Jump();
            }
        }

        else if (!grounded)
        {
            highJumpTimer = 100f;
        }
        if (Input.GetKey("s"))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
    }

    private void FixedUpdate()
    {
        JumpingTimer();
        GravityController(gravityScaler);
        GroundTouching();
        Movement();
        Crouching(isCrouching);
    }

}
