using UnityEngine;
using System.Collections;

public class NetworkStrengthIndicator : MonoBehaviour
{
    public Transform indicatorsParent;
    public float lastPingTime;

    void OnEnable()
    { 
        //print (Mathf.Atan (1)*Mathf.Rad2Deg);
        if (indicatorsParent == null)
        {
            Debug.Log("Assign Indicator's parent", gameObject);
            return;
        }
        if (indicatorsParent.childCount < 2)
        {
            Debug.Log("Indicator's parent has to have at least 2 children/indicators", gameObject);
            return;
        }
        IndicateSpeedLevel(10);
        StartCoroutine("Ping_c");
    }

    void OnDisable()
    {
        StopCoroutine("Ping_c");
    }

    IEnumerator Ping_c()
    {
        float t = 0;
        while (true)
        {
            t = Time.realtimeSinceStartup;
            WWW ping = new WWW("http://www.google.com");
            yield return ping;
            if (!string.IsNullOrEmpty(ping.error))
            {
                IndicateSpeedLevel(999);
                lastPingTime = 999;
                //Logs.Add.Info("Last ping Failed: " + ping.error);
            }
            else
            {
                lastPingTime = Time.realtimeSinceStartup - t;
                IndicateSpeedLevel(lastPingTime); 
                //Logs.Add.Info("Last ping: " + lastPingTime.ToString());
            } 
            yield return new WaitForSeconds(5);
        }
    }

    int ci;
    //child index
    float m = 2;
    //max acceptable ping time in seconds
    void IndicateSpeedLevel(float pingTime)
    {
        ci = Mathf.Clamp((int)(pingTime / (m / indicatorsParent.childCount)), 0, indicatorsParent.childCount - 1);
        SetObjectActive(ci); 
    }

    void SetObjectActive(int childIndex)
    {
        for (int i = 0; i < indicatorsParent.childCount; i++)
        {
            indicatorsParent.GetChild(i).gameObject.SetActive(false);
        }
        indicatorsParent.GetChild(childIndex).gameObject.SetActive(true);
    }

    //bool CheckForInternetConnection()
    //{
    //    System.Net.WebClient client;
    //    System.IO.Stream stream;
    //    try
    //    {
    //        client = new System.Net.WebClient();
    //        stream = client.OpenRead("http://www.google.com");
    //        return true;
    //    }
    //    catch (System.Exception ex)
    //    {
    //        return false;
    //    }
    //}

    public float testSpeed;

    void test()
    {
        IndicateSpeedLevel(testSpeed);
    }
}
