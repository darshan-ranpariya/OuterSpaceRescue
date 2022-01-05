using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Toast : MonoBehaviour
{
    public Text msgText;
    public TextMeshProUGUI msgTextPro;

    static Toast _instance;
    static Toast instance
    {
        get
        {
            if (_instance == null)
            {
                FindObjectOfType<Toast>().GetComponent<Toast>().init();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    static Transform elements;  
    float displayTime = 2;

    void OnEnable()
    {
        init();
    }

    public void init()
    {
        instance = this;
        elements = transform.GetChild(0); 
        elements.gameObject.SetActive(false); 
    }

    void Test()
    {
        Show("Initialized");
    }

    public static void Show(string msg)
    {
        instance.ShowIns(msg, instance.displayTime, false, elements.position);
    }

    public static void Show(string msg, Vector3 position)
    {
        instance.ShowIns(msg, instance.displayTime, true, position);
    } 

    public static void Show(string msg, float time, Vector3 position)
    {
        instance.ShowIns(msg, time, true, position);
    } 

    void ShowIns(string msg, float time, bool customPos, Vector3 position)
    { 
        if(msgText) msgText.text = msg;
        if(msgTextPro) msgTextPro.text = msg;

        Transform tElements = elements.Duplicate(); 
        tElements.localScale = Vector3.one;
        tElements.gameObject.SetActive(true);  

        new Interpolate.Position(tElements, position, position + new Vector3(0, 50*elements.lossyScale.x, 0), .6f); 

        Delayed.Function<Transform>((g) => {
            new Interpolate.Scale(g, Vector3.one, Vector3.zero, .2f); 
        },tElements, time); 

        Delayed.Function<GameObject>((g) => {
            Destroy(g); 
        },tElements.gameObject, time+.3f);
    } 
}
