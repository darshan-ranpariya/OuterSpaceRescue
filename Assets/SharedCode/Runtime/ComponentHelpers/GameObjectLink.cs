using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectLink : MonoBehaviour 
{
    public GameObjectEvents source;
    public GameObjectEvents target;

    void OnEnable()
    {
        UpdateTarget(); 
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    bool subbed = false;
    void Subscribe()
    {
        if (!subbed)
        {
            source.EnableEvent += UpdateTarget;
            source.DisableEvent += UpdateTarget;
            //target.EnableEvent += UpdateTarget;
            //target.DisableEvent += UpdateTarget;
            subbed = true;
        }
    }
    void Unsubscribe()
    {
        if (subbed)
        {
            source.EnableEvent -= UpdateTarget;
            source.DisableEvent -= UpdateTarget;
            //target.EnableEvent -= UpdateTarget;
            //target.DisableEvent -= UpdateTarget;
            subbed = false;
        }
    }

    void UpdateTarget()
    { 
        Unsubscribe();
        target.gameObject.SetActive(source.gameObject.activeSelf); 
        Subscribe();
    }
} 