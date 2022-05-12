using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public PlayerController player;

    public void WalkABit()
    {
        player.WalkABit();
    }
    public void ChangePlayer(PlayerController p)
    {
        player = p;
    }
}
