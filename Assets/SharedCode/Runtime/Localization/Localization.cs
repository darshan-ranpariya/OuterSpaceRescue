using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System.Xml;
using System;
using System.Text;

public class Localization : MonoBehaviour
{
    public static Localization instance;
    public static event System.Action CurrentLanguageUpdated;
    public static event System.Action NewLanguageAdded;

    public LanguageSetup setup;
    static bool debug = true;
    static bool initialized;

    public static List<string> missingKeys = new List<string>();

    public enum AvailableLanguage
    {
        English, Korean, Japanese, Chinese, Espanol, Portugues, Francais, Taliano, Hindi, Deutsch, Russian, Vietnamese
    }

    public static LanguageSetup.LanguageData CurrentLanguage
    {
        get
        {
            try
            {
                LanguageSetup.LanguageData data = null;
                for (int i = 0; i < instance.setup.availableLanguages.languagesData.Count; i++)
                {
                    if (instance.setup.availableLanguages.languagesData[i].name.Equals(instance.setup.availableLanguages.prefferedLanguageName))
                    {
                        data = instance.setup.availableLanguages.languagesData[i];
                        break;
                    }
                }
                return data;
            }
            catch
            {
                return null;
            }
        }
    }

    void OnEnable()
    {
        instance = this;
        setup.LanguageSet += OnLanguageSet;
        setup.Initialize();
        for (int i = 0; i < instance.setup.availableLanguages.languagesData.Count; i++)
        {
            if (instance.setup.availableLanguages.languagesData[i].name.ToUpper().Equals("ENGLISH"))
            {
                if (instance.setup.availableLanguages.languagesData[i].latestVersion == 0)
                {
                    instance.setup.availableLanguages.languagesData[i].latestVersion++;
                    LocalData.Save(LanguageSetup.AvailableLanguages.fileName, setup.availableLanguages, true);
                }
                break;
            }
        }
        LocalTCPMessanger.Received += LocalTCPMessageReceived;
    }

    void OnDisable()
    {
        setup.LanguageSet -= OnLanguageSet;
        LocalTCPMessanger.Received -= LocalTCPMessageReceived;
    }

    private void LocalTCPMessageReceived(string obj)
    {
        if (obj.Equals("LangUp"))
        {
            setup.ReInitialize();
        }
    }

    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.L))
    //    {
    //        if (Input.GetKeyDown(KeyCode.M))
    //        {
    //            Debug.Log(missingKeys.GetDump().WithColorTag(Color.magenta));
    //        }
    //    }
    //}

    //private void OnExtensionResponseReceived(string cmd, Sfs2X.Entities.Data.SFSObject dataObject)
    //{
    //    if (cmd.Equals(SFSHandlers.GetLanguageData.Cmd))
    //    {
    //        print(LanguageSetup.SystemLanguage);

    //        SFSHandlers.GetLanguageData data = new SFSHandlers.GetLanguageData(dataObject);
    //        if (data.status)
    //        {
    //            LanguageSetup.LanguageData updateCurrentLanguage = null;

    //            print(data.language.Length.ToString().WithColorTag(Color.red));
    //            for (int i = 0; i < data.language.Length; i++)
    //            {
    //                bool old = false;

    //                for (int j = 0; j < setup.availableLanguages.languagesData.Count; j++)
    //                {
    //                    if (setup.availableLanguages.languagesData[j].name.Equals(data.language[i], StringComparison.CurrentCultureIgnoreCase))
    //                    {
    //                        print("old lang : " + data.language[i]);
    //                        print(data.version[i] + " " + setup.availableLanguages.languagesData[j].latestVersion);
    //                        old = true;

    //                        if (!setup.availableLanguages.manuallyChanged && data.language[i].Equals(LanguageSetup.SystemLanguage, StringComparison.CurrentCultureIgnoreCase))
    //                        {
    //                            updateCurrentLanguage = setup.availableLanguages.languagesData[j];
    //                        }

    //                        if (updateCurrentLanguage == null)
    //                        {
    //                            if (data.language[i].Equals(setup.availableLanguages.prefferedLanguageName, StringComparison.CurrentCultureIgnoreCase)
    //                                && data.version[i] > setup.availableLanguages.languagesData[j].downloadedVersion)
    //                            {
    //                                updateCurrentLanguage = setup.availableLanguages.languagesData[j];
    //                            }
    //                        }

    //                        setup.availableLanguages.languagesData[j].name = data.language[i];
    //                        setup.availableLanguages.languagesData[j].Id = data.languageId[i];
    //                        if (setup.availableLanguages.languagesData[j].latestVersion < data.version[i])
    //                            setup.availableLanguages.languagesData[j].latestVersion = data.version[i];
    //                        break;
    //                    }
    //                }

    //                if (!old)
    //                {
    //                    print("new lang : " + data.language[i]);
    //                    LanguageSetup.LanguageData newLangData = new LanguageSetup.LanguageData();
    //                    newLangData.name = data.language[i];
    //                    newLangData.Id = data.languageId[i];
    //                    newLangData.latestVersion = data.version[i];
    //                    setup.availableLanguages.languagesData.Add(newLangData);

    //                    if (!setup.availableLanguages.manuallyChanged && data.language[i].Equals(LanguageSetup.SystemLanguage, StringComparison.CurrentCultureIgnoreCase))
    //                    {
    //                        updateCurrentLanguage = newLangData;
    //                    }
    //                }
    //            }

    //            if (updateCurrentLanguage != null)
    //            {
    //                setup.SetLanguage(updateCurrentLanguage);
    //            }
    //            else
    //            {
    //                LocalData.Save(LanguageSetup.AvailableLanguages.fileName, setup.availableLanguages, true);
    //            }
    //        }
    //    }
    //    else if (cmd.Equals(SFSHandlers.LocalizationUpdateData.Cmd))
    //    {
    //        ForceUpdateEnglish();
    //        SFS.SendExtensionRequest(SFSHandlers.GetLanguageData.Cmd, new SFSObject());
    //        //SFSHandlers.LocalizationUpdateData data = new SFSHandlers.LocalizationUpdateData(dataObject);
    //        //if (data.langName.Equals(setup.availableLanguages.prefferedLanguageName, StringComparison.InvariantCulture))
    //        //{
    //        //    UpdateCurrentLanguage();
    //        //}
    //    }
    //}

    void ForceUpdateEnglish()
    {
        for (int i = 0; i < instance.setup.availableLanguages.languagesData.Count; i++)
        {
            if (instance.setup.availableLanguages.languagesData[i].name.ToUpper().Equals("ENGLISH"))
            {
                instance.setup.availableLanguages.languagesData[i].latestVersion++;
                LocalData.Save(LanguageSetup.AvailableLanguages.fileName, setup.availableLanguages, true);
                if (instance.setup.availableLanguages.prefferedLanguageName.Equals(instance.setup.availableLanguages.languagesData[i].name))
                {
                    SetCurrentLanguage(instance.setup.availableLanguages.prefferedLanguageName);
                }
                break;
            }
        }
    }

    internal static void SetCurrentLanguageManual(string text)
    {
        instance.setup.availableLanguages.manuallyChanged = true;
        SetCurrentLanguage(text);
    }
    internal static void SetCurrentLanguage(string text)
    {
        if (instance == null) return;

        instance.setup.SetLanguage(text);
    }

    internal static void UpdateCurrentLanguage()
    {
        if (instance == null) return;

        instance.setup.UpdateCurrentLanguage();
    }

    private void OnLanguageSet(LanguageSetup.LanguageData obj)
    {
        //Debug.Log("OnLanguageSet");
        if (CurrentLanguageUpdated != null) CurrentLanguageUpdated();
        LocalTCPMessanger.Send("LangUp");
    }

    public static bool HasString(string key)
    {
        if (instance == null) return false;

        return instance.setup.strings.ContainsKey(key);
    }

    public static string GetString(string key)
    {
        if (string.IsNullOrEmpty(key)) return string.Empty;

        if (instance == null) return GetKeyCleaned(key);

        if (instance.setup.strings.ContainsKey(key)) return instance.setup.strings[key];

        return GetKeyCleaned(key);
    }

    public static string GetString(string key, string fallbackString = "")
    {
        if (instance == null) return fallbackString;

        if (string.IsNullOrEmpty(key)) return fallbackString;

        if (instance.setup.strings.ContainsKey(key)) return instance.setup.strings[key];

        //Debug.LogFormat("Language key is missing :: {0} : {1}".WithColorTag(Color.magenta),key, fallbackString);
        //if (!string.IsNullOrEmpty(fallbackString) && !missingKeys.Contains(key)) missingKeys.Add(fallbackString);

        if (!string.IsNullOrEmpty(fallbackString)) return fallbackString;

        return key;
    }

    public static string FindString(string s)
    {
        return GetString(s.Replace(' ', '_').ToLower(), s);
    }

    //public static string GetStringFormat(string format, string[] keys, string fallback)
    //{
    //    if (instance == null) return fallback;

    //    if (!format.Contains("{") && instance.setup.strings.ContainsKey(format)) format = instance.setup.strings[format];

    //    string[] sa = new string[keys.Length];
    //    for (int i = 0; i < keys.Length; i++)
    //    {
    //        sa[i] = GetString(keys[i], "--");
    //        if (sa[i].Equals("--"))
    //        {
    //            return fallback;
    //        }
    //    }

    //    return string.Format(format, sa);
    //}

    public static string GetStringFormat(string formatKey, string formatFallback, params string[] args)
    {
        try
        {
            for (int i = 0; i < args.Length; i++) args[i] = GetString(args[i]);
            return string.Format(GetString(formatKey, formatFallback), args);
        }
        catch
        {
            return string.Format(formatFallback, args);
        }
    }

    public static string GetKeyCleaned(string key)
    {
        StringBuilder sb = new StringBuilder(key);
        bool capitalize = true;
        for (int i = 0; i < sb.Length; i++)
        {
            if (capitalize)
            {
                sb[i] = key[i].ToString().ToUpper()[0];
                capitalize = false;
            }
            if (sb[i] == '_') sb[i] = ' ';
            else if (sb[i] == '.') capitalize = true;
        }
        return sb.ToString();
    }

    public static class CommonStrings
    {
        public static string successTitle
        {
            get { return GetString("success", "Success"); }
        }

        public static string errorTitle
        {
            get { return GetString("oops", "Oops!"); }
        }

        public static string errorMessage
        {
            get { return GetString("errorOccured", "An error occured"); }
        }

        public static string ErrorMessage(string errorKey)
        {
            return GetString(errorKey, errorMessage);
        }

        public static string tryAgain
        {
            get { return GetString("plz_try_again", "Please try again"); }
        }

        public static string tryAgainLater
        {
            get { return GetString("plz_try_again", "Please try again later"); }
        }

        public static string ok
        {
            get { return GetString("ok", "Ok"); }
        }

        public static string cancel
        {
            get { return GetString("cancel", "cancel"); }
        }

        public static string yes
        {
            get { return GetString("yes", "Yes"); }
        }

        public static string no
        {
            get { return GetString("no", "No"); }
        }

        public static string all
        {
            get { return GetString("all", "All"); }
        }

        public static string noData
        {
            get { return GetString("no_data", "No Data"); }
        }

        public static string loading
        {
            get { return GetString("loading", "Loading"); }
        }

        public static string unknown
        {
            get { return GetString("unknown", "Unknown"); }
        }

        public static string exit
        {
            get { return GetString("exit", "Exit"); }
        }
    }
}
