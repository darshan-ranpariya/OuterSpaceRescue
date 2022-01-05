using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownLabelTMPro : MonoBehaviour
{
    public Dropdown dropdown;
    public TextMeshProUGUI textPro;

    void OnEnable()
    {
        if (textPro == null) textPro = GetComponent<TextMeshProUGUI>();
        dropdown.onValueChanged.AddListener(OnValChanged);
        OnValChanged(dropdown.value);
    }

    void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(OnValChanged);
    }

    void OnValChanged(int v)
    {
        if(v < dropdown.options.Count) textPro.text = dropdown.options[v].text;
        else textPro.text = string.Empty;
    }
}
