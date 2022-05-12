using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigidController : MonoBehaviour
{
    [Header("Character")]
    public float lifes;
    GameObject Player;

    [Header("Jumping")]

    public float lowTime;
    public float lowJumpHeight;

    public float airRotationSpeed = 10f;

    bool grounded;
    bool newGrounded;
    bool rememberGrounded;


    [Header("Running")]
    [Space]

    public float runningSpeed;
    public float airRunningSpeed;
    public float runningAccTime;
    public float runningDecTime;

    float currentMaxSpeed;
    Vector3 runningDecVector;
    Vector3 speedVector;
    Vector3 runningAccVector;

    bool rightWalled = false;
    bool leftWalled = false;

    bool walkingTimerEnabled = false;
    float walkingTimer = 0f;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    bool keyA = false;
    bool keyD = false;

    [Header("Collision")]
    [Space]

    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private Transform LeftCheck;
    [SerializeField] private Transform RightCheck;
    Rigidbody2D rigb;

    const float GroundedRadius = 0.1f;
    const float SideRadius = 0.05f;

    Collider2D oldCollider;


    /// ///


    private void Awake()
    {
        rigb = GetComponent<Rigidbody2D>();
        //rigb.freezeRotation = true;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    void GravityController(float scale = 1f)
    {
        rigb.gravityScale = (1 / 9.8f) * 2 * lowJumpHeight / (lowTime * lowTime) * scale;
        return;
    }

    void GroundTouching()
    {
        bool wasGrounded = grounded;
        grounded = newGrounded;
        if (grounded)
        {
            if (!wasGrounded)
            {
                OnLandEvent.Invoke();
            }
        }

        if (!grounded)
        {
            if (rigb.angularVelocity > airRotationSpeed)
            {
                rigb.angularVelocity = airRotationSpeed;
            }
            else if (rigb.angularVelocity < -airRotationSpeed)
            {
                rigb.angularVelocity = -airRotationSpeed;
            }
        }
    }

    public void OnGroundEnter()
    {
        newGrounded = true;
    }

    public void OnGroundExit()
    {
        newGrounded = false;
    }

    //moving

    void Movement()
    {
        rigb.velocity = new Vector3(0, rigb.velocity.y, 0);

        if (grounded)
        {
            currentMaxSpeed = runningSpeed;
        }
        else
        {
            currentMaxSpeed = airRunningSpeed;
        }

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

        if (leftWalled && speedVector.x < 0)
        {
            speedVector = Vector3.zero;
        }
        if (rightWalled && speedVector.x > 0)
        {
            speedVector = Vector3.zero;
        }

        transform.position += runningSpeed * speedVector * Time.fixedDeltaTime;
    }

    //
    // publics
    //
    
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

    public void TakeDamage(float damage)
    {
        lifes -= damage;
    }


    //
    // unities
    //


    void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
        runningAccVector = new Vector3(2 / (runningAccTime * runningAccTime), 0, 0);
        runningDecVector = new Vector3(2 / (runningDecTime * runningDecTime), 0, 0);
        currentMaxSpeed = runningSpeed;
    }

    void Update()
    {
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
    }


    //
    // collision
    //
    

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

        oldCollider = other;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        oldCollider = null;
    }

    public void OnWallEnter(bool isRight)
    {
        if (isRight)
        {
            rightWalled = true;
            return;
        }
        leftWalled = true;
    }

    public void OnWallExit(bool isRight)
    {
        if (isRight)
        {
            rightWalled = false;
            return;
        }
        leftWalled = false;
    }
}
