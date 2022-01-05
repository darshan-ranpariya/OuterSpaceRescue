using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapScrollAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum Repeat { None, Loop, PingPong }
    public enum Direction { Horizontal, Vertical }

    public SnapScroll target;
    public Direction direction;
    public Repeat repeat;
    public float speed = 10;
    public bool pauseOnHover;

    Vector3 delta;
    int dir;
    bool paused;
    void OnEnable()
    {
        dir = 1;
        paused = false;
        delta = Vector3.zero;
        target.ResetScroll();
    }
    void Update()
    {
        if (target == null) return;
        if (paused) return;
        if (target.objectsParentRect.childCount == 0) return;

        delta.x = 0;
        delta.y = 0;
        delta.z = 0;
        if (direction == Direction.Horizontal) delta.x = speed * Time.deltaTime * dir;
        else delta.y = speed * Time.deltaTime * dir;

        delta = target.Scroll(delta);
        if (delta.sqrMagnitude < 0.00001)
        {
            switch (repeat)
            {
                case Repeat.None:
                    paused = true;
                    break;
                case Repeat.Loop:
                    target.ResetScroll();
                    break;
                case Repeat.PingPong:
                    dir *= -1;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!pauseOnHover) return;
        paused = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!pauseOnHover) return;
        if (repeat == Repeat.None) return;
        paused = false;
    }
} 