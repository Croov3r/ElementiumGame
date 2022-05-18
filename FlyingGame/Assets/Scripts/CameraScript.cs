using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject player;
    PlayerHandler ph;
    public List<GameObject> allplayers;
    public float xOffset;
    public bool playing;

    public float minZoom;
    public float maxZoom;

    Vector3 nowPos;
    float timer;
    float endTime;
    float nowZoom;

    bool toNewPlayer = false;
    

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

        GetComponent<Camera>().orthographicSize = Mathf.Min(maxZoom,Mathf.Max(minZoom, (maxX - minX + 4) / (16f / 9f * 2f), (maxY - minY + 4) / 2f));

        transform.position = new Vector3(numX + xOffset, transform.position.y, transform.position.z);
    }

    public void MovePosition(Vector3 pos, float time = 1f)
    {
        if (playing)
        {
            return;
        }
        toNewPlayer = false;
        nowPos = transform.position;
        timer = time;
        endTime = time;
        nowZoom = GetComponent<Camera>().orthographicSize;
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
            transform.position = (transform.position = new Vector3(ptp.x + xOffset, transform.position.y, transform.position.z) * (endTime - timer) + nowPos * timer) / endTime;
            GetComponent<Camera>().orthographicSize = (8f * (endTime - timer) + nowZoom * timer) / endTime;
            timer -= Time.deltaTime;
            return;
        }

        transform.position = new Vector3(ptp.x + xOffset, transform.position.y, transform.position.z);
    }
}
