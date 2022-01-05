using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LayoutSwitcher : MonoBehaviour 
{
    [System.Serializable]
    public class ParentSwitch
    { 
        public RectTransform rectTransform;
        public Transform parent; 
        public bool refit = true;

        public void Switch()
        {
            rectTransform.SetParent(parent); 
            if (refit)
            {
                rectTransform.anchorMin = new Vector2(0, 0); 
                rectTransform.anchorMax = new Vector2(1, 1); 
                rectTransform.offsetMin = new Vector2(0, 0); 
                rectTransform.offsetMax = new Vector2(0, 0); 
            }
        } 
    }
    [System.Serializable]
    public class Config
    { 
        public string key = "config";
        public ParentSwitch[] parentSwitches = new ParentSwitch[0]; 
        public GameObject[] exclusiveObjects = new GameObject[0]; 
        public float minimumScreenSize;

        public void Activate()
        {
            for (int i = 0; i < exclusiveObjects.Length; i++)
            {
                exclusiveObjects[i].SetActive(true);
            }
            for (int i = 0; i < parentSwitches.Length; i++)
            {
                parentSwitches[i].Switch();
            }
        } 

        public void Deactivate()
        {
            for (int i = 0; i < exclusiveObjects.Length; i++)
            {
                exclusiveObjects[i].SetActive(false);
            }
        }
    }

    public Config[] configs = new Config[0];
    int currentConfigI = -1;
    public float windowSizePhysical;

    void OnEnable()
    {
        for (int i = 0; i < configs.Length; i++)
        {
            configs[i].Deactivate();
        }
        UpdateWindowSizePhysical();
        Switch();
    }
        
    #if UNITY_EDITOR || UNITY_STANDALONE
    float h,w;
    void Update()
    {
        if (h != Screen.height || w != Screen.width)
        {
            UpdateWindowSizePhysical();
            Switch();
        }
        h = Screen.height;
        w = Screen.width;
    }
    #endif

    void UpdateWindowSizePhysical()
    {
        windowSizePhysical = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2)) / Screen.dpi;
    }

    void Switch()
    {
        int bestConfigI = 0;
        for (int i = 0; i < configs.Length; i++)
        {
            if (windowSizePhysical > configs[i].minimumScreenSize)
            {
                if (configs[i].minimumScreenSize > configs[bestConfigI].minimumScreenSize)
                {
                    bestConfigI = i;
                }
            }
        }
//        print("bestConfigI " + bestConfigI);
//        print("currentConfigI " + currentConfigI);
        if (currentConfigI != bestConfigI)
        {
            configs[bestConfigI].Activate();
            if (currentConfigI >= 0)
                configs[currentConfigI].Deactivate();
            currentConfigI = bestConfigI;
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(LayoutSwitcher))]
public class LayoutSwitcherEditor : Editor
{
    LayoutSwitcher script;
    void OnEnable()
    {
        script = target as LayoutSwitcher;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        for (int i = 0; i < script.configs.Length; i++)
        { 
            if (GUILayout.Button(string.Format("Activate: {0}", script.configs[i].key)))
            { 
                for (int j = 0; j < script.configs.Length; j++)
                { 
                    if (j!=i)
                    { 
                        script.configs[j].Deactivate();
                    } 
                } 
                script.configs[i].Activate();
            } 
        } 
    }
}
#endif