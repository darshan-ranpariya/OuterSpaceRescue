using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class Screenshot : MonoBehaviour
{
    public enum GameName
    {
        TeamPoker,
        GuardianAngles
    }
    static bool debug = true;

    public static void Take()
    {
        gameName = GameName.TeamPoker;
        Screenshot ss = new GameObject("Screenshot").AddComponent<Screenshot>();
        DontDestroyOnLoad(ss.gameObject);
        ss.Initialize(debug);
    }

    public static void Take(GameName game)
    {
        gameName = game;
        Screenshot ss = new GameObject("Screenshot").AddComponent<Screenshot>();
        DontDestroyOnLoad(ss.gameObject);
        ss.Initialize(debug);
    }

    public static GameName gameName;

    public static string _folderName;
    public static string folderName
    {
        get
        {
            switch (gameName)
            {
                case GameName.TeamPoker:
                    _folderName = "TeamPokers";
                    break;
                case GameName.GuardianAngles:
                    _folderName = "GuardianAngles";
                    break;
            }
            return _folderName;
        }
    }

    public static string _fileNamePrefix;
    public static string fileNamePrefix
    {
        get
        {
            switch (gameName)
            {
                case GameName.TeamPoker:
                    _fileNamePrefix = "TeamPokers_";
                    break;
                case GameName.GuardianAngles:
                    _fileNamePrefix = "GuardianAngles_";
                    break;
            }
            return _fileNamePrefix;
        }
    }



    string _fileName = "";
    public string fileName
    {
        get
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                StringBuilder sb = new StringBuilder("/");
                sb.Append(fileNamePrefix);
                sb.Append(System.DateTime.Now.Year.ToString("0000"));
                sb.Append(System.DateTime.Now.Month.ToString("00"));
                sb.Append(System.DateTime.Now.Day.ToString("00"));
                sb.Append(System.DateTime.Now.Hour.ToString("00"));
                sb.Append(System.DateTime.Now.Minute.ToString("00"));
                sb.Append(System.DateTime.Now.Second.ToString("00"));
                sb.Append(System.DateTime.Now.Millisecond.ToString("0"));
                sb.Append(".png");
                _fileName = sb.ToString();
            }
            return _fileName;
        }
    }

    string _captureLocation = "";
    public string captureLocation
    {
        get
        {
            if (string.IsNullOrEmpty(_captureLocation))
            {
#if UNITY_EDITOR
                StringBuilder sb = new StringBuilder();
                string[] pdp = Application.dataPath.Split('/');
                for (int i = 0; i < pdp.Length - 1; i++)
                {
                    sb.Append(pdp[i]);
                    sb.Append("/");
                }
                _captureLocation = sb.ToString();
#elif UNITY_STANDALONE_WIN 
                _captureLocation = Application.dataPath;
#else
                _captureLocation = Application.persistentDataPath;
#endif
            }
            return _captureLocation;
        }
    }

    string _saveLocation = "";
    public string saveLocation
    {
        get
        {
            if (string.IsNullOrEmpty(_saveLocation))
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                //Debug.Log(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures), folderName));
                _saveLocation = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures), folderName);

#elif UNITY_ANDROID
                StringBuilder sl = new StringBuilder();
                string[] pdp = Application.persistentDataPath.Split('/');
                int s = 0;
                for (int i = 0; i < pdp.Length; i++)
                {
                    if (pdp[i].ToUpper().Contains("ANDROID") || pdp[i].Contains("Android"))
                    {
                        s = i - 1;
                        break;
                    }
                }
                for (int i = 0; i <= s; i++)
                {
                    sl.Append(pdp[i] + "/");
                }
                sl.Append(folderName); 
                if (debug) Debug.LogError("Folder Location: " + sl.ToString());
                _saveLocation = sl.ToString(); 

#else
                _saveLocation = Application.persistentDataPath;
#endif
            }
            return _saveLocation;
        }
    }

    void Initialize(bool _debug)
    {
        debug = _debug;
        if (debug) Debug.LogError("Save Location: " + Path.Combine(saveLocation, fileName.Substring(1)));
        //ScreenCapture.CaptureScreenshot(Path.Combine(saveLocation, fileName.Substring(1)));
        ScreenCapture.CaptureScreenshot(fileName.Substring(1));
        if (debug) Debug.Log("Taking Screenshot: " + fileName);
        StartCoroutine(ProcessFile());

#if UNITY_ANDROID && !UNITY_EDITOR
            RefreshAndroidGallery(saveLocation);
#elif UNITY_ANDROID && UNITY_EDITOR
        if (debug) Debug.Log("RefreshAndroidGallery");
#endif

        return;
    }

    IEnumerator ProcessFile()
    {
        if (File.Exists(captureLocation + fileName))
        {
            if (debug) Debug.LogFormat("Moving File: '{0}{2}' -> '{1}{2}'", captureLocation, saveLocation, fileName);

            try
            {
                if (!Directory.Exists(saveLocation))
                {
                    if (debug) Debug.LogFormat("Directory '{0}' not found, attempting to create.", saveLocation);
                    Directory.CreateDirectory(saveLocation);
                }
                File.Move(captureLocation + fileName, saveLocation + fileName);
                if (debug) Debug.Log("Moved!");
            }
            catch (System.Exception ex)
            {
                if (debug) Debug.Log(ex.Message);
                Popup.Show("Screenshot Error", ex.Message, "Ok", Popup.Dismiss);
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            RefreshAndroidGallery(saveLocation);
#elif UNITY_ANDROID && UNITY_EDITOR
            if (debug) Debug.Log("RefreshAndroidGallery");
#endif
            Toast.Show(Localization.GetString("screenshot_saved") + saveLocation);
            Destroy(gameObject);
        }
        else
        {
            yield return new WaitForSeconds(.3f);
            if (debug) Debug.Log("File not there(" + captureLocation + fileName + ") yet");
            StartCoroutine(ProcessFile());
        }
    }

    void RefreshAndroidGallery(string dir)
    {
        dir = "file://" + dir;

        try
        {
            Method1(dir);
        }
        catch (System.Exception ex1)
        {
            try
            {
                Method2(dir);
            }
            catch (System.Exception ex2)
            {
                if (debug) Debug.Log(ex1.Message + "\n" + ex2.Message);
                Popup.Show("Screenshot Error", ex1.Message + "\n" + ex2.Message, "Ok", Popup.Dismiss);
            }
        }
    }

    void Method1(string dir)
    {
        if (debug) Debug.Log("Method1 Dir: " + dir);
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_MEDIA_MOUNTED"), uriClass.CallStatic<AndroidJavaObject>("parse", dir));
        activityObject.Call("sendBroadcast", intentObject);
        if (debug) Debug.Log("Android media scanner triggered");
        AndroidJavaObject contenUriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", dir);
        intentObject.CallStatic("setData", contenUriObject);
    }

    void Method2(string dir)
    {
        if (debug) Debug.Log("Method2 Dir: " + dir);
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_MEDIA_SCANNER_SCAN_FILE"), uriClass.CallStatic<AndroidJavaObject>("parse", dir));
        activityObject.Call("sendBroadcast", intentObject);
        if (debug) Debug.Log("Android media scanner triggered");
    }

}
