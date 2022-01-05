using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class VersionText : MonoBehaviour
{
    Text t;
    TextMeshProUGUI tmPro;

    void Awake()
    {
        t = GetComponent<Text>();
        tmPro = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (t) t.text = Application.version;
        if (tmPro) tmPro.text = Application.version;
    }
}
