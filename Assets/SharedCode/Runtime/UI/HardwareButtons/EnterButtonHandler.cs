using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EnterButtonHandler : MonoBehaviour
{
    public class ButtonAction
    {
        public Button uiButton = null;
        public UnityEvent unityEvent = null;
        public System.Action function = null;

        public void Invoke()
        {
            if (uiButton != null) uiButton.onClick.Invoke();
            else if (unityEvent != null) unityEvent.Invoke();
            else if (function != null) function();
        }
    }

    static List<ButtonAction> registeredActions = new List<ButtonAction>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (registeredActions.Count == 0) return;
#if UNITY_EDITOR
            ButtonAction ba = registeredActions[registeredActions.Count - 1];
            GameObject go = null;
            if (ba.uiButton != null) go = ba.uiButton.gameObject;
            string s = "";
            if (ba.uiButton != null) s = ba.uiButton.name;
            else if (ba.unityEvent != null) s = ba.unityEvent.GetPersistentMethodName(0);
            else if (ba.function != null) s = ba.function.Method.Name;
            Debug.Log(s, go);
#endif
            registeredActions[registeredActions.Count - 1].Invoke();
        }
    }

    public static void Register(Button uiButton)
    {
        ButtonAction nba = new ButtonAction();
        nba.uiButton = uiButton;
        registeredActions.Add(nba);
    }

    public static void Remove(Button uiButton)
    {
        for (int i = 0; i < registeredActions.Count; i++)
        {
            if (registeredActions[i].uiButton == uiButton)
            {
                registeredActions.RemoveAt(i);
                break;
            }
        }
    }

    public static void Register(UnityEvent unityEvent)
    {
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
}
