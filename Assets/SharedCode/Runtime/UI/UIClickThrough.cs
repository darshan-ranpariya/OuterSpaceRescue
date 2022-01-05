using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIClickThrough : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    static GraphicRaycaster _rayCaster = null;
    static GraphicRaycaster rayCaster
    {
        get
        {
            if (_rayCaster == null)
            {
                _rayCaster = FindObjectOfType<GraphicRaycaster>();
            }
            return _rayCaster;
        }
    }

    public bool passManualClicks = true;
    public float clickTimeLimit = 0;
    DateTime downTime;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!passManualClicks) return;
        if (clickTimeLimit > 0 && DateTime.Now.Subtract(downTime).TotalSeconds > clickTimeLimit) return;
        print("click");
        SendClick(eventData);
    }

    public void SendClick(PointerEventData eventData)
    {
        new Delayed.Action<PointerEventData>((ed) =>
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ed, results);
            //print(results.Count);
            if (results.Count > 1)
            {
                IPointerClickHandler[] compsClick = new IPointerClickHandler[0];
                int ri = 1;
                while ((compsClick == null || compsClick.Length == 0) && (ri < results.Count))
                {
                    compsClick = results[ri].gameObject.GetComponents<IPointerClickHandler>();
                    int c = 0;
                    if (compsClick != null) c = compsClick.Length;
                    //print(ri + " " + results[ri].gameObject.name + " " + c);
                    if (compsClick != null)
                    {
                        for (int i = 0; i < compsClick.Length; i++)
                        {
                            compsClick[i].OnPointerClick(ed);
                        }
                    }
                    ri++;
                }
            }
        }, eventData, .05f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        downTime = DateTime.Now;
    }
}
