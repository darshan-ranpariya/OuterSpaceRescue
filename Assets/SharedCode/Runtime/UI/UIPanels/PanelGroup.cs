using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelGroup : MonoBehaviour 
{  
    //[HideInInspector]
    [SerializeField]
    List<Panel> panels = new List<Panel>(); 
 
    List<Panel> panelsHistory = new List<Panel>(); 
    public Panel defaultPanel;

    void OnValidate()
    {
        for (int i = panels.Count - 1; i >= 0; i--)
        {
            if (panels[i] == null) panels.RemoveAt(i);
            else if (panels[i].group == null || panels[i].group != this) panels.RemoveAt(i);
        }
    }

    void Awake()
    {
        if (defaultPanel != null) RegisterPanel(defaultPanel); 
        StartCoroutine(DelayedAwake());
    }

    IEnumerator DelayedAwake()
    {
        yield return null;  
        for (int i = 0; i < panels.Count; i++)
        {  
            if (defaultPanel != null && panels[i] == defaultPanel) continue;
            panels[i].Deactivate(); 
        }
    }

    void OnEnable()
    {
        if(defaultPanel != null) defaultPanel.Activate();
    }  

    public void RegisterPanel(Panel panel)
    {
        if (!panels.Contains(panel))
        {
            panels.Add(panel);
            if (defaultPanel!=null && panel!=defaultPanel)
            {
                panel.Deactivate();
            }
        }
    }

    public void UnregisterPanel(Panel panel)
    {
        if (panels.Contains(panel))
        {
            panels.Remove(panel);
        }
    }

    public void ActivatePanel(Panel panel)
    {
        panel.Activate();
    }

    public void OnPanelActivate(Panel panel)
    {
        if (panelsHistory.Contains(panel)) panelsHistory.Remove(panel);
        panelsHistory.Add(panel);

        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] == panel) continue; 
            panels[i].Deactivate();
        }
    }

    public void OnPanelDeactivate(Panel panel)
    {
        if (panelsHistory.Contains(panel))
            panelsHistory.Remove(panel);

        if (panelsHistory.Count > 0)
        {
            panelsHistory[panelsHistory.Count - 1].Activate();
        } 
        else if(defaultPanel != null)
        {
            defaultPanel.Activate();
        }
    } 
}
