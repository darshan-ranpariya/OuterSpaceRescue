using UnityEngine;
using System.Collections;

public class GameObjectEvents : MonoBehaviour {
    public event System.Action AwakeEvent;
    public event System.Action StartEvent;
    public event System.Action EnableEvent;
    public event System.Action DelayedEnableEvent;
    public event System.Action DisableEvent;

    void Awake () {
        if (AwakeEvent != null)
            AwakeEvent();
    }

    void Start () {
        if (StartEvent != null)
            StartEvent();
    }

    void OnEnable () {
        if (EnableEvent != null)
            EnableEvent();

        StartCoroutine(DelayedOnEnable());
    }

    IEnumerator DelayedOnEnable()
    {
        yield return null;
        if (DelayedEnableEvent != null)
            DelayedEnableEvent();
    }

    void OnDisable () {
        if (DisableEvent != null)
            DisableEvent();
    }
}
