using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class UnityEventOnEnable : MonoBehaviour {
    //Fires the unity event on OnEnable and OnDisable
    //You can assign functions to those event
    //Saves a lot of hassle sometimes


    public UnityEvent m_enableEvent,m_disableEvent;

    void OnEnable() {
        m_enableEvent.Invoke();
    }

    void OnDisable()
    {
        m_disableEvent.Invoke();
    }
}
