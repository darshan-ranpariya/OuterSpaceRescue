using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DynamicOption
{
    public int id = 0;
    public string key = "";
    public string name = "";
}

//public abstract class DynamicSelection<T> : MonoBehaviour where T : DynamicSelectionOption
public class DynamicOptions : MonoBehaviour 
{ 
    public event Action SelectionChanged;
    public event Action<DynamicOptions> SelectionChangedThis;
    public event Action OptionsUpdated;
    public event Action<bool> LoadingOptions;

    public bool loadOptionsOnEnable;
    public bool loadOptionsOnConnection;
    public int selectedIndex = -1;
    protected DynamicOption emptyOption = new DynamicOption();
    public DynamicOption selectedOption
    {
        get
        {
            if (selectedIndex < options.Count && selectedIndex >= 0) return options[selectedIndex];
            else return emptyOption;
        }
    } 

    public List<DynamicOption> defaultOptions = new List<DynamicOption>();
    public List<DynamicOption> options = new List<DynamicOption>(); 

    protected void OnEnable()
    {
        PreInit(); 
        if (loadOptionsOnEnable) LoadOptions();
        //if (loadOptionsOnConnection) SFS.OnRoomJoined += OnRoomJoined;
        if (options.Count == 0)
        {
            AddDefaultOptions();
            Select(0);
        }
    }
    protected void OnDisable()
    {
        //if (loadOptionsOnConnection) SFS.OnRoomJoined -= OnRoomJoined;
    }

    //private void OnRoomJoined(Sfs2X.Entities.Room room)
    //{
    //    LoadOptions(); 
    //}

    protected virtual void PreInit() { } 

    public void Select(int index)
    {
        //print(name +  " Selecting " + index);

        if (options.Count < 1) return;

        index = Mathf.Clamp(index, 0, options.Count-1);
        if (selectedIndex != index)
        {
            selectedIndex = index;
            if (SelectionChanged != null) SelectionChanged();
            if (SelectionChangedThis != null) SelectionChangedThis(this);
        }

        //print(name + " Selected " + index);
    }

    public void Select(string keyword)
    {
        int s = options.FindIndex((o) => { return o.key.ToLower().Contains(keyword.ToLower()); });
        if (s < 0) return;
        if (selectedIndex != s)
        {
            selectedIndex = s;
            if (SelectionChanged != null) SelectionChanged();
            if (SelectionChangedThis != null) SelectionChangedThis(this); 
        }
    }

    protected void FireOptionsUpdated()
    {
        if (OptionsUpdated != null) OptionsUpdated();
    }
    protected void FireLoadingOptions(bool b)
    {
        if (LoadingOptions != null) LoadingOptions(b);
    } 

    protected void AddDefaultOptions(bool fireUpdate = true)
    {
        for (int i = 0; i < defaultOptions.Count; i++)
        {
            options.Add(defaultOptions[i]);
        }
        if(fireUpdate) FireOptionsUpdated();
    }

    protected void LoadOptions()
    {
        if (GetAdditionalOptions())
        {
            if (LoadingOptions != null) LoadingOptions(true);
        }
    }

    protected virtual bool GetAdditionalOptions() { return false; }

    protected void SetAdditionalOptions(List<DynamicOption> opts)
    {
        if (opts==null)
        {
            if (LoadingOptions != null) LoadingOptions(false);
            return;
        }

        options.Clear();
        for (int i = 0; i < defaultOptions.Count; i++)
        {
            options.Add(defaultOptions[i]);
        }
        for (int i = 0; i < opts.Count; i++)
        {
            options.Add(opts[i]);
        }

        if (selectedIndex == -1 && options.Count > 0) Select(0);

        if (LoadingOptions != null) LoadingOptions(false);
        if (OptionsUpdated != null) OptionsUpdated();
    }
}
