using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class UILayoutElement<T>
{
    public T element;
    public abstract void Apply();
}

[Serializable]
public class UILayoutRect : UILayoutElement<RectTransform>
{
    public RectTransform refElement;

    public override void Apply()
    {
        if (element == null || refElement == null) return;
        element.anchorMin = refElement.anchorMin;
        element.anchorMax = refElement.anchorMax;
        element.pivot = refElement.pivot;
        element.anchoredPosition = refElement.anchoredPosition;
        element.sizeDelta = refElement.sizeDelta;
        element.localRotation = refElement.localRotation;
        element.localScale = refElement.localScale;
    }
}
[Serializable]
public class UILayoutTextMesh : UILayoutElement<TextMeshProUGUI>
{
    public TextAlignmentOptions alignment;
    public override void Apply()
    {
        element.alignment = alignment;
    }
}
[Serializable]
public class UILayoutLayoutGroup : UILayoutElement<LayoutGroup>
{
    public TextAnchor alignment;
    public override void Apply()
    {
        element.childAlignment = alignment;
    }
}
[Serializable]
public class UILayoutHorizontalLayoutGroup : UILayoutElement<HorizontalOrVerticalLayoutGroup>
{
    public bool childForceExpandWidth;
    public bool childForceExpandHight;
    public override void Apply()
    {
        element.childForceExpandWidth = childForceExpandWidth;
        element.childForceExpandHeight = childForceExpandHight;
    }
}
//[Serializable]
//public class UILayoutSevenStudCardLayoutGroup : UILayoutElement<Poker.SevenStudCardsLayoutGroup>
//{
//    public float[] foldedSpacings = new float[7];
//    public float[] closedSpacings = new float[7];
//    public float[] openedSpacings = new float[7];
//    public override void Apply()
//    {
//        element.foldedSpacings = foldedSpacings;
//        element.closedSpacings = closedSpacings;
//        element.openedSpacings = openedSpacings;
//    }
//}
[Serializable]
public class UIGridLayoutLayoutGroup : UILayoutElement<GridLayoutGroup>
{
    public GridLayoutGroup refLayoutGroup;
    public override void Apply()
    {
        element.cellSize = refLayoutGroup.cellSize;
        element.childAlignment = refLayoutGroup.childAlignment;
        element.padding = refLayoutGroup.padding;
        element.spacing = refLayoutGroup.spacing;
        element.startCorner = refLayoutGroup.startCorner;
        element.startAxis = refLayoutGroup.startAxis;
        element.constraint = refLayoutGroup.constraint;
        element.constraintCount = refLayoutGroup.constraintCount;
    }
}


public class UILayout : MonoBehaviour
{
    [Space]
    public GameObject[] activeObjects;
    public GameObject[] hiddenObjects;

    [Space]
    public Behaviour[] activeComponents;
    public Behaviour[] hiddenComponents;

    [Space]
    public UILayoutRect[] rects;
    [Space]
    public UILayoutTextMesh[] texts;
    [Space]
    public UILayoutLayoutGroup[] layoutGroups;
    [Space]
    public UIGridLayoutLayoutGroup[] gridLayoutsGroups;
    [Space]
    public UILayoutHorizontalLayoutGroup[] horOrVerLayoutsGroups;
    [Space]
    //public UILayoutSevenStudCardLayoutGroup[] studCardLayoutsGroups;
    [Space]
    public bool activateOnEnable;

    void OnEnable()
    {
        if (activateOnEnable)
        {
            Activate();
        }
    }

    public void Activate()
    {
        for (int i = 0; i < rects.Length; i++) if (rects[i].element != null) rects[i].Apply();
        for (int i = 0; i < texts.Length; i++) if (texts[i].element != null) texts[i].Apply();
        for (int i = 0; i < layoutGroups.Length; i++) if (layoutGroups[i].element != null) layoutGroups[i].Apply();
        for (int i = 0; i < gridLayoutsGroups.Length; i++) if (gridLayoutsGroups[i].element != null) gridLayoutsGroups[i].Apply();
        for (int i = 0; i < horOrVerLayoutsGroups.Length; i++) if (horOrVerLayoutsGroups[i].element != null) horOrVerLayoutsGroups[i].Apply();
        //for (int i = 0; i < studCardLayoutsGroups.Length; i++) if (studCardLayoutsGroups[i].element != null) studCardLayoutsGroups[i].Apply();
        for (int i = 0; i < activeObjects.Length; i++) if (activeObjects[i] != null) activeObjects[i].SetActive(true);
        for (int i = 0; i < hiddenObjects.Length; i++) if (hiddenObjects[i] != null) hiddenObjects[i].SetActive(false);
        for (int i = 0; i < activeComponents.Length; i++) if (activeComponents[i] != null) activeComponents[i].enabled = true;
        for (int i = 0; i < hiddenComponents.Length; i++) if (hiddenComponents[i] != null) hiddenComponents[i].enabled = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UILayout))]
public class UILayoutEditor : Editor
{
    UILayout script;
    void OnEnable()
    {
        script = target as UILayout;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Apply"))
        {
            script.Activate();
        }
        if (GUI.changed)
        {
            //script.Activate();
        }
    }
}
#endif