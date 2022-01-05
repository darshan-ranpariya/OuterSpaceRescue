using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LocalData
{
    static string gameName = "/TetrixBuilder";
    static string _filesFolder;
    static string filesFolder
    {
        get
        {
#if UNITY_ANDROID
            return Application.persistentDataPath;
#else
            if (string.IsNullOrEmpty(_filesFolder))
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) _filesFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Cookies) + gameName;
                else _filesFolder = Application.dataPath;

                Logs.Add.Info("Local data loc: "+_filesFolder);
            }

            //if (string.IsNullOrEmpty(_filesFolder))
            //{
            //    _filesFolder = Application.dataPath;
            //    if (_filesFolder.Contains("Table_data"))
            //    {
            //        _filesFolder.Remove("Table_data");
            //    }
            //}

            return _filesFolder;
#endif
        }
    }

    public static string GetFilePath(string fileName, string folderPath = "")
    { 
        if(!string.IsNullOrEmpty(folderPath)) return folderPath.Replace('\\', '/') + "/" + fileName;
        return filesFolder.Replace('\\','/') + "/" + fileName;
    }

    public static bool Save(string fileName, object data, bool overwrite = false, string fileFolder = "")
    {
        if (!overwrite && File.Exists(GetFilePath(fileName, fileFolder)))
        {
            return false;
        }

        if(string.IsNullOrEmpty(fileFolder)) fileFolder = filesFolder;
        if (!Directory.Exists(fileFolder))
        {
            Directory.CreateDirectory(fileFolder);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(GetFilePath(fileName, fileFolder), FileMode.OpenOrCreate);

        bf.Serialize(file, data);
        file.Close();

        return true;
    }

    public static T Load<T>(string fileName, string fileFolder = "")
    { 
        if (File.Exists(GetFilePath(fileName, fileFolder)))
        {
            int t = 0;
            if (t < 50)
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(GetFilePath(fileName, fileFolder), FileMode.Open, FileAccess.Read, FileShare.Read);
                    T data = (T)bf.Deserialize(file);
                    file.Close();
                    file.Dispose();
                    return data;
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.Message);
                }
                t++;
            }
        }
        else
        {
            Debug.Log("File not founf : " + GetFilePath(fileName, fileFolder));
        }
        return default(T);
    }

    public static void Delete(string fileName, string fileFolder = "")
    { 
        if (File.Exists(GetFilePath(fileName, fileFolder)))
        {
            File.Delete(GetFilePath(fileName, fileFolder));
        } 
    }
}

[System.Serializable]
public abstract class LocalDataClass
{
    public abstract string fileName { get; set; }
    public void SaveFile(bool overrideOldFile = true)
    {
        LocalData.Save(fileName, this, overrideOldFile);
    }
    public void DeleteFile()
    {
        LocalData.Delete(fileName);
    }
}