using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChildToggler : MonoBehaviour
{ 
    public void ToggleRandom()
    {
        ToggleAtIndex(Random.Range(0, transform.childCount));
    }

    public void ToggleAtIndex(int i)
    {
        i = Mathf.Clamp(i, 0, transform.childCount);
        for (int c = 0; c < transform.childCount; c++) transform.GetChild(c).gameObject.SetActive(false);
        if(i>=0 && i<transform.childCount) transform.GetChild(i).gameObject.SetActive(true);
    }

    public void ToggleNextWraped()
    {
        int ii = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                for (int c = 0; c < transform.childCount; c++) transform.GetChild(c).gameObject.SetActive(false);
                i = (i + 1) % transform.childCount;
                transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
    }

    public void ToggleChild(Transform t)
    {
        if (t.parent == transform)
        {
            for (int c = 0; c < transform.childCount; c++)
            { 
                transform.GetChild(c).gameObject.SetActive(false);
            }
            t.gameObject.SetActive(true); 
        } 
    }

    public void ToggleAll()
    {
        if (transform.childCount > 0)
        {
            ToggleAll(!transform.GetChild(0).gameObject.activeSelf);
        }
    }
    public void ToggleAll(bool active)
    {
        for (int c = 0; c < transform.childCount; c++) transform.GetChild(c).gameObject.SetActive(active);
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(ChildToggler))]
public class ChildTogglerEditor : Editor
{
    ChildToggler script;
    public int  activeChildIndex = 0;
    string[] names = new string[0];
    void OnEnable() {
        script = target as ChildToggler;
        activeChildIndex = script.transform.childCount;
        names = new string[script.transform.childCount+1];
        for (int i = 0; i < script.transform.childCount; i++)
        {
            names[i] = script.transform.GetChild(i).name;
            if (script.transform.GetChild(i).gameObject.activeSelf)
            {
                activeChildIndex = i; 
            }
        }
        names[names.Length - 1] = "None";
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();   
        activeChildIndex = GUILayout.SelectionGrid(activeChildIndex, names, 1);   
        if (GUI.changed)
        {
            script.ToggleAtIndex(activeChildIndex);
        }
    }
}

#endif