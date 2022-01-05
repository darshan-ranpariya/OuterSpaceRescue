using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour {


    [System.Serializable]
    public class Data
    {
        public string title;
        public string msg;
        public string[] btnsActionText;
        public System.Action[] btnsAction;
    }

    static Popup _instance;
    static Popup instance {
        get {
            if (_instance==null)
            {
                FindObjectOfType<Popup>().GetComponent<Popup>().init();
            }
            return _instance;
        }
        set {
            _instance = value;
        }
    }

    public Text titleText;
    public TextMeshProUGUI titleTextPro;
    public Text msgText;
    public TextMeshProUGUI msgTextPro;
    public Transform buttonsParent;
    static Transform elements;
    static Data current;
    Interpolate.Position animPos;
    Interpolate.Scale animScale;

    void OnEnable() {
        init();
    } 

    public void init() {
        instance = this;
        elements = transform.GetChild(1);
        Dismiss();
    }

    public static void Show(string title, string msg, string btnActionText, System.Action btnAction)
    {
        current = new Data();
        current.title = title;
        current.msg = msg;
        current.btnsAction = new System.Action[1];
        current.btnsAction[0] = btnAction;
        current.btnsActionText = new string[1];
        current.btnsActionText[0] = btnActionText;
        instance.Show(current); 
    }

    public static void Show(string title, string msg, string btn1ActionText, System.Action btn1Action, string btn2ActionText, System.Action btn2Action)
    {
        current = new Data();
        current.title = title;
        current.msg = msg;
        current.btnsAction = new System.Action[2];
        current.btnsAction[0] = btn1Action;
        current.btnsAction[1] = btn2Action;
        current.btnsActionText = new string[2];
        current.btnsActionText[0] = btn1ActionText;
        current.btnsActionText[1] = btn2ActionText;
        instance.Show(current); 
    }

    void Show(Data data)
    {
        elements.gameObject.SetActive(true);

        Text t = null;
        TextMeshProUGUI tp = null;

        if (data.btnsActionText.Length==1)
        {
            t = buttonsParent.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
            tp = buttonsParent.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            if(t) t.text = current.btnsActionText[0];
            if(tp) tp.text = current.btnsActionText[0];
            buttonsParent.GetChild(0).gameObject.SetActive(true);
            buttonsParent.GetChild(1).gameObject.SetActive(false);
        }

        if (data.btnsActionText.Length == 2)
        {
            t = buttonsParent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
            tp = buttonsParent.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            if (t) t.text = current.btnsActionText[0];
            if (tp) tp.text = current.btnsActionText[0];

            t = buttonsParent.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();
            tp = buttonsParent.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            if (t) t.text = current.btnsActionText[1];
            if (tp) tp.text = current.btnsActionText[1];
             
            buttonsParent.GetChild(1).gameObject.SetActive(true);
            buttonsParent.GetChild(0).gameObject.SetActive(false);
        }

        if(titleText) titleText.text = data.title;
        if(titleTextPro) titleTextPro.text = data.title;
        if (msgText) msgText.text = data.msg;
        if (msgTextPro) msgTextPro.text = data.msg;

        //if (animPos != null) animPos.Stop();
        //animPos = new Interpolate.Position(elements, new Vector3(0, -50, 0), Vector3.zero, 1, true);
        if (animScale != null) animScale.Stop();
        animScale = new Interpolate.Scale(elements, Vector3.one*0.8f, Vector3.one, .15f);
        AudioPlayer.PlaySFX("popup");
    }  

    public static void Dismiss()
    {
        elements.gameObject.SetActive(false); 
    }

    public void BtnPressed(int indx) {
        if(current.btnsAction[indx]!=null) current.btnsAction[indx]();
    } 
}
