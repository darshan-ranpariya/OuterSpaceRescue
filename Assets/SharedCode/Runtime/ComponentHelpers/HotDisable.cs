using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this script to any annoying game object. 
/// Hitting the space bar will disable it. 
/// Works is editor only. 
/// Disable this component if the game object is not annoying for a while. 
/// Remove this component if the game object is not annoying anymore. 
/// </summary>
public class HotDisable : MonoBehaviour { 
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(2))
        {
            gameObject.SetActive(false);
        }
    }
#endif
}
