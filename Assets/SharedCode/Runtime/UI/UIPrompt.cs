using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPrompt : MonoBehaviour
{
    public Panel panel; 
 
    public bool active
    {
        get
        {
            if (panel != null) return panel.isActive;
            else return gameObject.activeSelf;
        }
    }

    //public string title;
    //public string msg;
    public string[] btnsActionText;
    public System.Action[] btnsAction; 

    public Text titleText;
    public Text msgText;
    public uString title;
    public uString msg;
    public Transform buttonsParent; 

    void OnEnable()
    {
        //Dismiss();
    } 

    public void Activate(string _title, string _msg, string btnActionText, System.Action btnAction)
    { 
        title.Value = _title;
        msg.Value = _msg;
        btnsAction = new System.Action[1];
        btnsAction[0] = btnAction;
        btnsActionText = new string[1];
        btnsActionText[0] = btnActionText;
        Show();
    }

    public void Activate(string _title, string _msg, string btn1ActionText, System.Action btn1Action, string btn2ActionText, System.Action btn2Action)
    {
        title.Value = _title;
        msg.Value = _msg;
        btnsAction = new System.Action[2];
        btnsAction[0] = btn1Action;
        btnsAction[1] = btn2Action;
        btnsActionText = new string[2];
        btnsActionText[0] = btn1ActionText;
        btnsActionText[1] = btn2ActionText;
        Show();
    }

    void Show()
    {
        if (panel!=null) panel.Activate();
        else gameObject.SetActive(true);

        if (btnsActionText.Length == 1)
        {
            try
            {
                buttonsParent.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = btnsActionText[0];
            }
            catch
            {
                buttonsParent.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = btnsActionText[0];
            }
            buttonsParent.GetChild(0).gameObject.SetActive(true);
            buttonsParent.GetChild(1).gameObject.SetActive(false);
        }

        if (btnsActionText.Length == 2)
        {
            try
            {
                buttonsParent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = btnsActionText[0];
                buttonsParent.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = btnsActionText[1];
            }
            catch
            {
                buttonsParent.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = btnsActionText[0];
                buttonsParent.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = btnsActionText[1]; 
            } 
            buttonsParent.GetChild(1).gameObject.SetActive(true);
            buttonsParent.GetChild(0).gameObject.SetActive(false);
        }

        if (titleText) titleText.text = title.Value;
        if (msgText) msgText.text = msg.Value;
         
    }

    public void Dismiss()
    {
        if (panel != null) panel.Deactivate();
        else gameObject.SetActive(false);
    }

    public void BtnPressed(int indx)
    {
        if (btnsAction[indx] != null) btnsAction[indx]();
        Dismiss();
    }
}
