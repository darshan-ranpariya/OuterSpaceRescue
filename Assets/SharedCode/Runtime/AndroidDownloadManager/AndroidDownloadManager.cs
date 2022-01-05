using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AndroidDownloadManager : MonoBehaviour
{
    static AndroidJavaObject _currentActivity;
    static AndroidJavaObject currentActivity
    {
        get
        {
            if (_currentActivity == null)
            {
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _currentActivity;
        }
    }

    static AndroidJavaClass _pluginClass = null;
    static AndroidJavaClass pluginClass
    {
        get
        {
            if (_pluginClass == null)
            {
                _pluginClass = new AndroidJavaClass("com.girish.androiddownloadmanager.DownloadManagerWrapper");
            }
            return _pluginClass;
        }
    }

    public static long StartDownload(string url, string filename)
    {
        return pluginClass.CallStatic<long>("StartDownload", url, filename, currentActivity);
    }

    public static string GetStatus(long id)
    {
        return pluginClass.CallStatic<string>("GetStatus", id, currentActivity);
    }

    public static long GetDownloadedSize(long id)
    {
        return pluginClass.CallStatic<long>("GetDownloadedSize", id, currentActivity);
    }

    public static long GetFileSize(long id)
    {
        return pluginClass.CallStatic<long>("GetFileSize", id, currentActivity);
    }

    public static string GetFileUri(long id)
    {
        return pluginClass.CallStatic<string>("GetFileUri", id, currentActivity);
    }

    public static void OpenFile(long id)
    {
        pluginClass.CallStatic("OpenFile", id, currentActivity);
    }

    //public Text statusText;
    //public long downloadId = 0;
    //int i = 0;
    //IEnumerator Start ()
    //{
    //    yield return new WaitForSeconds(1);
    //    try
    //    {
    //        downloadId = StartDownload("https://i.redd.it/j9skozzkyv401.png", "image.png"); 
    //    }
    //    catch (System.Exception e)
    //    {
    //        statusText.text = e.Message;
    //        yield break;
    //    }
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1);
    //        i++;
    //        statusText.text = GetStatus(downloadId) + i.ToString();
    //        if (statusText.text.Contains("SUCCESS"))
    //        {
    //            try
    //            {
    //                statusText.text = GetFileUri(downloadId); 
    //            }
    //            catch (System.Exception e)
    //            {
    //                statusText.text = e.Message;
    //            }
    //            yield return new WaitForSeconds(1);
    //            try
    //            {
    //                OpenFile(downloadId);
    //            }
    //            catch (System.Exception e)
    //            {
    //                statusText.text += "\n" + e.Message;
    //            }
    //            yield break;
    //        }
    //    }
    //}
}