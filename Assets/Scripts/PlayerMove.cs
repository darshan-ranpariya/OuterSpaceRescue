using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove inst;
    public float maxSpeed = 3.5f;
    public float x, y;
    float minX = -3.25f, maxX = 3.25f;
    float minY = -2.37f, maxY = 4.25f;
    public SpriteRenderer flame; 

    public Transform[] refPos;



    Rigidbody2D rb;
    Coroutine addingForce;
    bool canAddForce;
    Vector3 startPos;
    private void Awake()
    {
        inst = this;
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        //print("left pos" + refPos[1].position);
        minX = refPos[1].position.x;
        //print("top pos" + refPos[0].position);
        //print("right pos" + refPos[2].position);
        maxX = refPos[2].position.x;
        //print("bottom pos" + refPos[3].position);
    }

    private void OnEnable()
    {
        TapListner.OnClick += OnClick;
        TapListner.OnPressed += OnPressed;
        TapListner.OnExit += OnExit;
    }

    private void OnDisable()
    {
        TapListner.OnClick -= OnClick;
        TapListner.OnPressed -= OnPressed;
        TapListner.OnExit -= OnExit;
    }
    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    #region EventMethods
    private void OnExit(bool isFlip)
    {
        canAddForce = false;
    }

    private void OnPressed(bool isFlip)
    {
        if (isFlip) FlipSide();
        else ResetSide();
        canAddForce = true;
        addingForce = StartCoroutine(StartAddingForce());
    }

    private void OnClick(bool isFlip)
    {
        if (isFlip) FlipSide();
        else ResetSide();
        AddForce();
    }
    #endregion

    IEnumerator StartAddingForce()
    {
        while (canAddForce)
        {
            yield return new WaitForSeconds(0.5f);
            AddForce();
        }
    }
    void AddForce()
    {
        Vector2 force = new Vector2(1 * x, 1 * y);
        flame.flipX = !flame.flipX;
        //print("velocity: " + rb.velocity + " || magnitude: " + rb.velocity.magnitude);
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Item"))
        {
            col.GetComponent<Item>().OnPlayerTouched();
        }
    }

    public void FlipSide()
    {
        if (x < 0) return;
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        x *= -1;
        Vector2 v = rb.linearVelocity;
        v.x = 0;
        rb.linearVelocity = v;
    }

    public void ResetSide()
    {
        if (x > 0) return;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        x *= -1;
        Vector2 v = rb.linearVelocity;
        v.x = 0;
        rb.linearVelocity = v;
    }

    public void ResetPos()
    {
        transform.position = startPos;
    }
}


