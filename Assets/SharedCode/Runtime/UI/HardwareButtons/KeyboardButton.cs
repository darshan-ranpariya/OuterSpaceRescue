using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] 
public class KeyboardButton : MonoBehaviour
{
    Button _btn;
    Button btn
    {
        get
        {
            if (_btn == null) _btn = GetComponent<Button>();
            return _btn;
        }
    }

    public KeyCode[] keys;
    bool clicked;

    void OnEnable()
    {
        clicked = false;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            if (!clicked)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (!Input.GetKey(keys[i])) return;
                }
                print("Clicked "+name);
                btn.onClick.Invoke();
                clicked = true;
            }
        }
        else clicked = false;
    }
}
