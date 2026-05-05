using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RectTransformTarget
{
    public RectTransform rect;
    public Vector3 offset;
    public bool x, y, z;
}

public class RectTransformFollower : MonoBehaviour
{
    public List<RectTransformTarget> targets;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }
    private void LateUpdate()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        //print(string.Format("trans Pos {0}  LocalPos {1} ", transform.position, transform.localPosition));
        for (int i = 0; i < targets.Count; i++)
        {
            Vector3 pos = transform.position;
            pos += targets[i].offset;

            if(!targets[i].x)
            {
                pos.x = targets[i].rect.position.x;
            }
            if (!targets[i].y)
            {
                pos.y = targets[i].rect.position.y;
            }
            if (!targets[i].z)
            {
                pos.z = targets[i].rect.position.z;
            }

            targets[i].rect.position = pos;
        }
    }
}
