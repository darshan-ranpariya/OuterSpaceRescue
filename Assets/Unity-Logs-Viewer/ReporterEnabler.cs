using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReporterEnabler : MonoBehaviour
{
    public bool forceDebug;

    private void OnEnable()
    {
        if (forceDebug) return;
        if (Debug.isDebugBuild) return;
        gameObject.SetActive(false);
    }
}
