using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public List<GameObject> allplayers;
    public PlayerController player;
    public GameObject ball;
    CameraScript cam;

    public void Button()
	{
        player.Button();
    }

    public void ChangePlayer(PlayerController p, float t = 1f)
    {
        player.activated = false;
        cam.MovePosition(p.transform.position, t);
        player = p;
        player.activated = true;
        
    }

    public void ActivatePlayer(bool act)
	{
        player.activated = act;
    }

    void Start()
	{
        cam = Camera.main.GetComponent<CameraScript>();
        player.activated = true;
    }

    void Update()
	{
        if (ball.GetComponent<BulletScript>().stickyPlayer != null)
		{
            return;
		}
        float minDis = 1000000f;
        GameObject thePlayer = player.gameObject;
        foreach (GameObject go in allplayers)
		{
            float dis = (go.transform.position - ball.transform.position).magnitude;
            if (dis < minDis)
			{
                minDis = dis;
                thePlayer = go;
			}
		}
        if (thePlayer != player.gameObject)
		{
            ChangePlayer(thePlayer.GetComponent<PlayerController>(),Mathf.Max(Mathf.Min((player.transform.position - thePlayer.transform.position).magnitude, 15f), 7f)/15f);
		}
	}
}
