using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using SimpleJSON;

public class TimeZoneData : MonoBehaviour
{
    public class FormattingHelp
    {
        //DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7, 123);

        //String.Format("{0:y yy yyy yyyy}",      dt);  // "8 08 008 2008"   year
        //String.Format("{0:M MM MMM MMMM}",      dt);  // "3 03 Mar March"  month
        //String.Format("{0:d dd ddd dddd}",      dt);  // "9 09 Sun Sunday" day
        //String.Format("{0:h hh H HH}",          dt);  // "4 04 16 16"      hour 12/24
        //String.Format("{0:m mm}",               dt);  // "5 05"            minute
        //String.Format("{0:s ss}",               dt);  // "7 07"            second
        //String.Format("{0:f ff fff ffff}",      dt);  // "1 12 123 1230"   sec.fraction
        //String.Format("{0:F FF FFF FFFF}",      dt);  // "1 12 123 123"    without zeroes
        //String.Format("{0:t tt}",               dt);  // "P PM"            A.M. or P.M.
        //String.Format("{0:z zz zzz}",           dt);  // "-6 -06 -06:00"   time zone

        //// month/day numbers without/with leading zeroes
        //String.Format("{0:M/d/yyyy}",           dt);  // "3/9/2008"
        //String.Format("{0:MM/dd/yyyy}",         dt);  // "03/09/2008"

        //// day/month names
        //String.Format("{0:ddd, MMM d, yyyy}",   dt);  // "Sun, Mar 9, 2008"
        //String.Format("{0:dddd, MMMM d, yyyy}", dt);  // "Sunday, March 9, 2008"

        //// two/four digit year
        //String.Format("{0:MM/dd/yy}",           dt);  // "03/09/08"
        //String.Format("{0:MM/dd/yyyy}",         dt);  // "03/09/2008"
    }

    [Serializable]
    public class AllZonesData
    {
        public List<string> names = new List<string>();
        public List<int> offsetMinutes = new List<int>();
    }

    static bool init = false;

    public static ObservableVariable<string> currentTimeZone = new ObservableVariable<string>();
    public static ObservableVariable<TimeSpan> currentTimeZoneOffset = new ObservableVariable<TimeSpan>();
    //public static int currentTimeZoneId
    //{
    //    get { return UserStatics.TimeZoneId.Value; }
    //    set { UserStatics.TimeZoneId.Value = value; }
    //} 

    public TextAsset allZonesJson;
    public static AllZonesData allZones = new AllZonesData();

    //public ServerConnection serverConnection;

    static DateTime defDateTime = new DateTime();
    static string defTimeZone = "Automatic";
    static TimeSpan _defTimeZoneOffset = TimeSpan.FromSeconds(99);
    static TimeSpan defTimeZoneOffset
    {
        get
        {
            if (_defTimeZoneOffset.TotalSeconds == 99)
            {
                //Toast.Show(_defTimeZoneOffset.TotalMinutes.ToString());
                Debug.Log(_defTimeZoneOffset.TotalMinutes);
                _defTimeZoneOffset = DateTime.Now.RoundToNearest(TimeSpan.FromMinutes(1)).Subtract(DateTime.UtcNow.RoundToNearest(TimeSpan.FromMinutes(1)));
            }
            return _defTimeZoneOffset;
        }
    }

    bool systemOffsetFixedByServer;

    public static string dateTimeFormat = "";//GameStatics.dateTimeFormat;
    public static string dateFormat = "d MMM yyyy";
    public static string timeFormat = "h:mm tt";

    void OnEnable()
    {
        LocalTCPMessanger.Received += LocalTCPMessageReceived;
        //if (serverConnection != null) serverConnection.ServerMessageReceived += ServerConnection_ServerMessageReceived;
        StartCoroutine("FSO_c");
    }

    void OnDisable()
    {
        LocalTCPMessanger.Received -= LocalTCPMessageReceived;
        //if (serverConnection != null) serverConnection.ServerMessageReceived -= ServerConnection_ServerMessageReceived;
    }

    private void LocalTCPMessageReceived(string obj)
    {
        //if (obj.Equals("TZUp"))
        //{
        //    UserStatics.TimeZoneId.initialised = false;
        //    SetCurrentTimeZone(UserStatics.TimeZoneId.Value);
        //}
    }

    void Awake()
    {
        if (!init)
        {
            init = true;

            if (allZonesJson != null)
            {
                allZones = JsonUtility.FromJson<AllZonesData>(allZonesJson.text);
                allZones.names.Insert(0, defTimeZone);
                allZones.offsetMinutes.Insert(0, (int)defTimeZoneOffset.TotalMinutes);
            }

            new RestAPI.Get("http://ip-api.com/json", OnApiResponse);
        }

        //SetCurrentTimeZone(UserStatics.TimeZoneId.Value);
    }

    public static void SetCurrentTimeZone(int id)
    {
        //currentTimeZoneId = id;
        try
        {
            //currentTimeZone.Value = allZones.names[currentTimeZoneId];
            //DateTimeExt.LocalTimeOffsetSpan = TimeSpan.FromMinutes(allZones.offsetMinutes[currentTimeZoneId]);
            currentTimeZoneOffset.Value = DateTimeExt.LocalTimeOffsetSpan;
        }
        catch
        {
            currentTimeZone.Value = defTimeZone;
            DateTimeExt.LocalTimeOffsetSpan = defTimeZoneOffset;
            currentTimeZoneOffset.Value = DateTimeExt.LocalTimeOffsetSpan;
        }

        LocalTCPMessanger.Send("TZUp");
    }


    //private void ServerConnection_ServerMessageReceived(string arg1, string arg2, Sfs2X.Entities.Data.SFSObject arg3)
    //{
    //    if (arg1.Equals("GetUTCTime"))
    //    {
    //        try
    //        {
    //            DateTimeExt.systemOffsetSpan = DateTime.UtcNow.Subtract(DateTime.Parse(arg3.GetUtfStringSafe("utcTime")));
    //            systemOffsetFixedByServer = true;
    //        }
    //        catch { }
    //    }
    //}
    class res
    {
        public string currentFileTime;
    }
    IEnumerator FSO_c()
    {
        WWW w = new WWW("http://worldclockapi.com/api/json/utc/now");
        System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        yield return w;
        if (!string.IsNullOrEmpty(w.error)) yield break;
        if (systemOffsetFixedByServer) yield break;
        //UnityEngine.Debug.LogFormat("MonoRestCall {0}. Seconds took {1}", name, TimeSpan.FromTicks(sw.ElapsedTicks).TotalSeconds);
        //UnityEngine.Debug.LogFormat("MonoRestCall {0}. Result {1}", name, w.text);
        res r = JsonUtility.FromJson<res>(w.text);
        DateTime realLocalTime = DateTime.FromFileTime(long.Parse(r.currentFileTime)).Add(TimeSpan.FromTicks(sw.ElapsedTicks / 2));
        //UnityEngine.Debug.Log(realLocalTime.ToString());
        DateTimeExt.systemOffsetSpan = DateTime.Now.Subtract(realLocalTime);
        UnityEngine.Debug.Log("SYSTEM CLOCK OFFSET".WithColorTag(Color.green) + DateTimeExt.systemOffsetSpan.TotalSeconds);
    }


    public static DateTime ParseGMTToLocal(ref string gmtTime)
    {
        try
        {
            gmtTime = gmtTime.Replace('T', ' ');
            gmtTime = gmtTime.Replace("Z", "");
            return DateTime.Parse(gmtTime).Add(currentTimeZoneOffset.Value);
        }
        catch
        {
            Logs.Add.Info(string.Format("Time: {0} could not be parsed.", gmtTime));
            return defDateTime;
        }
    }

    static void OnApiResponse(string json)
    {
        JSONNode rootNode = JSONNode.Parse(json);
        try
        {
            defTimeZone = string.Format
                (
                "(GMT{0}{1}:{2:00}) {3}",
                (defTimeZoneOffset.TotalMinutes > 0 ? "+" : ""),
                Math.Floor(defTimeZoneOffset.TotalHours),
                Math.Ceiling((double)defTimeZoneOffset.Minutes),
                rootNode["timezone"].Value
                );

            allZones.names[0] = defTimeZone;
            //if (currentTimeZoneId == 0) currentTimeZone.Value = defTimeZone;
            print(defTimeZone);
        }
        catch { }
    }
}
