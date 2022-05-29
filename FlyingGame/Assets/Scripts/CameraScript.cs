using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject player;
    PlayerHandler ph;
    public List<GameObject> allplayers;
    public float xOffset;

    Vector3 nowPos;
    float timer;
    float endTime;
    float nowZoom;

    public void PositionCam()
    {
        float numX = 0f;
        float numY = 0f;

        float minX = 1000000f;
        float minY = 1000000f;
        float maxX = -1000000f;
        float maxY = -1000000f;

        foreach (GameObject go in allplayers)
        {
            numX += go.transform.position.x;
            numY += go.transform.position.y;
            if (go.transform.position.x > maxX)
            {
                maxX = go.transform.position.x;
            }
            if (go.transform.position.x < minX)
            {
                minX = go.transform.position.x;
            }
            if (go.transform.position.y > maxY)
            {
                maxY = go.transform.position.y;
            }
            if (go.transform.position.y < minY)
            {
                minY = go.transform.position.y;
            }
        }
        numX /= allplayers.Count;
        numY /= allplayers.Count;

        transform.position = new Vector3(Mathf.Max(-17.77778f, Mathf.Min(17.77778f, numX + xOffset)), transform.position.y, transform.position.z);
    }

    public void MovePosition(Vector3 pos, float time = 1f)
    {
        nowPos = transform.position;
        timer = time;
        endTime = time;
    }


    void Start()
    {
        ph = FindObjectOfType<PlayerHandler>();
        PositionCam();
    }

    void Update()
    {
        player = ph.player.gameObject;
        Vector3 ptp = player.transform.position;

        if (timer > 0)
        {
            Vector3 vec = (new Vector3(ptp.x + xOffset, transform.position.y, transform.position.z) * (endTime - timer) + nowPos * timer) / endTime;
            transform.position = new Vector3(Mathf.Max(-17.77778f, Mathf.Min(17.77778f, vec.x)), vec.y, vec.z);
            timer -= Time.deltaTime;
            return;
        }

        transform.position = new Vector3(Mathf.Max(-17.77778f, Mathf.Min(17.77778f, ptp.x + xOffset)), transform.position.y, transform.position.z);
    }
}
