using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public bool activated = false;
    public GameObject cannon;
    public GameObject bullet;
    public List<GameObject> bulletPoints;
    public float bulletPointDistance = 0.5f;
 
    float cannonRotation = 70f;
    float realRotation = 70f;
    float strength = 10f;
    bool cannonLocked = false;
    float cannonLockTimer = 0f;
    bool cannonFired = false;
    public float maxStrength = 20f;

    public GameObject ball;
    bool holdingBall = false;


    PlayerHandler ph;
    CameraScript cam;
    RigidController rc;

    public void Button() 
    {
        Shoot();
        cannonFired = true;
        cannonLocked = true;
    }

    void PointTowards()
    {
        if (cannonLocked)
        {
            return;
        }
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference = new Vector3(difference.x, difference.y, 0f);
        strength = Mathf.Min(difference.magnitude/(3f * Mathf.Sqrt(bulletPointDistance) * 0.316228f), maxStrength);
        cannonRotation = -Mathf.Atan(difference.y / difference.x)*180/Mathf.PI + 90f;
        realRotation = MyMathf.Arg(difference);
        BulletPoints();
    }

    void BulletPoints()
	{
        Vector3 difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        difference.Normalize();

        if (cannonLocked)
		{
            return;
		}

        Vector3 velocity = new Vector3(Mathf.Cos(realRotation / 180 * Mathf.PI), Mathf.Sin(realRotation / 180 * Mathf.PI), 0f) * strength;
        for (int i = 0; i < bulletPoints.Count; i++)
		{
            float t = (i+1) * bulletPointDistance;
            bulletPoints[i].transform.position = 0.5f * new Vector3(0, -9.8f, 0) * t * t + velocity * t + cannon.transform.position + 0.5f * difference;
        }
    }

    public void Shoot()
    {
        if (!holdingBall)
		{
            return;
		}

        BulletScript script = ball.GetComponent<BulletScript>();
        Vector3 difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        difference.Normalize();
        script.Set(cannon.transform.position + 0.5f*difference, realRotation, strength);

        holdingBall = false;
    }

    void Start()
    {
        rc = GetComponent<RigidController>();
        ph = FindObjectOfType<PlayerHandler>();
        cam = Camera.main.GetComponent<CameraScript>();
    }

    void ActiveUpdate(bool act)
	{
        //Always

        foreach (GameObject bp in bulletPoints)
        {
            bp.SetActive(act);
        }

        if (!act)
        {
            rc.keyA = false;
            rc.keyD = false;
            return;
		}

        PointTowards();


    }

    void Update()
    {
        Quaternion trea = transform.rotation;
        cannon.transform.rotation = new Quaternion(-trea.w, -trea.x, -trea.y, - trea.z) * Quaternion.Euler(0,0, cannonRotation);

        ActiveUpdate(activated);

        if (Input.GetMouseButtonUp(0))
        {
            cannonLockTimer = 0.001f;
        }

        if (cannonLockTimer > 0)
		{
            cannonLockTimer += Time.deltaTime;
            if (cannonLockTimer > 0.05f)
			{
                if (cannonFired)
                {
                    cannonFired = false;
                    cannonLocked = true;
                    cannonLockTimer = 0;
                    return;
                }
                cannonLocked = !cannonLocked;
                cannonLockTimer = 0;
            }
		}
        
        if ((!activated) && holdingBall)
		{
            ball.GetComponent<BulletScript>().Drop();
            holdingBall = false;
		}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!activated)
			{
                return;
			}
            if (holdingBall)
			{
                Shoot();
                return;
			}
            if ((transform.position - ball.transform.position).magnitude < 2f)
            {
                holdingBall = true;
                ball.GetComponent<BulletScript>().StickTo(gameObject);
            }
        }

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ph.ChangePlayer(gameObject.GetComponent<PlayerController>());
        }
    }
}
