using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    public Animator anim;
    GameHandlerScript gh;
    public float timer = 0;

    public void Reset()
	{
        anim.SetTrigger("FadeOut");
        timer = 1.5f;
	}

    void Start()
	{
        gh = FindObjectOfType<GameHandlerScript>();
	}

    void Update()
    {
        if (timer > 0)
		{
            timer -= Time.deltaTime;
            if (timer <= 0)
			{
                anim.SetTrigger("FadeIn");
                gh.Reset();
			}
		}
    }
}
