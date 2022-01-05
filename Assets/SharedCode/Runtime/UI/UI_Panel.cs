using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UI_Panel : MonoBehaviour {
    public int defaultPanelIndex = 1;
    public Transform excludedItem;
	void OnEnable(){  
        if(defaultPanelIndex>0) ActivateMenuItem(defaultPanelIndex);
	}
 
	public void ActivateMenuItem(int itemIndex)
    {  
        for(int i = 1 ; i < gameObject.transform.childCount ; i++)
        { 
            if(transform.GetChild(i) != excludedItem) transform.GetChild(i).gameObject.SetActive(false);  
		}  
		if (itemIndex > 0 && itemIndex < transform.childCount)
		{
            if(transform.GetChild(itemIndex) != excludedItem) transform.GetChild(itemIndex).gameObject.SetActive(true); 
		}
		else Debug.LogError("Transform Child Out Of Bounds"); 
	}  
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(UI_Panel))]
public class UI_PanelEditor : Editor
{
    UI_Panel script;
    public int  activeChildIndex = -1; 
    string[] names = new string[0];
    void OnEnable() {
        script = target as UI_Panel;
        if(script.excludedItem == null) names = new string[script.transform.childCount-1];
        else names = new string[script.transform.childCount-2];
        for (int i = 1; i < script.transform.childCount; i++)
        {
            if (!(script.excludedItem != null && script.transform.GetChild(i) == script.excludedItem))
            {
                names[i-1] = script.transform.GetChild(i).name; 
                if (script.transform.GetChild(i).gameObject.activeSelf)
                {
                    activeChildIndex = i-1;
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();    
        activeChildIndex = GUILayout.SelectionGrid(activeChildIndex, names, 1);  
        if (GUI.changed)
        {
            script.ActivateMenuItem(activeChildIndex+1);
            Selection.activeGameObject = script.transform.GetChild(activeChildIndex + 1).gameObject;
        }
    }
}

#endif