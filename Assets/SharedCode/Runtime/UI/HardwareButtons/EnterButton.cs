using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EnterButton : MonoBehaviour {
    Button _btn;
    Button btn
    {
        get
        {
            if (_btn == null) _btn = GetComponent<Button>();
            return _btn;
        }
    }

    void OnEnable()
    {
        EnterButtonHandler.Register(btn);
    }

    void OnDisable()
    {
        EnterButtonHandler.Remove(btn);
    }
}
