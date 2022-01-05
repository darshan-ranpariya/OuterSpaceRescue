using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceOnEnable : MonoBehaviour
{
    void OnEnable()
    { 
        Debug.Log("Enabled " + new System.Diagnostics.StackTrace());
    }
    void OnDisable()
    {
        Debug.Log("Disable " + new System.Diagnostics.StackTrace());
    }
}
