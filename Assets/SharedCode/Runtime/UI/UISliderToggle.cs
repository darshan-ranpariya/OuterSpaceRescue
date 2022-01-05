using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class UISliderToggle : MonoBehaviour, IPointerClickHandler
{
    bool m_isOn = false;
    public bool isOn { get { return m_isOn; } protected set { m_isOn = value; } }
    public Slider slider;
    public UnityAction<bool> onValueChanged;

    void OnEnable()
    {
        Init();
    }
    public virtual void Init()
    {
        SetSliderToValue();
    }

    public virtual void Toggle(bool _isOn)
    {
        isOn = _isOn;
        if (gameObject.activeInHierarchy)
        {
            StopCoroutine("AnimateSlider_c");
            StartCoroutine("AnimateSlider_c");
        }
        else
        {
            SetSliderToValue();
        }
        if(onValueChanged!=null) onValueChanged.Invoke(isOn);
    }
      
    public IEnumerator AnimateSlider_c()
    {
        float to = isOn ? 1 : 0;
        float lf = 0;
        while (lf < 1)
        {
            slider.value = Mathf.Lerp(slider.value, to, .3f);
            lf += Time.deltaTime;
            yield return null;
        }
        SetSliderToValue();
    }

    public void SetSliderToValue()
    {
        slider.value = isOn ? 1 : 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle(!isOn);
    }
}
