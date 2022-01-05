using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class UIListItem : MonoBehaviour {

    [System.Serializable]
    public abstract class BaseKV
    {
        public string key;
        public abstract void SetValue(object val);
        public abstract object GetValue();
    }

    [System.Serializable]
    public class StringKV : BaseKV
    { 
        public uString val;

        public override object GetValue()
        {
            return val.Value;
        }

        public override void SetValue(object _val)
        {
            val.Value = (string)_val;
        }
    }

    [System.Serializable]
    public class NumberKV : BaseKV
    { 
        public uNumber val;

        public override object GetValue()
        {
            return val.Value;
        }

        public override void SetValue(object _val)
        {
            val.Value = (double)_val;
        }
    }

    [System.Serializable]
    public class ImageKV : BaseKV
    {
        string source = "";
        public Image[] imgs;

        public override object GetValue()
        {
            return source;
        }

        public override void SetValue(object _val)
        {
            source = (string)_val;
            for (int i = 0; i < imgs.Length; i++)
            {
                new Download.Image(source, imgs[i]);
            }
        }
    }

    public StringKV[] strings;
    public NumberKV[] numbers;
    public ImageKV[] images;
    Dictionary<string, BaseKV> _vars;
    public Dictionary<string, BaseKV> vars
    {
        get { 
            if (_vars==null)
            {
                _vars = new Dictionary<string, BaseKV>();
                for (int i = 0; i < strings.Length; i++)
                {
                    _vars.Add(strings[i].key, strings[i]);
                }
                for (int i = 0; i < numbers.Length; i++)
                {
                    _vars.Add(numbers[i].key, numbers[i]);
                }
                for (int i = 0; i < images.Length; i++)
                {
                    _vars.Add(images[i].key, images[i]);
                }
            }
            return _vars;
        }
    } 

    public void UpdateItem(string[] keys, object[] vals)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (vars.ContainsKey(keys[i]))
            {
                vars[keys[i]].SetValue(vals[i]);
            }
            else {
                Logs.Add.Error("UIListItem: No var with key "+keys[i]+" found!");
            }
        }
    }
}
