using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System.Xml;
using System;

[CreateAssetMenu()]
public class LanguageSetup : ScriptableObject {
    [Serializable]
    public class LanguageData
    { 
        public string name;
        public int Id;
        public int latestVersion;
        public int downloadedVersion = 0;
        public string filePathFormat; 
        public string filePath
        {
            get
            {
                if (string.IsNullOrEmpty(filePathFormat))
                {
                    return string.Empty;
                }
                return string.Format(
                    filePathFormat, 
                    Application.streamingAssetsPath, 
                    Application.persistentDataPath,
#if UNITY_ANDROID && !UNITY_EDITOR
                    Application.persistentDataPath);
#else
                    Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "/" + Application.companyName + "/" + Application.productName);
#endif
            }
        } 
        public string downloadUrl;
    }

    string preloadPath { get { return Application.streamingAssetsPath; } }
    string downloadPath
    {
        get
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Application.persistentDataPath;
#else
            return Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "/" + Application.companyName + "/" + Application.productName;
#endif
        } 
    }

    [Serializable]
    public class AvailableLanguages
    {
        public const string fileName = "Languages";
        public string prefferedLanguageName = "";
        public List<LanguageData> languagesData = new List<LanguageData>();
        public bool manuallyChanged = false;  
    }

    public AvailableLanguages availableLanguages;
    public Dictionary<string, string> strings = new Dictionary<string, string>();
    public event Action<LanguageData> LanguageSet;

    bool debug = true;
    [NonSerialized]
    bool setup = false;

    public static string SystemLanguage
    {
        get
        {
            //return "Korean";
            return Application.systemLanguage.ToString();
        }
    }

    public void ReInitialize()
    {
        Debug.Log("LanguageSetup ReInit");
        setup = false;
        Initialize();
    }

    bool deleteOldFiles
    {
        get { return PlayerPrefs.GetInt("locFix") != 8; }
        set { PlayerPrefs.SetInt("locFix", (value ? 9 : 8)); }
    }

    public void Initialize()
    {
        if (setup) return;
        setup = true; 
        Debug.Log("LanguageSetup Init");
        Debug.Log(Application.persistentDataPath);
        Debug.Log(LocalData.GetFilePath(string.Empty));

        try
        {
            if (deleteOldFiles)
            {
                AvailableLanguages ld = LocalData.Load<AvailableLanguages>(AvailableLanguages.fileName);
                deleteOldFiles = false;
                for (int i = 0; i < ld.languagesData.Count; i++)
                {
                    if (File.Exists(ld.languagesData[i].filePath)) File.Delete(ld.languagesData[i].filePath);
                    ld.languagesData[i].downloadedVersion = -1;
                }
                if (File.Exists(LocalData.GetFilePath(AvailableLanguages.fileName))) File.Delete(LocalData.GetFilePath(AvailableLanguages.fileName));
            }
        }
        catch { }

        BetterStreamingAssets.Initialize();

        AvailableLanguages lastData = LocalData.Load<AvailableLanguages>(AvailableLanguages.fileName);
        if (lastData!=null)
        {
            Debug.Log("Cache data " + JsonUtility.ToJson(availableLanguages));
            availableLanguages = lastData;
        }
        else Debug.Log("Cache data could not be loaded");

        int setLanguageIndex = -1;
        for (int i = 0; i < availableLanguages.languagesData.Count; i++)
        {
            //Debug.LogFormat("{0} : {1}", availableLanguages.languagesData[i].name, availableLanguages.prefferedLanguageName);
            //Debug.LogFormat("{0} : {1}", availableLanguages.languagesData[i].name, SystemLanguage);
            
            if(!availableLanguages.manuallyChanged && availableLanguages.languagesData[i].name.Equals(SystemLanguage, StringComparison.CurrentCultureIgnoreCase)) setLanguageIndex = i;
    
            if (setLanguageIndex == -1)
            {
                if (availableLanguages.languagesData[i].name.Equals(availableLanguages.prefferedLanguageName, StringComparison.CurrentCultureIgnoreCase)) setLanguageIndex = i;
            }
        }
        if (setLanguageIndex == -1) setLanguageIndex = 0;
        SetLanguage(availableLanguages.languagesData[setLanguageIndex]);
    }

    public void SetLanguage(string languageName)
    {
        for (int i = 0; i < availableLanguages.languagesData.Count; i++)
        {
            if (availableLanguages.languagesData[i].name.Equals(languageName))
            {
                SetLanguage(availableLanguages.languagesData[i]);
                break;
            }
        }
    }

    public void UpdateCurrentLanguage()
    { 
        if (xmlDownload != null)
        {
            return;
        }

        for (int i = 0; i < availableLanguages.languagesData.Count; i++)
        {
            if (availableLanguages.languagesData[i].name.Equals(availableLanguages.prefferedLanguageName))
            {
                availableLanguages.languagesData[i].downloadUrl = string.Empty; 
                availableLanguages.languagesData[i].filePathFormat = string.Empty;
                SetLanguage(availableLanguages.languagesData[i]);
                break;
            }
        }
    }

    RestAPI.Get xmlDownload;
    LanguageData downloadingLanguageData;
    public void SetLanguage(LanguageData languageData)
    {
        if(debug) Logs.Add.Info("Setting Language " + languageData.name + new System.Diagnostics.StackTrace());

        if (xmlDownload != null)
        {
            xmlDownload.Cancel();
            downloadingLanguageData = null;
        }

        if (
            string.IsNullOrEmpty(languageData.downloadUrl) 
            && (string.IsNullOrEmpty(languageData.filePath) || !FileExists(languageData.filePath) || languageData.latestVersion > languageData.downloadedVersion))
        {
            Debug.LogFormat(languageData.filePath);
            Toast.Show("Downloading Language"); //Localize 
            downloadingLanguageData = languageData;
            xmlDownload = new RestAPI.Get("");//(GameStatics.GetLanguageDownloadUrl(languageData.name,"unity"), OnGetFileUrl, OnNewLanguageDownloadFailed);
            return;
        }

        if (!string.IsNullOrEmpty(languageData.downloadUrl))
        {
            downloadingLanguageData = languageData;
            xmlDownload = new RestAPI.Get(languageData.downloadUrl, OnNewLanguageDownloaded, OnNewLanguageDownloadFailed);
            return;
        }

        string xmlString = string.Empty;  
        if (!string.IsNullOrEmpty(languageData.filePath))
        {
            if (debug) Debug.Log("Loading File\n" + languageData.filePath);
            ReadFile(languageData.filePath, ref xmlString);
        }  

        if (!string.IsNullOrEmpty(xmlString))
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(xmlString);
            }
            catch (System.Exception ex)
            {
                if (debug) Logs.Add.Error("Failed to add the language: Failed to Prase\n" + ex.Message);
                for (int i = 0; i < availableLanguages.languagesData.Count; i++)
                {
                    if (LanguageSet != null && availableLanguages.languagesData[i].name.Equals(availableLanguages.prefferedLanguageName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Debug.Log(availableLanguages.languagesData[i].name);
                        LanguageSet(availableLanguages.languagesData[i]);
                        break;
                    }
                }
                Toast.Show(Localization.CommonStrings.errorMessage);
                return;
            }

            XmlNode langNode = xmlDoc.ChildNodes[1].FirstChild;
            strings.Clear();
            if (debug) Logs.Add.Info("langNodes: " + langNode.ChildNodes.Count);
            for (int i = 0; i < langNode.ChildNodes.Count; i++)
            {
                try
                {
                    //Debug.LogFormat("key: {0} ", langNode.ChildNodes[i].Attributes["id"].InnerText);
                    strings.Add(langNode.ChildNodes[i].Attributes["id"].InnerText, langNode.ChildNodes[i].InnerText);
                }
                catch (System.Exception ex)
                {
                    if (debug) Logs.Add.Info(string.Format("lang: {0}, i: {1}, ex: {2}", langNode.ChildNodes[i].Attributes["id"].InnerText, i, ex.Message));
                }
            }
            if (debug) Logs.Add.Info("strings count: " + strings.Count);

            availableLanguages.prefferedLanguageName = languageData.name;
            languageData.downloadedVersion = languageData.latestVersion;
            LocalData.Save(AvailableLanguages.fileName, availableLanguages, true);
            Debug.Log(JsonUtility.ToJson(availableLanguages));
            xmlDownload = null;

            if (LanguageSet != null) LanguageSet(languageData);
        }
        else
        {
            if (debug) Logs.Add.Error("Failed to add the language");
        }
    }

    [Serializable]
    class XmlUrlResponse
    {
        public string result;
        public string file;
    }
    private void OnGetFileUrl(string resString)
    {
        Debug.Log(resString);
        try
        {
            XmlUrlResponse res = JsonUtility.FromJson<XmlUrlResponse>(resString);
            downloadingLanguageData.downloadUrl = res.file;
            Debug.Log(res.file);
            SetLanguage(downloadingLanguageData);
        }
        catch { }
    }

    private void OnNewLanguageDownloaded(string xmlString)
    {
        try
        {
            downloadingLanguageData.filePathFormat = ("{2}/" + downloadingLanguageData.name + ".xml");

            string _dir = Path.GetDirectoryName(downloadingLanguageData.filePath);
            if (!Directory.Exists(_dir)) Directory.CreateDirectory(_dir);
            FileStream fileStream = new FileStream(downloadingLanguageData.filePath, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(xmlString);
            streamWriter.Close();
            fileStream.Close();
            fileStream.Dispose();

            downloadingLanguageData.downloadUrl = string.Empty;
            downloadingLanguageData.downloadedVersion = downloadingLanguageData.latestVersion;
            SetLanguage(downloadingLanguageData);

            downloadingLanguageData = null;
        }
        catch (Exception e)
        {
            OnNewLanguageDownloadFailed(e.Message);
        }
    }

    private void OnNewLanguageDownloadFailed(string error)
    {
        Toast.Show("Language Download Failed"); //Localize 

    }

    bool FileExists(string path)
    {
        bool b = File.Exists(path);
        if (!b)
        {
            string[] sa = path.Split('/');
            Debug.Log("FileExists" + path);
            b = BetterStreamingAssets.FileExists(sa[sa.Length - 1]);
        }
        return b;
    }
    
    void ReadFile(string path, ref string s)
    {
        try
        {
            StreamReader streamReader = new StreamReader(path);
            s = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
        }
        catch (System.Exception ex)
        {
            string[] sa = path.Split('/');
            s = BetterStreamingAssets.ReadAllText(sa[sa.Length - 1]);
        }
    }
}  
