using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoDateTimeExtBase : MonoBehaviour
{
    public MonoDateTime refDateTime;
    public virtual void OnEnableBase() { }
    public virtual void OnDisableBase() { }

    void OnEnable()
    {
        if (refDateTime != null)
        {
            refDateTime.ValChanged += OnValChange;
            OnValChange();
        }
        OnEnableBase();
    }

    void OnDisable()
    {
        if (refDateTime != null)
        {
            refDateTime.ValChanged -= OnValChange; 
        }
        OnDisableBase();
    }

    public void SetRefDateTime(MonoDateTime _refDateTime)
    {
        if (refDateTime != null) refDateTime.ValChanged -= OnValChange;
        if (_refDateTime != null)
        {
            refDateTime = _refDateTime;
            refDateTime.ValChanged += OnValChange;
            OnValChange();
        }
    }
     
    public abstract void OnValChange();
}
