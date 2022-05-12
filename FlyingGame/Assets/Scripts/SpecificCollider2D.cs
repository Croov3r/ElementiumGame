using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpecificCollider2D : MonoBehaviour
{
    public UnityEvent collisionEnterEvent;
    public UnityEvent collisionStayEvent;
    public UnityEvent collisionExitEvent;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 3)
        {
            return;
        }
        collisionEnterEvent.Invoke();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer != 3)
        {
            return;
        }
        collisionStayEvent.Invoke();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != 3)
        {
            return;
        }
        collisionExitEvent.Invoke();
    }
}
