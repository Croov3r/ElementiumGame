using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public GameObject cannon;
    RigidController rc;

    void Start()
    {
        rc = GetComponent<RigidController>();
    }


    public void WalkABit()
    {
        rc.Walk(1, 3f);
    }

    public void Update()
    {
        Quaternion t = transform.rotation;
        cannon.transform.rotation = new Quaternion(-t.w,-t.x,-t.y,-t.z);
    }
}
