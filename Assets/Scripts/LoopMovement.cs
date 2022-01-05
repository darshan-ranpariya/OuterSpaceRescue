using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMovement : MonoBehaviour
{
    public float addOffset;
    public float cutOff;
    [Range(0f,3f)]
    public float speed;
    Vector3 pos;
    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void OnDisable()
    {
        transform.position = startPos;
    }

    private void Update()
    {
        pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        transform.position = pos;
    }

    private void LateUpdate()
    {
        if(transform.position.y < cutOff)
        {
            pos = transform.position;
            pos.y += addOffset;
            transform.position = pos;
        }
    }

}
