using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public PlayerController player;
    CameraScript cam;

    public void WalkABit()
    {
        player.WalkABit();
    }

    public void Button()
	{
        player.Button();
    }

    public void ChangePlayer(PlayerController p)
    {
        player.activated = false;
        cam.MovePosition(p.transform.position, 1f);
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
}
