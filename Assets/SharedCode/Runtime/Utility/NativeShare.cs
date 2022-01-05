using UnityEngine;
using System.Collections;
using System.IO;

public class NativeShare 
{ 
    public static void ShareImage(string subject, string title, string message, string imagePath)
    {
#if UNITY_EDITOR
        Debug.LogFormat("Share_Image {0} {1} {2} {3}", subject, title, message, imagePath);
#elif UNITY_ANDROID 
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "image/*");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), title);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", imagePath);
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);

        bool fileExist = fileObject.Call<bool>("exists");
        Debug.Log("File exist : " + fileExist);
        // Attach image to intent
        if (fileExist)
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        //currentActivity.Call("startActivity", intentObject); 
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        currentActivity.Call("startActivity", jChooser);
#endif
    }  

    public static void SendSMS(string to, string msg)
    {
#if UNITY_EDITOR
        Debug.LogFormat("SendSMS:: to:{0} msg:{1}", to, msg);
#elif UNITY_ANDROID 
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_VIEW"));
 
        intentObject.Call<AndroidJavaObject>("setType", "vnd.android-dir/mms-sms");
        intentObject.Call<AndroidJavaObject>("putExtra", "address", to);
        intentObject.Call<AndroidJavaObject>("putExtra", "sms_body", msg);  
       
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity"); 
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "SMS via");
        currentActivity.Call("startActivity", jChooser);
#endif
    }

    public static void SendEmail(string to, string subject, string body)
    {
        #if UNITY_EDITOR
        Debug.LogFormat("SendSMS:: to:{0} subject:{1} body:{2}", to, subject, body);
        #elif UNITY_ANDROID 
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_SEND"));

        intentObject.Call<AndroidJavaObject>("setType", "text/html");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_EMAIL"), to);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);  
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);  

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity"); 
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Email via");
        currentActivity.Call("startActivity", jChooser);
        #endif
    }
}
