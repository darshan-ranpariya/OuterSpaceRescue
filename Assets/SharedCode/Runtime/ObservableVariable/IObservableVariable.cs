using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public interface IObservableVariable<T>
{
    event Action Updated; 
    T Value { get; set; }
}


[Serializable]
public class ObservableVariable<T> : IObservableVariable<T>
{
    public event Action Updated;

    T _Value = default(T);
    public T Value
    {
        get
        {
            return _Value;
        }
        set
        {
            if (_Value == null || !_Value.Equals(value))
            {
                _Value = value;
                if (Updated != null) Updated();
            }   
        }
    }
}

public interface IObservableList<T>
{
    event Action Updated;
    event Action<int, T> ItemAdded;
    event Action<int, T> ItemUpdated;
    event Action<int, T> ItemRemoved;

    void UpdateList(List<T> refList);
    void ClearList();
    void AddItem(T item);
    void InsertItem(T item, int i);
    void UpdateItem(T item, int i);
    void RemoveItem(T item);
    void RemoveItem(int i);
    T GetItem(int i);
    int GetItemsCount();
}
 
public class ObservableList<T> : IObservableList<T>
{
    List<T> list = new List<T>();  

    public event Action Updated; //when entire list is updated/changed
    public event Action<int, T> ItemAdded;
    public event Action<int, T> ItemRemoved;
    public event Action<int, T> ItemUpdated;

    public void UpdateList(List<T> refList)
    {
        list.Clear();
        if(refList != null) list.AddRange(refList);
        if (Updated != null) Updated();
    }

    public void ClearList()
    {
        list.Clear();
        if (Updated != null) Updated();
    }

    public void AddItem(T item)
    {
        list.Add(item);
        if (ItemAdded != null) ItemAdded(list.Count-1, item);
    }

    public void InsertItem(T item, int i)
    {
        list.Insert(i, item);
        if (ItemAdded != null) ItemAdded(i, item);
    }

    public void UpdateItem(T item, int i)
    {
        list[i] = item;
        if (ItemAdded != null) ItemAdded(i, item);
    }

    public void RemoveItem(T item)
    {
        int indexBefore = list.IndexOf(item);
        list.Remove(item);
        if (ItemRemoved != null) ItemRemoved(indexBefore, item);
    }

    public void RemoveItem(int i)
    {
        T removedItem = list[i];
        list.RemoveAt(i);
        if (ItemRemoved != null) ItemRemoved(i, removedItem);
    }

    public T GetItem(int i)
    {
        return list[i];
    }

    public int GetItemsCount()
    {
        return list.Count;
    }
}

[Serializable]
public class ObservableString : ObservableVariable<string> { }

[Serializable]
public class ObservableStringRef
{
    public ObservableString refVar;
    public MonoBehaviour varScript;
    public string refVarName = string.Empty;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ObservableStringRef))]
public class ObservableStringDrawer : PropertyDrawer
{
    SerializedProperty sp;
    ObservableStringRef target;
    List<FieldInfo> fields = new List<FieldInfo>();
    string[] vars = new string[] { "a", "b" };
    int so;
    int s;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Application.isPlaying) return;
        if (target == null || sp != property)
        {
            sp = property;
            target = fieldInfo.GetValue(property.serializedObject.targetObject) as ObservableStringRef;
            if (target != null)
            {
                if (target.varScript != null)
                {
                    fields.Clear();
                    fields.AddRange(target.varScript.GetType().GetFields());
                    //Debug.Log("fields");
                    //for (int i = fields.Count - 1; i >= 0; i--) Debug.Log(fields[i].FieldType);
                    for (int i = fields.Count - 1; i >= 0; i--)
                    {
                        if (!fields[i].FieldType.Equals(typeof(ObservableString)))
                        {
                            //Debug.Log("removed" + fields[i].Name);
                            fields.RemoveAt(i);
                        }
                    }
                    vars = new string[fields.Count];
                    for (int i = 0; i < fields.Count; i++)
                    {
                        vars[i] = fields[i].Name;
                        if (vars[i].Equals(target.refVarName)) s = i;
                    }
                }
            }
        } 
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, position.width / 2, position.height);
        var unitRect = new Rect(position.x + position.width / 2, position.y, position.width / 2 , position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("varScript"), GUIContent.none);
        s = EditorGUI.Popup(unitRect, s, vars);
        if (so != s || target.refVar == null)
        { 
                target.refVarName = fields[s].Name;
                target.refVar = (ObservableString)(fields[s].GetValue(target.varScript));
                Debug.Log(target.refVar); 
        }
        so = s;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif