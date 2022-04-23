using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [Header("Characters")]
    public GameObject Player;
    public float lifes;

    [Header("Jumping")]
    [Space]
    public float normalHighTime;
    public float normalLowTime;
    public float normalJumpHeight;

    float jumpHeight;
    float highTime;
    float lowTime;

    bool grounded;



    [Header("Running")]
    [Space]

    public float runningSpeed;
    public float runningAccTime;
    public float runningDecTime;

    public float crouchingSpeed;

    float currentMaxSpeed;
    Vector3 runningDecVector;
    Vector3 speedVector;
    Vector3 runningAccVector;

    float walkingTimer;
    bool walkingTimerEnabled = false;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool wasCrouching = false;
    private bool isCrouching = false;

    private bool keyA = false;
    private bool keyD = false;
    private bool keyS = false;

    private Collider2D oldCollider = null;

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
    const float CeilingRadius = .01f;
    const float SideRadius = .01f;



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

    void GravityController()
    {
        if (rigb.velocity.y <= 0)
        {
            rigb.gravityScale = (1 / 9.8f) * 2 * jumpHeight / (lowTime * lowTime);
            return;
        }
        rigb.gravityScale = (1 / 9.8f) * 2 * jumpHeight / (highTime * highTime);
    }

    void Movement()
    {
        rigb.velocity = new Vector3(0, rigb.velocity.y, 0);
        if (keyA == keyD)
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
        else if (keyA)
        {
            if (speedVector.x > -currentMaxSpeed)
            {
                speedVector -= runningAccVector * Time.fixedDeltaTime;
            }
            else
            {
                speedVector = new Vector3(-currentMaxSpeed, 0, 0);
            }
        }
        else if (keyD)
        {
            if (speedVector.x < currentMaxSpeed)
            {
                speedVector += runningAccVector * Time.fixedDeltaTime;
            }
            else
            {
                speedVector = new Vector3(currentMaxSpeed, 0, 0);
            }
        }

        if (Physics2D.OverlapCircle(LeftCheck.position, SideRadius, WhatIsGround) && speedVector.x < 0)
        {
            speedVector = Vector3.zero;
        }
        if (Physics2D.OverlapCircle(RightCheck.position, SideRadius, WhatIsGround) && speedVector.x > 0)
        {
            speedVector = Vector3.zero;
        }

        transform.position += runningSpeed * speedVector * Time.fixedDeltaTime;
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
                    OnLandEvent.Invoke();
                }
                highTime = normalHighTime;
                lowTime = normalLowTime;
            }
        }
    }

    void Crouching(bool crouch)
    {
        if (wasCrouching && !crouch)
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


    //


    public void Jump(float height = -1f)
    {
        if (height == -1)
        {
            height = normalJumpHeight;
        }

        jumpHeight = height;
        if (!grounded)
        {
            return;
        }

        highTime = normalHighTime * Mathf.Sqrt(height) / Mathf.Sqrt(5);
        lowTime = normalLowTime * Mathf.Sqrt(height) / Mathf.Sqrt(5);

        rigb.velocity = new Vector2(0f, 2 * height / highTime);
    }
    
    public void Walk(float x, float duration = -1f)
    {
        keyA = (x < 0f);
        keyD = (x > 0f);
        if (duration == -1f)
        {
            walkingTimerEnabled = false;
            return;
        }
        walkingTimerEnabled = true;
        walkingTimer = duration;
    }

    public void Crouch(bool crouch)
    {
        isCrouching = crouch;
    }

    public void TakeDamage(float damage)
    {
        lifes -= damage;
    }

    //


    void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
        jumpHeight = 5;
        runningAccVector = new Vector3(2 / (runningAccTime * runningAccTime), 0, 0);
        runningDecVector = new Vector3(2 / (runningDecTime * runningDecTime), 0, 0);
        currentMaxSpeed = runningSpeed;

        highTime = normalHighTime;
        lowTime = normalLowTime;
    }

    void Update()
    {
        if (keyS)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        if (walkingTimerEnabled)
        {
            if (walkingTimer > 0)
            {
                walkingTimer -= Time.deltaTime;
            }
            else
            {
                keyA = false;
                keyD = false;
            }
        }
    }

    private void FixedUpdate()
    {
        GravityController();
        GroundTouching();
        Movement();
        Crouching(isCrouching);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (oldCollider == other)
        {
            return;
        }

        var Tags = other.gameObject.GetComponent<CustomTag>();

        if (Tags == null)
        {
            return;
        }

        if (Tags.HasTag("Spell"))
        {
            SpellScript ss = other.gameObject.GetComponent<SpellScript>();
            TakeDamage(ss.damage);

        }

        oldCollider = other;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        oldCollider = null;
    }
}
