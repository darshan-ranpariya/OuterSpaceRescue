using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

/// <summary>
/// Changes the label text when 1st item of dropdown is selected
/// </summary>
public class UIDropdownLabelAlt : MonoBehaviour
    //, IPointerClickHandler
{
    public string altTextKey = "";
    public string altText = "";
    public bool updateOnChange = true;
    public bool upperCase;
    //public bool updateOnClick = false;

    Dropdown _dd;
    Dropdown dd
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

    void Awake()
    {
        if(updateOnChange) dd.onValueChanged.AddListener(UpdateLabel);
    }

    void OnEnable()
    { 
        UpdateLabel(dd.value);
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if(updateOnClick) UpdateLabel(dd.value);
    //}

    public void UpdateLabel(int i)
    {
        StopCoroutine("UL_c");
        StartCoroutine("UL_c");
    }
    IEnumerator UL_c()
    {
        yield return null; 
        if (dd.value == 0 && dd.captionText != null)
        {
            if (!string.IsNullOrEmpty(altTextKey))
            {
                altText = Localization.GetString(altTextKey, altText); 
            }
            if (upperCase) altText = altText.ToUpper();
            dd.captionText.text = altText;
        }
    }
}
