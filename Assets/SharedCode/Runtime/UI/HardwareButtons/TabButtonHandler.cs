using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButtonHandler : MonoBehaviour
{
    int currentlySelectedField = -1;
    public InputField[] fields;
    public UIInputField[] UIfields;
    private void Awake()
    {
        if (UIfields == null || UIfields.Length == 0)
        {
            UIfields = new UIInputField[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                UIfields[i] = fields[i].gameObject.AddComponent<UIInputField>();
            }
        }
    }
    void OnEnable()
    {
        currentlySelectedField = -1;
        Cycle();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cycle();
        }
    }
    void Cycle()
    {
        if (UIfields.Length > 0)
        {
            currentlySelectedField = (currentlySelectedField + 1) % UIfields.Length;
            UIfields[currentlySelectedField].ActivateInputField();
        }
    }
}
