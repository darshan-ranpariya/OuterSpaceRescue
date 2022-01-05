using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WebRequestData
{
    public string customer_id;
    public string card_no;
    public string account_id;
    public string site_id;
    public int win_status;
    public int game_id;

}

[System.Serializable]
public class WebResponseData
{
    public int code;
    public string message;
    public string data;
}
public class Utility : MonoBehaviour
{
    public const string customer_id = "customer_id";
    public const string card_no = "card_no";
    public const string account_id = "account_id";
    public const string site_id = "site_id";
    /// <summary>
    /// 1- if customer wins and 0 - if lose
    /// </summary>
    public const string win_status = "win_status"; 
    public const string game_id = "game_id";

    public const string win_time = "win_time";
    public const string win_score = "win_score";


      public const string API_URL = "http://52.172.51.202:8065/customer/createtxn";  // Production
  //  public const string API_URL = "http://45.76.160.104:8065/customer/createtxn";   // UAT 


    public static Dictionary<string, string> GetParametersFromURL(string url)
    {
        Dictionary<string, string> pairs = new Dictionary<string, string>();
        Uri uri = new Uri(url);
        if (uri == null)
            throw new ArgumentNullException("uri");

        if (uri.Query.Length == 0)
            return new Dictionary<string, string>();

        pairs = uri.Query.TrimStart('?')
                        .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                        .GroupBy(parts => parts[0],
                                 parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? parts[1] : ""))
                        .ToDictionary(grouping => grouping.Key,
                                      grouping => string.Join(",", grouping));
        string s= "Pairs\n";
        foreach (var pair in pairs)
        {
            s += string.Format("{0} = {1}\n", pair.Key, pair.Value);
        }
        print(s);
        return pairs;
    }
}
