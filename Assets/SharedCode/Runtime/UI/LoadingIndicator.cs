using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class LoadingIndicator : MonoBehaviour
{
    public static LoadingIndicator StaticInstance;
    public bool staticInstance;

    public GameObject indicator;
    public Text text;
    internal List<string> pendingsKey = new List<string>(); 
    internal List<string> pendingsMsg = new List<string>(); 

    void OnEnable()
    {
        if (staticInstance)
        {
            StaticInstance = this;
        }
    }

    public void AddProccessSingle(string key, string msg = "HIDDEN")
    {
        if (!string.IsNullOrEmpty(key) && !pendingsKey.Contains(key))
        {
            AddProccess(key, msg);
        }
    }

    public void AddProccess(string key, string msg = "HIDDEN")
    {
        Debug.Log("Add Process "+key);
        if (string.IsNullOrEmpty(key)) return;
        pendingsKey.Add(key);
        if (string.IsNullOrEmpty(msg)) msg = key;
        pendingsMsg.Add(msg);
        indicator.SetActive(true);
        UpdateText();
    }  

    public void RemoveProccess(string key)
    {
        Debug.Log("Remove Process " + key);
        if (!string.IsNullOrEmpty(key) && pendingsKey.Contains(key))
        {
            int i = pendingsKey.IndexOf(key);
            pendingsKey.RemoveAt(i);
            pendingsMsg.RemoveAt(i);
            UpdateText();
        }
        
        if (pendingsKey.Count == 0)
        {
            indicator.SetActive(false);
        }

    }

    public void UpdateText()
    {
        StringBuilder sb = new StringBuilder();
        //for (int i = 0; i < pendingsMsg.Count; i++)
        //{
        //    if (!pendingsMsg[i].Equals("HIDDEN"))
        //    {
        //        sb.Append(pendingsMsg[i]);
        //        sb.Append("\n");
        //    }
        //}
        if (text) text.text = sb.ToString();
    }
}
