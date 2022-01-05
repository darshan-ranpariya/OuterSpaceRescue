using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutSaperation : MonoBehaviour
{
    public Transform[] elements;
    GameObjectEvents[] elementsEvents;
    public GameObject onNoneActive;
    Transform lastSaperator;
    int totalActiveObects;

    void Awake()
    {
        elementsEvents = new GameObjectEvents[elements.Length];
        for (int i = 0; i < elements.Length; i++)
        {
            elementsEvents[i] = elements[i].GetComponent<GameObjectEvents>();
            if (elementsEvents[i] != null)
            {
                elementsEvents[i].EnableEvent += UpdateSaperators;
                elementsEvents[i].DisableEvent += UpdateSaperators;
            }
        }
    }

    void OnEnable()
    { 
        UpdateSaperators();
    }

    void UpdateSaperators()
    {
        totalActiveObects = 0;
        for (int i = 0; i < elements.Length; i++)
        {
            if (elementsEvents[i] == null)
            {
                elements[i].gameObject.SetActive(false);
                lastSaperator = elements[i];
            }
            else
            {
                if (elementsEvents[i].gameObject.activeSelf)
                {
                    totalActiveObects++;
                    if (totalActiveObects > 1 && lastSaperator != null)
                    {
                        lastSaperator.gameObject.SetActive(true);
                    }
                }
            }
        }
        onNoneActive.SetActive(totalActiveObects == 0);
    }
}
