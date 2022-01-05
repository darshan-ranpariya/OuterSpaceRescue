using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPanelToggle : MonoBehaviour 
{ 
    public Toggle toggle;
    public UI_Panel panel;
    public int panelItemIndex;

    void OnEnable()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(TogglePanelItem); 
        StartCoroutine(DelayedOnEnable());
    }

    IEnumerator DelayedOnEnable()
    {
        yield return null;
        TogglePanelItem(toggle.isOn);
    }
    void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(TogglePanelItem);
    }
    void TogglePanelItem(bool b)
    {
        if (b)
        {
            panel.ActivateMenuItem(panelItemIndex);
        } 
    } 
}
