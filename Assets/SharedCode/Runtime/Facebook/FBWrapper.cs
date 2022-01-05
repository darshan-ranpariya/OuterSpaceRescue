//#define FB_SDK

using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
#if FB_SDK
using Facebook.Unity;
#endif

public class FBWrapper : MonoBehaviour
{
    public class UserDetails
    {
        public string id = "";
        public string name = "";
        public string email = "";
        public string firstName = "";
        public string lastName = "";
        public string avatarUrl = "";
        public string token = "";
    } 

    public static UserDetails user = null;

    public static bool initialized;
    static System.Action initializeSuccessCallback, initializeFailureCallback; 
    static System.Action loginSuccessCallback, loginFailureCallback;
    static System.Action userDetailsSuccessCallback, userDetailsFailureCallback;


    public void InitTest()
    {
        Initialize(() => { Toast.Show("success"); }, () => { Toast.Show("failure"); });
    }

    public static void Initialize(System.Action successCallback = null, System.Action failureCallback = null)
    {
#if !UNITY_ANDROID
        if(failureCallback!=null) failureCallback();
        return;
#endif
#if FB_SDK
        Logs.Add.Info("FB: Initializing");
        if (initialized) return;
        if (initializeSuccessCallback != null) return;
        FB.Init(OnInitSuccess);
        initializeSuccessCallback = successCallback;
        initializeFailureCallback = failureCallback;
#else
        if (failureCallback != null) failureCallback();
        return;
#endif
    }

    static void OnInitSuccess()
    {
#if FB_SDK
        initialized = FB.IsInitialized;
        Logs.Add.Info("FB: Initialized: "+ initialized);
        FB.ActivateApp();
        if(initialized) if(initializeSuccessCallback!=null) initializeSuccessCallback();
        else if (initializeFailureCallback != null) initializeFailureCallback();
        initializeSuccessCallback = null;
        initializeFailureCallback = null;
#endif
    }



    public void LoginTest()
    {
        Login(()=> { Toast.Show("success"); }, ()=> { Toast.Show("failure"); });
    }

    public static void Login(System.Action successCallback = null, System.Action failureCallback = null)
    {
#if FB_SDK
        Logs.Add.Info("FB: Logging in...");
        if (!initialized)
        {
            Logs.Add.Error("FB: SDK not initialized, can not login!");
            return;
        }

        loginSuccessCallback = successCallback;
        loginFailureCallback = failureCallback;

        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
#else
        if (failureCallback != null) failureCallback(); 
#endif
    }

#if FB_SDK
    static void AuthCallback(ILoginResult result) {
        if (FB.IsLoggedIn)
        {
            Logs.Add.Info("FB: Login success!");
            if (loginSuccessCallback != null) loginSuccessCallback();
        }
        else
        {
            Logs.Add.Error("FB: Login Failed!");
            if (loginFailureCallback != null) loginFailureCallback();
        }
        loginSuccessCallback = null;
        loginFailureCallback = null;
    }
#endif



    public static void GetUserDetails(System.Action successCallback = null, System.Action failureCallback = null)
    {
#if FB_SDK
        Logs.Add.Info("FB: Getting user details...");
        if (!initialized)
        {
            Logs.Add.Error("FB: SDK not initialized, can not GetUserDetails!");
            return;
        }
        if (!FB.IsLoggedIn)
        {
            Logs.Add.Error("FB: No user logged in, can not GetUserDetails!");
            return;
        }

        userDetailsSuccessCallback = successCallback;
        userDetailsFailureCallback = failureCallback;

        FB.API("me?fields=id,name,email,first_name,last_name,picture", HttpMethod.GET, OnUserDetailsResponse);
#else
        if (failureCallback != null) failureCallback();
#endif
    }

#if FB_SDK
    static void OnUserDetailsResponse(IGraphResult graphResult) {
        Logs.Add.Info("FB: user details recieved.\n" + graphResult.RawResult);

        string error = "";
         
        if (!string.IsNullOrEmpty(graphResult.Error)) error = graphResult.Error;
        else {
            string[] reqDetails = new string[] {"id","name","email"};
            for (int i = 0; i < reqDetails.Length; i++)
            {
                if (!graphResult.ResultDictionary.ContainsKey(reqDetails[i]))
                {
                    error = reqDetails[i] + " not received.";
                    break;
                }
            }
        }
         

        if (string.IsNullOrEmpty(error))
        {
            user = new UserDetails();
            user.id = graphResult.ResultDictionary["id"].ToString();
            user.name = graphResult.ResultDictionary["name"].ToString();
            user.email = graphResult.ResultDictionary["email"].ToString();


            if (graphResult.ResultDictionary.ContainsKey("first_name"))
                user.firstName = graphResult.ResultDictionary["first_name"].ToString();
            else
                user.firstName = user.name.Split(' ')[0];

            if (graphResult.ResultDictionary.ContainsKey("last_name"))
                user.lastName = graphResult.ResultDictionary["last_name"].ToString();
            else
                if(user.name.Split(' ').Length>1) user.firstName = user.name.Split(' ')[1];

            if (graphResult.ResultDictionary.ContainsKey("picture"))
            {
                Dictionary<string, object> picDict = (Dictionary<string, object>)graphResult.ResultDictionary["picture"];
                Dictionary<string, object> picDict2 = (Dictionary<string, object>)picDict["data"];
                user.avatarUrl = picDict2["url"].ToString();
                //Logs.Add.Info("fb avatar: "+user.avatarUrl);
            } 
            else
                user.avatarUrl = "";


            if (userDetailsSuccessCallback != null) userDetailsSuccessCallback();
        }
        else
        {
            Logs.Add.Error("FB: Failed to get user details. Error: " + error);
            if (userDetailsFailureCallback != null) userDetailsFailureCallback();
        }


        userDetailsSuccessCallback = null;
        userDetailsFailureCallback = null;
    } 
#endif


    static System.Action feedShareSuccessCallback = null;
    static System.Action<string> feedShareFailureCallback = null;
    public static void FeedShare(string link, string title, string caption, string description, System.Action successCallback = null, System.Action<string> failureCallback = null)
    {
#if FB_SDK
        feedShareSuccessCallback = successCallback;
        feedShareFailureCallback = failureCallback;
        FB.FeedShare("", new System.Uri(link), title, caption, description, null, "", OnFeedShare);
#else
        if (failureCallback != null) failureCallback("Not Available");
#endif
    }

#if FB_SDK
    static void OnFeedShare(IShareResult shareResult) 
    {
        if (string.IsNullOrEmpty(shareResult.Error))
        {
            if (feedShareFailureCallback != null)
                feedShareFailureCallback(shareResult.Error);
        }
        else
        {
            if (feedShareSuccessCallback != null)
                feedShareSuccessCallback();
        }

        feedShareSuccessCallback = null;
        feedShareFailureCallback = null;
    }
#endif
}
