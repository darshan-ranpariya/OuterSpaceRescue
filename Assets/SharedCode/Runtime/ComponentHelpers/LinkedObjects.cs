using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LinkedObjects : MonoBehaviour { 
    public GameObject[] linkedObjects;
    public bool invert;
    void OnEnable()
    {
        for (int i = 0; i < linkedObjects.Length; i++)
        {
            if(linkedObjects[i]) linkedObjects[i].SetActive(!invert);
        }
    }
    void OnDisable()
    {
        for (int i = 0; i < linkedObjects.Length; i++)
        {
            if (linkedObjects[i]) linkedObjects[i].SetActive(invert);
        }
    }
}
