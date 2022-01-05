using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedObject : MonoBehaviour
{
    public GameObjectEvents source;
    public GameObject target;

    void OnEnable()
    {
        source.EnableEvent += UpdateTarget;
        source.DisableEvent += UpdateTarget;
        UpdateTarget();
    }

    void OnDisable()
    {
        source.EnableEvent -= UpdateTarget;
        source.DisableEvent -= UpdateTarget;
    }

    void UpdateTarget()
    {
        target.SetActive(source.gameObject.activeSelf);
    }
}
