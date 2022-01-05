using UnityEngine; 
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {
    [System.Serializable]
    public class LocalizedString
    {
        public string key = "";     
        public bool capitalize; 

        public override string ToString()
        {
            if(capitalize) return Localization.GetString(key).ToUpper();
            else return Localization.GetString(key);
        }
    }

    public Text textComp;
    public TMPro.TextMeshProUGUI textProComp;
    public string textFormat = "{0}"; 
    public string textFormatKey = ""; 
    public LocalizedString[] keys = new LocalizedString[]{new LocalizedString()};
    string fallbackText = string.Empty;

    public bool force;


    void OnEnable()
    {
        if (textComp == null && textProComp == null)
        {
            textComp = GetComponent<Text>();
            textProComp = GetComponent<TMPro.TextMeshProUGUI>();
        }
        if (textComp == null && textProComp == null) return;

        Localization.CurrentLanguageUpdated += UpdateText;
        if (string.IsNullOrEmpty(fallbackText))
        {
            if (textComp != null) fallbackText = textComp.text;
            if (textProComp != null) fallbackText = textProComp.text;
        }
        UpdateText();
        //TBA
        new Delayed.Action(() =>{
            if (textComp!=null && string.IsNullOrEmpty(textComp.text))
            {
                UpdateText();
            }
            if (textProComp != null && string.IsNullOrEmpty(textProComp.text))
            {
                UpdateText();
            }
        }, 1);

        //if (!GameStatics.DesktopMode)
        //{
        //    UnityEngine.UI.Extensions.LetterSpacing spacing = GetComponent<UnityEngine.UI.Extensions.LetterSpacing>();
        //    if (spacing != null) spacing.enabled = false;
        //    if(textComp!=null) textComp.fontStyle = FontStyle.Normal; 
        //    //if (spacing == null)
        //    //{
        //    //    spacing = gameObject.AddComponent<UnityEngine.UI.Extensions.LetterSpacing>();
        //    //}
        //    ////spacing.spacing = 11;

        //    //UnityEngine.UI.Shadow shadow = GetComponent<UnityEngine.UI.Shadow>();
        //    //if (shadow!=null)
        //    //{
        //    //    shadow.enabled = false;
        //    //    UnityEngine.UI.Shadow newShadow = GetComponent<UnityEngine.UI.Shadow>();
        //    //    newShadow = gameObject.AddComponent<UnityEngine.UI.Shadow>();
        //    //    newShadow.effectColor = shadow.effectColor;
        //    //    newShadow.effectDistance = shadow.effectDistance;
        //    //}
        //}

        if (force)
        {
            new Delayed.Action(UpdateText, 1);
        }
    }

    void OnDisable()
    {
        Localization.CurrentLanguageUpdated -= UpdateText;
    }

    public void SetKey(string _key, string _fallback)
    {
        textFormatKey = string.Empty;
        textFormat = "{0}";
        keys = new LocalizedString[] { new LocalizedString() };
        keys[0].key = _key;
        fallbackText = _fallback;
        UpdateText();
    }

    public void SetFormatKey(string _formatKey, string _fallbackFormat, string _fallbackText, params string[] _keys)
    {
        if (Localization.HasString(_formatKey))
        {
            textFormatKey = _formatKey;
            textFormat = string.Empty;
        }
        else
        {
            textFormatKey = string.Empty;
            textFormat = _fallbackFormat;
        }

        keys = new LocalizedString[_keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = new LocalizedString();
            keys[i].key = _keys[i];
        }
        fallbackText = _fallbackText;
        UpdateText();
    }

    public void SetFormat(string _format, string _fallbackText, params string[] _keys)
    {
        textFormatKey = string.Empty;
        textFormat = _format;
        LocalizedString[] keys = new LocalizedString[_keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].key = _keys[i];
        }
        fallbackText = _fallbackText;
        UpdateText();
    }

    void UpdateText()
    { 
        if (!string.IsNullOrEmpty(textFormatKey))
        {
            textFormat = Localization.GetString(textFormatKey, fallbackText);
        }
        for (int i = 0; i < keys.Length; i++)
        {
            if (!Localization.HasString(keys[i].key))
            {
                if (textComp != null) textComp.text = fallbackText;
                if (textProComp != null) textProComp.text = fallbackText;
                return;
            }
        }
        if (keys != null && keys.Length > 0 && !string.IsNullOrEmpty(textFormat))
        {
            if (textComp != null) textComp.text = string.Format(textFormat, keys);
            if (textProComp != null) textProComp.text = string.Format(textFormat, keys);
        }

        //string f = textFormatKey;
        //if(string.IsNullOrEmpty(f)) f = textFormat;

        //string[] sa = new string[keys.Length];
        //for (int i = 0; i < sa.Length; i++)
        //{
        //    sa[i] = keys[i].key;
        //}

        //textComp.text = Localization.GetStringFormat(f, sa, fallbackText);
    }
}


