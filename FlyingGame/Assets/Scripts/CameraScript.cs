using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float xOffset;

    void Update()
    {
        Vector3 ptp = player.transform.position;
        transform.position = new Vector3(ptp.x + xOffset, transform.position.y, transform.position.z);
    }
}
