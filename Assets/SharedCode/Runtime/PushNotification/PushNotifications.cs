//#define FCM_SDK_INSTALLED

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushNotifications
{
    [Serializable]
    public class DeviceData
    {
        public bool enabled = false;
        public string token = string.Empty;
    }

    public class NotificationData
    {
        public string title = string.Empty;
        public string body = string.Empty;
        public IDictionary<string, string> data = new Dictionary<string, string>();
    }


    private static string tokenFileName = "pnotf";
    private static DeviceData deviceData;  

    static bool initialized;
#if FCM_SDK_INSTALLED
    static Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
#endif
    static System.Action<bool> initCallback;
    public static void Initialize(System.Action<bool> callback)
    { 
#if !UNITY_ANDROID
        if (callback != null) callback(false);
        return;
#endif

#if FCM_SDK_INSTALLED
        initCallback = callback;

        deviceData = LocalData.Load<DeviceData>(tokenFileName);
        if (deviceData == null) deviceData = new DeviceData();

        dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
        if (dependencyStatus != Firebase.DependencyStatus.Available)
        {
            Firebase.FirebaseApp.FixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    SendInitCallback(true);
                }
                else
                {
                    SendInitCallback(false, "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
        else
        {
            SendInitCallback(true);
        }
#else
        if (callback != null) callback(false);
        return;
#endif

    }

    static void SendInitCallback(bool success, string msg = "")
    {
        if (success)
        {
            Debug.Log("Firebase: Initialized");
        }
        else
        {
            Debug.LogError("Firebase: " + msg);
        }
        if (initCallback != null) initCallback(success);
        initCallback = null;
        initialized = success;
    }


    public static bool notificationsEnabled {
        get
        {
            if (!initialized) return false;
            return deviceData.enabled;
        }
        set
        {
#if FCM_SDK_INSTALLED
            if (!initialized) return;
            deviceData.enabled = value;
            LocalData.Save(tokenFileName, deviceData);
#endif
        }
    }
    public static bool SetNotifications(bool set)
    {
#if FCM_SDK_INSTALLED
        Debug.Log("Firebase: SetNotifications " + set);

        if (!initialized) return false;
        if (notificationsEnabled == set) return false;

        if (set) AssignEvents();
        else DeassignEvents();

        notificationsEnabled = set;
        Debug.Log("Called From Set Notification");
        UpdateTokenOnServer();
        
        return true;
#else
        return false;
#endif
    }

    static void UpdateTokenOnServer()
    {
#if FCM_SDK_INSTALLED
        if (deviceData.token.IsNullOrEmpty()) return;
        Debug.Log("Firebase: UpdateTokenOnServer " + deviceData.token);
        SFSObject objOut = new SFSObject();
        objOut.PutInt("userId", UserStatics.id);
        objOut.PutUtfString("deviceTokenId", notificationsEnabled ? deviceData.token : string.Empty);
        SFS.SendExtensionRequest("ActivateDeactivateNotification", objOut);
#endif
    }

    public static bool eventsAssigned;
    static void AssignEvents()
    {
#if FCM_SDK_INSTALLED
        if (eventsAssigned) return;

        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;

        eventsAssigned = true;
#endif
    }

    static void DeassignEvents()
    {
#if FCM_SDK_INSTALLED
        if (!eventsAssigned) return;

        Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;

        eventsAssigned = false;
#endif
    }

    public static event System.Action<NotificationData> NotificationReceived;
#if FCM_SDK_INSTALLED
    static void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        //return;
        Logs.Add.Info(string.Format("Firebase: Message Rceived: {0} :: {1}", e.Message.Notification.Title, e.Message.Notification.Body));
        if (NotificationReceived != null)
            NotificationReceived(
                new NotificationData()
                {
                    title = e.Message.Notification.Title,
                    body = e.Message.Notification.Body,
                    data = e.Message.Data,
                });
        //Popup.Show(e.Message.Notification.Title, e.Message.Notification.Body, "Ok", Popup.Dismiss);

        //        DebugLog("Received a new message");
        //        var notification = e.Message.Notification;
        //        if (notification != null) {
        //            DebugLog("title: " + notification.Title);
        //            DebugLog("body: " + notification.Body);
        //        }
        //        if (e.Message.From.Length > 0)
        //            DebugLog("from: " + e.Message.From);
        //        if (e.Message.Data.Count > 0) {
        //            DebugLog("data:");
        //            foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
        //                e.Message.Data) {
        //                DebugLog("  " + iter.Key + ": " + iter.Value);
        //            }
        //        }
    }

    static void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs t)
    {
        //return;
        Debug.Log("Firebase: Received Registration Token: " + t.Token);
        if (!deviceData.token.Equals(t.Token))
        {
            deviceData.token = t.Token;
            Debug.Log("Called From OnTokenReceived");
            if (notificationsEnabled) UpdateTokenOnServer();
        }
    }
#endif
}

