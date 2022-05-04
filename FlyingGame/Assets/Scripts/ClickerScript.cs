using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ClickerScript : MonoBehaviour
{
    public KeyCode keyCode;
    public Text text;
    public UnityEvent OnPressedEvent;

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            Destroy(gameObject);
            OnPressedEvent.Invoke();
        }
    }
}
