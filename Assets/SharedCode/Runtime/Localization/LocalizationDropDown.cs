using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class LocalizationDropDown : MonoBehaviour {
    Dropdown ddComp;

    void OnEnable()
    {
        if (ddComp == null) ddComp = GetComponent<Dropdown>(); 
        Localization.NewLanguageAdded += UpdateDD;
        Localization.CurrentLanguageUpdated += UpdateDD;
        ddComp.onValueChanged.AddListener(ddValueChanged);
        UpdateDD();
    }

    void OnDisable()
    {
        Localization.NewLanguageAdded -= UpdateDD;
        Localization.CurrentLanguageUpdated -= UpdateDD;
        ddComp.onValueChanged.RemoveAllListeners();
    }

    void UpdateDD()
    {
        ddComp.ClearOptions();
        List<string> languageNames = new List<string>();
        int ddVal = 0;
        for (int i = 0; i < Localization.instance.setup.availableLanguages.languagesData.Count; i++)
        {
            languageNames.Add(Localization.instance.setup.availableLanguages.languagesData[i].name);
            if (Localization.instance.setup.availableLanguages.languagesData[i].name.Equals(Localization.instance.setup.availableLanguages.prefferedLanguageName)) ddVal = i;
        }
        ddComp.AddOptions(languageNames);

        ddComp.onValueChanged.RemoveAllListeners();
        ddComp.value = ddVal;
        ddComp.onValueChanged.AddListener(ddValueChanged);
    }

    public void ddValueChanged(int val)
    {
        Localization.SetCurrentLanguageManual(ddComp.options[ddComp.value].text);
        //Localization.UpdateCurrentLanguage();
    }
}
