using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    RigidController rc;

    void Start()
    {
        rc = GetComponent<RigidController>();
    }


    public void WalkABit()
    {
        rc.Walk(1, 3f);
    }

}
