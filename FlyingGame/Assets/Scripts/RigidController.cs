using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigidController : MonoBehaviour
{
    [Header("Character")]
    GameObject player;

    [Header("Jumping")]
    [Space]

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

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [HideInInspector]
    public bool keyA = false;
    [HideInInspector]
    public bool keyD = false;

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
    CameraScript cam;
    PlayerHandler ph;

    /// ///


    private void Awake()
    {
        rigb = GetComponent<Rigidbody2D>();

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
    // unities
    //


    void Start()
    {
        player = gameObject;
        runningAccVector = new Vector3(2 / (runningAccTime * runningAccTime), 0, 0);
        runningDecVector = new Vector3(2 / (runningDecTime * runningDecTime), 0, 0);
        currentMaxSpeed = runningSpeed;
        cam = Camera.main.GetComponent<CameraScript>();
        ph = FindObjectOfType<PlayerHandler>();
    }

    void Update()
    {
        if (ph.player.gameObject == player)
		{
            keyA = Input.GetKey(KeyCode.A);
            keyD = Input.GetKey(KeyCode.D);
        }

        Vector3 tp = transform.position;
        transform.position = new Vector3(tp.x, tp.y, 0f);
        rigb.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        GravityController();
        GroundTouching();
        Movement();
        transform.rotation = Quaternion.identity;
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
