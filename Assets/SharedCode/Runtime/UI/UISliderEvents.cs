using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Slider))]
public class UISliderEvents : MonoBehaviour
{
    [System.Serializable]
    public class KeyEvtPair
    {
        public float key;
        public UnityEvent evt;
    }

    public KeyEvtPair[] events;
    Slider slider;
    void OnEnable()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValChanged);
        OnSliderValChanged(slider.value);
    }

    void OnSliderValChanged(float v)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (v == events[i].key)
            {
                events[i].evt.Invoke();
            }
        }
    }
}
