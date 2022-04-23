using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public int rate = 3;
    public float duration;

    SpellScript ss;
    Collider2D col;

    float timer = 0;
    float dieTimer = 0;

    void Spell()
    {
        if (timer < rate)
        {
            timer++;
        }
        else
        {
            col.enabled = !col.enabled;
            timer = 0;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        ss = GetComponent<SpellScript>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!ss.isActive)
        {
            return;
        }
        dieTimer += Time.deltaTime;
        if (dieTimer >= duration)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (!ss.isActive)
        {
            return;
        }
        Spell();
    }
}
