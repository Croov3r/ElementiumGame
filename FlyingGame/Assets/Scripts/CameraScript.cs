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

    public void MovePosition(Vector3 pos, float time = 1f)
    {
        nowPos = transform.position;
        timer = time;
        endTime = time;
    }

    void Start()
    {
        ph = FindObjectOfType<PlayerHandler>();
        transform.position = new Vector3(-3,0, -10);
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
