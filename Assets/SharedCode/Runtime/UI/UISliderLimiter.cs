using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISliderLimiter : MonoBehaviour
{
    Slider slider;
    public float limit = -1;
    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnValueChange);
    }

    void OnValueChange(float v)
    {
        if (limit >=0 && v > limit)
        {
            slider.value = limit;
        }
    }
}
