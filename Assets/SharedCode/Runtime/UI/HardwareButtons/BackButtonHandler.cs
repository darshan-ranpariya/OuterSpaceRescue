using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

//This class handles hardware back button behaviour
//One instance of this is required to read the hardware input
//You can add or remove any action at any time to invoke with back button (there are three ways to do it, System.Action/UnityEvent/UI.Button)
//Though only the the last action is invoked and is NOT removed automatically, you'll have to manually unregister the action
public class BackButtonHandler : MonoBehaviour {
    public class ButtonAction
    {
        public Button uiButton = null;
        public UnityEvent unityEvent = null;
        public System.Action function = null; 

        public void Invoke() {
            if (uiButton != null) uiButton.onClick.Invoke();
            else if (unityEvent != null) unityEvent.Invoke();
            else if (function != null) function();
        }
    }

    static List<ButtonAction> registeredActions = new List<ButtonAction>();

    void Awake(){
        RegisterDefault();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            ButtonAction ba = registeredActions[registeredActions.Count - 1];
            GameObject go = null;
            if (ba.uiButton != null) go = ba.uiButton.gameObject;
            string s = "";  
            if (ba.uiButton != null) s = ba.uiButton.name;
            else if (ba.unityEvent != null) s = ba.unityEvent.GetPersistentMethodName(0);
            else if (ba.function != null) s = ba.function.Method.Name;
            //Debug.Log(s, go); 
            #endif
            registeredActions[registeredActions.Count - 1].Invoke();
        }
    }
    
    public static void Register(Button uiButton)
    { 
        RegisterDefault();
        ButtonAction nba = new ButtonAction();
        nba.uiButton = uiButton;
        registeredActions.Add(nba); 
    }

    public static void Remove(Button uiButton)
    {
        for (int i = 0; i < registeredActions.Count; i++)
        {
            if (registeredActions[i].uiButton==uiButton)
            {
                registeredActions.RemoveAt(i);
                break;
            }
        }
    }

    public static void Register(UnityEvent unityEvent)
    {  
        RegisterDefault();
        ButtonAction nba = new ButtonAction();
        nba.unityEvent = unityEvent;
        registeredActions.Add(nba);
    }

    public static void Remove(UnityEvent unityEvent)
    {
        for (int i = 0; i < registeredActions.Count; i++)
        {
            if (registeredActions[i].unityEvent == unityEvent)
            {
                registeredActions.RemoveAt(i);
                break;
            }
        }
    }

    /// <summary>
    /// Never pass a lambda expression here, since you won't be able to remove this registration later.
    /// </summary>
    /// <param name="function"></param>
    public static void Register(System.Action function)
    {
        RegisterDefault();
        ButtonAction nba = new ButtonAction();
        nba.function = function;
        registeredActions.Add(nba);
    }

    public static void Remove(System.Action function)
    {
        for (int i = 0; i < registeredActions.Count; i++)
        {
            if (registeredActions[i].function == function)
            {
                registeredActions.RemoveAt(i);
                break;
            }
        }
    }

    static bool quitAssigned = false;
    static void RegisterDefault()
    {
        if (!quitAssigned)
        { 
            registeredActions.Add(new ButtonAction(){function = Application.Quit});
            quitAssigned = true;
        }
    }
}
