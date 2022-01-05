using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIToggleHelper : MonoBehaviour {
    public Toggle toggle;
    void OnEnable()
    {
        if (toggle==null)
        {
            toggle = GetComponent<Toggle>();
        }
    }
    public void Toggle(bool on)
    {
        if (on)
        {
            toggle.isOn = on;
            toggle.onValueChanged.Invoke(on);
        }
    }
}
