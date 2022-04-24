using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public GameObject region;

    Camera cam;
    float regionLeftEdge;
    float regionRightEdge;

    bool CheckBounds(Vector3 pos)
    {
        float leftEdge = pos.x - 16f / 9f * cam.orthographicSize;
        float rightEdge = pos.x + 16f / 9f * cam.orthographicSize;

        if ((leftEdge > regionLeftEdge) && (rightEdge < regionRightEdge))
        {
            return true;
        }
        return false;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        regionLeftEdge = region.transform.position.x - region.transform.localScale.x / 2;
        regionRightEdge = region.transform.position.x + region.transform.localScale.x / 2;
    }

    void Update()
    {
        if (CheckBounds(player.transform.position))
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
    }

}
