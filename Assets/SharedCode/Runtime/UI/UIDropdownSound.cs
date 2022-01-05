using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDropdownSound : MonoBehaviour { 
    public string optionClickSoundKey = "BtnClick";

    Dropdown _dd;
    Dropdown dd
    {
        get
        {
            if (_dd == null)
            {
                _dd = GetComponent<Dropdown>();
            }
            return _dd;
        }
    }

    void Awake()
    {
        dd.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(int i)
    {
        AudioPlayer.PlaySFX(optionClickSoundKey);
    }
}
