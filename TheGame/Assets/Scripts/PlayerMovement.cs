using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerController pc;
    void Start()
    {
        pc = gameObject.GetComponent<PlayerController>();
        GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    void Update()
    {
        pc.Move(Input.GetAxis("Horizontal"), false, false);
        if (Input.GetKeyDown("space"))
        {
            pc.Move(0f, false, true);
        }
    }
}
