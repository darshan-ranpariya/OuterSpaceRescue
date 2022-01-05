using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiCall : MonoBehaviour
{
    public static ApiCall inst;

    private void Awake()
    {
        inst = this;
    }

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://52.172.51.202");
        //WWW www = new WWW("http://www.triangulargamestudio.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public void CallApi(bool isWin)
    {
        Time.timeScale = 0;
        StartCoroutine(checkInternetConnection((isConnected) => {
            // handle connection status here
            if (isConnected)
            {
                print("connected");
                StartCoroutine(Upload(isWin));
            }
            else
            {
                if (isWin)
                {
                    UIManager.inst.winPanel.gameObject.SetActive(true);
                }
                else
                {
                    UIManager.inst.gameOverPanel.gameObject.SetActive(true);
                }
            }
        }));
    }

    IEnumerator Upload(bool isWin)
    {

        WWWForm form = new WWWForm();
        form.AddField("customer_id", UserStatics.customer_id);
        form.AddField("account_id", UserStatics.account_id);
        form.AddField("card_no", UserStatics.card_no);
        form.AddField("site_id", UserStatics.site_id);
        form.AddField("win_status", isWin ? 1 : 0);
        form.AddField("game_id", 3);
        print(string.Format("{0}, {1}, {2}, {3}", UserStatics.customer_id, UserStatics.account_id, UserStatics.card_no, UserStatics.site_id));
        using (UnityWebRequest www = UnityWebRequest.Post(Utility.API_URL, form))
        {
            // www.uploadHandler = uH;
            UIManager.inst.loader.SetActive(true);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIManager.inst.loader.SetActive(false);

            }
            else
            {
                print("milan");
                print(www.downloadHandler.text);

                if (www.downloadHandler.isDone && www.downloadHandler.text != null)
                {
                    UIManager.inst.loader.SetActive(false);

                    string json = www.downloadHandler.text;
                    if (json != null)
                    {
                        WebResponseData data = JsonUtility.FromJson<WebResponseData>(json);
                        if (data != null)
                        {
                            print(data.message);

                            if (data.code == 1)
                            {
                                if (data.message == "1")
                                {
                                    //win
                                    UIManager.inst.gameOverPanel.gameObject.SetActive(true);
                                    UIManager.inst.gameOverMsg.Value = "CONGRATULATIONS, YOU HAVE WON 10 PLAY CREDITS!";

                                }
                                else if (data.message == "2")
                                {
                                    //exceed
                                    UIManager.inst.limitExceedPanel.gameObject.SetActive(true);

                                }
                                else if (data.message == "3")
                                {
                                    UIManager.inst.gameOverPanel.gameObject.SetActive(true);
                                    UIManager.inst.gameOverMsg.Value = "YOU HAVE EARNED 5 PLAY CREDITS. BETTER LUCK NEXT TIME!";


                                }
                               
                            }
                            else
                            {
                                UIManager.inst.gameOverPanel.gameObject.SetActive(true);
                                UIManager.inst.gameOverMsg.Value = data.message;

                            }
                        }
                    }
                }
            }
        }
    }
    //resopnse
    /* {
    "code": 1,
    "message": "Success",
    "data": "Transaction is Successfull and your Tx Id is: 1044326"
    }*/
}
