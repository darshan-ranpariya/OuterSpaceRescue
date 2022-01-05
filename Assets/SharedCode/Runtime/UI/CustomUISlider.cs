using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomUISlider : MonoBehaviour
{

    public uNumber minValue;
    public uNumber maxValue;
    public uNumber anteAmt;
    public uNumber currVal;

    public UIInputField inputField;
    public Slider slider;
    public UIButton minusBtn;
    public UIButton plusBtn;
    public UIButton betBtn;

    public float value
    {
        get { return slider.value; }
        set { slider.value = value; }
    }

    void OnEnable()
    {
        if (inputField != null) inputField.OnValueChanged += IF_OnValueChanged;
        if (minusBtn != null) minusBtn.Clicked += MinusBtnClicked;
        if (plusBtn != null) plusBtn.Clicked += PlusBtnClicked;
        if (slider != null) slider.onValueChanged.AddListener(CurrValueChanged);
    }


    void OnDisble()
    {
        if (inputField != null) inputField.OnValueChanged -= IF_OnValueChanged;
        if (minusBtn != null) minusBtn.Clicked -= MinusBtnClicked;
        if (plusBtn != null) plusBtn.Clicked -= PlusBtnClicked;
        if (slider != null) slider.onValueChanged.RemoveListener(CurrValueChanged);
    }


    private void PlusBtnClicked()
    {
        if (currVal.Value + anteAmt.Value < maxValue.Value)
        {
            currVal.Value += anteAmt.Value;
            slider.value = (float)(currVal.Value / maxValue.Value);
        }
    }

    private void MinusBtnClicked()
    {
        if (currVal.Value - anteAmt.Value > minValue.Value)
        {
            currVal.Value -= anteAmt.Value;
            slider.value = (float)(currVal.Value / maxValue.Value);
        }
    }

    private void IF_OnValueChanged(string val)
    {
        double v = 0;
        try
        {
            v = double.Parse(val);
        }
        catch (Exception e)
        {
            v = 0;
            Toast.Show(Localization.GetString("enter_valid_amount"), inputField.transform.position);
        }
        if (v < minValue.Value)
        {
            Toast.Show(Localization.GetString("enter_valid_amount"), inputField.transform.position);
        }
        else if (v > maxValue.Value)
        {
            Toast.Show(Localization.GetString("enter_valid_amount"), inputField.transform.position);
        }
        currVal.Value = v;
        slider.value = (float)v;
    }

    public void SetValues(double ante, double minSlider, double maxSlider, double minVal, double maxVal)
    {
        anteAmt.Value = ante;
        slider.minValue = (float)minSlider;
        slider.maxValue = (float)maxSlider;
        minValue.Value = minVal;
        maxValue.Value = maxVal;
        currVal.Value = Math.Floor(Extensions.Lerp(minValue.Value, maxValue.Value, slider.value));
    }


    private void CurrValueChanged(float val)
    {
        currVal.Value = Math.Floor(Extensions.Lerp(minValue.Value, maxValue.Value, slider.value));
    }
}
