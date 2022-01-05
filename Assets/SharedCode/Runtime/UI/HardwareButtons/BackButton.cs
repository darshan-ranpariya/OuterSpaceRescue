using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Simple script that registers an UI Button's onClick to BackButtonHandler's queue on enable and unregisters on disable
public class BackButton : MonoBehaviour
{  
    IPointerClickHandler[] comps = new IPointerClickHandler[0];

    void OnEnable() {
        comps = GetComponents<IPointerClickHandler>();
        BackButtonHandler.Register(InvokeIPointerClickHandlers);  
    }

    void OnDisable()
    {
        BackButtonHandler.Remove(InvokeIPointerClickHandlers); 
    }

    void InvokeIPointerClickHandlers()
    { 
        for (int i = 0; i < comps.Length; i++)
        {
            comps[i].OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
}
