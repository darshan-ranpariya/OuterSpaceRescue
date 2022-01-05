using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ThemeResource<T> : ScriptableObject
{
    public event Action Updated;
    public T m_value; 
    public T value
    {
        get { return m_value; }
        set
        {
            m_value = value;
            if (Updated != null) Updated();
        }
    }
}

public abstract class ThemeResourcePreset<T, TResource> : MonoBehaviour where TResource : ThemeResource<T>
{
    public TResource resource;
    public T value;
    public bool applyOnEnable = true;

    void OnEnable()
    {
        if (applyOnEnable)
        {
            Apply();
        }
    }

    public virtual void Apply()
    {
        resource.value = value;
    }
}

public abstract class ThemeResourceUser<T, TResource> : MonoBehaviour where TResource : ThemeResource<T>
{
    public TResource resource; 
    protected void OnEnable()
    {
        if (resource!=null)
        {
            resource.Updated += OnAssetUpdated;
        }
        OnAssetUpdated();
    }

    protected void OnDisable()
    {
        if (resource != null)
        {
            resource.Updated -= OnAssetUpdated;
        }
    }

    protected void OnValidate()
    {
    }

    public abstract void OnAssetUpdated();
}

public abstract class ThemeResourceUserSingleComponent<T, TResource, TComp> : ThemeResourceUser<T, TResource> where TResource : ThemeResource<T> where TComp : Component
{
    public TComp target;
    protected void OnValidate()
    { 
        if (target == null)
        {
            target = GetComponent<TComp>();
        }
    }

    public abstract override void OnAssetUpdated();
}