using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMovement : MonoBehaviour
{
    public Vector3 targetPos, offset;
    Vector3 lastPos;
    [Range(0f,1f)]
    public float speed;
    float time;

    public void MoveTo(Vector3 targetPos)
    {
        time = 0;
        StartCoroutine(Move(targetPos + offset));
    }

    IEnumerator Move(Vector3 pos)
    {
        while (time <= 1)
        {
            transform.position = Vector3.Lerp(lastPos, pos, time);
            lastPos = transform.position;
            time += speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void Test()
    {
        MoveTo(targetPos);
    }
}
