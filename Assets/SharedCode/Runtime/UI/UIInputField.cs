using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

interface UICustomInputField
{
    UnityEventString onValueChanged { get; }
    UnityEventString onEndEdit { get; }

    string text { get; set; }
    bool interactable { get; set; }
    bool isFocused { get; }
    bool isSelected { get; }

    void ActivateInputField();
    void DeactivateInputField();
}

public class UIInputField : MonoBehaviour 
{
    InputField inputField;
    TMP_InputField inputFieldPro;
    UICustomInputField inputFieldCustom;

    public event System.Action<string> OnValueChanged;
    public event System.Action<string> OnEndEdit;
    public UnityEventString SubmitEvent;

    public bool currencyField;
    string inputCurrencyText = string.Empty;
    public double inputCurrencyAmount = 0;
    System.Action<bool> MoveTextEnd;

    bool canSubmit;
     
    public string text
    {
        get
        {
            OnValidate();
            if (currencyField) return inputCurrencyAmount.ToString();
            else if (inputField != null) return inputField.text;
            else if (inputFieldPro != null) return inputFieldPro.text;
            else if (inputFieldCustom != null) return inputFieldCustom.text;
            else return string.Empty;
        }
        set
        {
            OnValidate();
            if (inputField != null) inputField.text = value;
            else if(inputFieldPro != null) inputFieldPro.text = value;
            else if (inputFieldCustom != null) inputFieldCustom.text = value;
        }
    }

    public string placeholderText
    {
        get
        {
            OnValidate();
            try
            {
                if (inputField != null) return ((Text)inputField.placeholder).text;
                else if (inputFieldPro != null) return ((TextMeshProUGUI)inputField.placeholder).text;
            }
            catch { }
            return string.Empty;
        }
        set
        {
            OnValidate();
            try
            {
                if (inputField != null) ((Text)inputField.placeholder).text = value;
                if (inputFieldPro != null) ((TextMeshProUGUI)inputField.placeholder).text = value;
            }
            catch { }
        }
    }

    public bool interactable
    {
        get
        {
            OnValidate();
            if (inputField != null) return inputField.interactable;
            else if (inputFieldPro != null) return inputFieldPro.interactable;
            else if (inputFieldCustom != null) return inputFieldCustom.interactable;
            else return false;
        }
        set
        {
            OnValidate();
            if (inputField != null) inputField.interactable = value;
            if (inputFieldPro != null) inputFieldPro.interactable = value;
            if (inputFieldCustom != null) inputFieldCustom.interactable = value;
        }
    }
    public bool isFocused
    {
        get
        {
            OnValidate();
            if (inputField != null) return inputField.isActiveAndEnabled;
            else if (inputFieldPro != null) return inputFieldPro.isActiveAndEnabled;
            else if (inputFieldCustom != null) return inputFieldCustom.isFocused;
            else return false;
        }
    }
    public bool isSelected
    {
        get
        {
            OnValidate();

            if (inputField != null)
            {
                return (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == inputField.gameObject);
            }
            if (inputFieldPro != null)
            {
                return (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == inputFieldPro.gameObject);
            }
            if (inputFieldCustom != null)
            {
                return inputFieldCustom.isSelected;
            }
            else return false;
        }
    }
    public int caretPosition
    {
        get
        {
            OnValidate();
            if (inputField != null) return inputField.caretPosition;
            else if (inputFieldPro != null) return inputFieldPro.caretPosition;
            else return 0;
        }
    }

    public void ActivateInputField()
    {
        OnValidate();
        if (inputField != null) inputField.ActivateInputField(); 
        if (inputFieldPro != null) inputFieldPro.ActivateInputField(); 
        if (inputFieldCustom != null) inputFieldCustom.ActivateInputField(); 
    }

    public void DeactivateInputField()
    {
        OnValidate();
        if (inputField != null) inputField.DeactivateInputField();
        if (inputFieldPro != null) inputFieldPro.DeactivateInputField();
        if (inputFieldCustom != null) inputFieldCustom.DeactivateInputField();
    }

    void OnEnable()
    {
        OnValidate();
        AddListeners();
    }

    void OnDisable()
    {
        RemoveListeners();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            if (isActiveAndEnabled && isSelected)
            {
                //print("submit "+name);
                DeactivateInputField();
                SubmitEvent.Invoke(text);
            }
        }
        if (currencyField)
        {
            //if (inputField!=null)
            //{
            //    if (!inputField.text.Equals(inputCurrencyText))
            //    {
            //        inputField.text = inputCurrencyText;
            //    }
            //}

            //if (caretPosition != inputCurrencyText.Length)
            //{
            //    if (isFocused && interactable && isActiveAndEnabled)
            //    {
            //        if (MoveTextEnd!=null) MoveTextEnd(false);
            //    }
            //}
        }
    }

    void AddListeners()
    {
        if (!Application.isPlaying) return;
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(OVC);
            inputField.onEndEdit.AddListener(OEE);
        }
        if (inputFieldPro != null)
        {
            inputFieldPro.onValueChanged.AddListener(OVC);
            inputFieldPro.onEndEdit.AddListener(OEE);
        }
        if (inputFieldCustom != null)
        {
            inputFieldCustom.onValueChanged.AddListener(OVC);
            inputFieldCustom.onEndEdit.AddListener(OEE);
        }
    }

    void RemoveListeners()
    {
        if (!Application.isPlaying) return;
        if (inputField != null)
        {
            inputField.onValueChanged.RemoveListener(OVC);
            inputField.onEndEdit.RemoveListener(OEE);
        }
        if (inputFieldPro != null)
        {
            inputFieldPro.onValueChanged.RemoveListener(OVC);
            inputFieldPro.onEndEdit.RemoveListener(OEE);
        }
        if (inputFieldCustom != null)
        {
            inputFieldCustom.onValueChanged.RemoveListener(OVC);
            inputFieldCustom.onEndEdit.RemoveListener(OEE);
        }
    }

    int lstDigitCount = 0;
    int crtDigitCount = 0;
    void OVC(string s)
    {
        OnValidate();
        canSubmit = true;
        if (currencyField)
        {
            inputCurrencyAmount = 0;
            List<int> digits = new List<int>();
            for (int i = 0; i < s.Length; i++)
            {
                try { digits.Add(int.Parse(s[i].ToString())); } catch { }
            }
            for (int i = 0; i < digits.Count; i++)
            {
                inputCurrencyAmount += (digits[digits.Count - i - 1] * Math.Pow(10, i));
            }
            inputCurrencyText = inputCurrencyAmount.ToFormattedString(true);
            RemoveListeners();
            if (inputCurrencyAmount > 0) text = inputCurrencyText;
            else text = string.Empty;

            crtDigitCount = digits.Count;
            MoveTextEnd(false);
            lstDigitCount = crtDigitCount;

            AddListeners();
        }
        if (OnValueChanged != null) OnValueChanged(s); 
    }

    void OEE(string s)
    {
        OnValidate();
        canSubmit = false;
        if (OnEndEdit != null) OnEndEdit(s);
        if (SubmitEvent!=null) SubmitEvent.Invoke(text);
    }

    bool b = true;
    void OnValidate()
    {
        if (b)
        {
            b = false;
            inputField = GetComponent<InputField>();
            inputFieldPro = GetComponent<TMP_InputField>();
            inputFieldCustom = GetComponent<UICustomInputField>();
            if (inputField!=null)
            {
                MoveTextEnd = inputField.MoveTextEnd;
                if (currencyField)
                {
                    inputField.contentType = InputField.ContentType.Standard;
                    inputField.customCaretColor = true;
                    inputField.caretColor = inputField.selectionColor = new Color(0, 0, 0, 0);
                }
            }
            if (inputFieldPro != null)
            {
                MoveTextEnd = inputFieldPro.MoveTextEnd;
                if (currencyField)
                {
                    inputFieldPro.contentType = TMP_InputField.ContentType.Standard;
                    inputFieldPro.customCaretColor = true;
                    inputFieldPro.caretColor = inputFieldPro.selectionColor = new Color(0, 0, 0, 0);
                }
            } 
        }
    }
} 