using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationEditorHelper : MonoBehaviour {

    public LocalizedText[] allTexts;
    public Text[] allTextsText;
    public string searchKey;
    public string searchValue;
    public List<LocalizedText> searchedTexts = new List<LocalizedText>();
    public void FindTextComps ()
    {
        //allTexts = FindObjectsOfType<LocalizedText>();
        allTextsText = new Text[allTexts.Length];
        for (int i = 0; i < allTextsText.Length; i++)
        {
            allTextsText[i] = allTexts[i].GetComponent<Text>();
        }
    } 
    public void FindKeyComps()
    {
        searchKey = searchKey.ToLower();
        searchedTexts.Clear();
        for (int i = 0; i < allTexts.Length; i++)
        {
            if (allTexts[i].keys[0].key.ToLower().Contains(searchKey))
            {
                searchedTexts.Add(allTexts[i]);
            }
        }
    } 
    public void FindValueComps()
    {
        searchValue = searchValue.ToLower();
        searchedTexts.Clear();
        for (int i = 0; i < allTexts.Length; i++)
        {
            //Debug.LogFormat("{1} : {0}", allTextsText[i].text, searchValue);
            if (allTextsText[i].text.ToLower().Contains(searchValue))
            {
                searchedTexts.Add(allTexts[i]);
            }
        }
    }
}
