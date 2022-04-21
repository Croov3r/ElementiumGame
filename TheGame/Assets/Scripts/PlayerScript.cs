using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject Clone;
    bool inTeleMode = false;

    void Start()
    {

    }

    void Update()
    {
        if (inTeleMode)
        {
            Clone.SetActive(true);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Clone.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                inTeleMode = false;
                transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            if (Input.GetMouseButtonDown(1))
            {
                inTeleMode = false;
            }
        }
        else
        {
            Clone.SetActive(false);
            if (Input.GetMouseButtonDown(0))
            {
                inTeleMode = true;
            }
        }
            
    }
}
