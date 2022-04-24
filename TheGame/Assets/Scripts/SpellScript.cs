using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpellScript : MonoBehaviour
{
    public Sprite sprite;
    public float damage;
    public bool isDragable = true;
    enum Directions
    {
        TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight
    }
    [SerializeField] Directions GrabbingAnchor = Directions.TopLeft;
    public Color inactiveColor = Color.white;
    public Color activeColor = Color.white;

    [HideInInspector]
    public bool isActive = false;
    Collider2D col;
    SpriteRenderer sr;
    Vector3 mousePosition;
    Vector3 grabbingAddVector;

    [Header("Events")]
    [Space]

    public UnityEvent OnActivate;

    ///

    private void Awake()
    {
        if (OnActivate == null)
            OnActivate = new UnityEvent();
    }

    void Activate()
    {
        isActive = true;
        isDragable = false;
        col.enabled = true;
        sr.color = activeColor;
        OnActivate.Invoke();
    }


    //


    void Start()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
        sr = GetComponent<SpriteRenderer>();
        sr.color = inactiveColor;
        if (isDragable)
        {
            float x = 0;
            float y = 0;
            switch (GrabbingAnchor)
            {
                case Directions.TopLeft:
                    x = -1;
                    y = 1;
                    break;
                case Directions.TopCenter:
                    x = 0;
                    y = 1;
                    break;
                case Directions.TopRight:
                    x = 1;
                    y = 1;
                    break;
                case Directions.MiddleLeft:
                    x = -1;
                    y = 0;
                    break;
                case Directions.MiddleCenter:
                    x = 0;
                    y = 0;
                    break;
                case Directions.MiddleRight:
                    x = 1;
                    y = 0;
                    break;
                case Directions.BottomLeft:
                    x = -1;
                    y = -1;
                    break;
                case Directions.BottomCenter:
                    x = 0;
                    y = -1;
                    break;
                case Directions.BottomRight:
                    x = 1;
                    y = -1;
                    break;
            }
            grabbingAddVector = new Vector3(x * transform.localScale.x / 2, y * transform.localScale.y / 2, 0);
        }
    }



    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        if (isDragable)
        {
            transform.position = mousePosition - grabbingAddVector;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Activate();
        }
    }
}
