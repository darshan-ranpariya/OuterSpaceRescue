using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectsStateAnimator : uAnimator
{
    [Serializable]
    public class GameObjectRange
    {
        public GameObject go;
        [MinMax(0,1)]
        public Vector2 range;
    }

    public GameObjectRange[] objects;

    public override void Animate(float value)
    {
        foreach (var obj in objects)
        {
            float v = (value - obj.range.x) / (obj.range.y - obj.range.x);
            obj.go.SetActive(v >= 0 && v <= 1);
        }
    }
} 