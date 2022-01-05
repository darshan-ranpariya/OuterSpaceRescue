using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDropdownHelper : MonoBehaviour {
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

    public List<int> valuesHistory = new List<int>();

    void Awake()
    {
        valuesHistory.Add(dropdown.value);
        dropdown.onValueChanged.AddListener(OnValChanged);
    }

    void OnValChanged(int v) {
        valuesHistory.Add(v);
        if (valuesHistory.Count>2)
        {
            valuesHistory.RemoveAt(0);
        }
    }

    public void SelectNextValue()
    {
        dropdown.value++;
    }

    public void SelectPrevValue()
    {
        dropdown.value--;
    }
}
