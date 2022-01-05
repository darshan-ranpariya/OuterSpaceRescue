using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class UIDropdownValueEvents : MonoBehaviour {
    [Serializable]
    public class DDValEvent
    {
        public int value;
        public UnityEvent onMatch;
        public UnityEvent onUnmatch;
    }

    Dropdown _dd;
    public Dropdown dropdown
    {
        get
        {
            if (_dd == null)
            {
                _dd = GetComponent<Dropdown>();
            }
            return _dd;
        }
    }
    public DDValEvent[] events = new DDValEvent[0];

    void Awake()
    { 
        dropdown.onValueChanged.AddListener(OnValChanged);
    }

    void OnValChanged(int v)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (events[i].value == v) events[i].onMatch.Invoke();
            else events[i].onUnmatch.Invoke();
        }
    } 
}
