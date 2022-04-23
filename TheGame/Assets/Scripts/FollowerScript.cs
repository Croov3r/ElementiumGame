using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    EnemyController Ec;
    Vector3 toPlayer;

    void Start()
    {
        Ec = GetComponent<EnemyController>();
    }

    void Update()
    {
        toPlayer = Ec.Player.transform.position - transform.position;
        if (Mathf.Abs(toPlayer.x) > 0.2f)
        {
            Ec.Walk(toPlayer.x);
        }
        else
        {
            Ec.Walk(0);
        }

        if (toPlayer.y > 0.1f)
        {
            Ec.Jump();
        }
    }
}
