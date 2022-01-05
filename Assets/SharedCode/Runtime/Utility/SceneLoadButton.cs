using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MonoBehaviour, IPointerClickHandler
{
    public string sceneName = "";
    public void OnPointerClick(PointerEventData eventData)
    {
        if (LoadingIndicator.StaticInstance != null) LoadingIndicator.StaticInstance.AddProccess("s_"+sceneName);
        SceneManager.LoadScene(sceneName);
    }
}