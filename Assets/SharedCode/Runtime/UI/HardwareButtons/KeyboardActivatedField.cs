using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))] 
public class KeyboardActivatedField : MonoBehaviour
{
    InputField _inputField;
    InputField inputField
    {
        get
        {
            if (_inputField == null) _inputField = GetComponent<InputField>();
            return _inputField;
        }
    }

    public KeyCode[] keys; 
    bool activated;

    void OnEnable()
    {
        activated = false;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            if (!activated)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (!Input.GetKey(keys[i])) return;
                }
                print("Activated " + name);
                inputField.ActivateInputField();
                activated = true;
            }
        }
        else
        {
            activated = false;
        }
    }
}
