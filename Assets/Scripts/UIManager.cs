using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;
    [Header("Panels")]
    public Panel startPanel;
    public Panel gamePlayPanel;
    public Panel gameOverPanel;
    public Panel pausePanel;
    public Panel winPanel;
    public Panel limitExceedPanel;
    [Header("UI Elements")]
    public double winScore;  
    public uNumber currScore;
    public uNumber highScore;
    public Image fillImg;
    public float totalTime;
    internal float currTime;
    [Header("Others")]
    public GameObject staticBg;
    public GameObject dynamicBg;
    public GameObject loader;
    public uAnimatorPlayer landAnim;
    public UISwitch[] audioToggles;
    public uString gameOverMsg;
    public uString gameWinMsg;
    [HideInInspector]
    public bool gameStarted;
    private void Awake()
    {
        inst = this;
    }

    private void OnEnable()
    {
        GetParmsAndSave();

        currScore.Value = 0;
        highScore.Value = PlayerPrefs.GetInt("HighScore", 0);
        UserStatics.isWinApiCalled = false;
        foreach (var sw in audioToggles)
        {
            sw.Set(AudioPlayer.effectsOn);
        }
        currTime = totalTime;
    }

    void ResetGame()
    {
        gameOverPanel.gameObject.SetActive(false);
        currTime = totalTime;
        fillImg.fillAmount = currTime / totalTime;
        StopCoroutine(StartTime());
        landAnim.Reset();
        currScore.Value = 0;
        gameStarted = false;
        PlayerMove.inst.ResetPos();
        dynamicBg.SetActive(false);
        staticBg.SetActive(true);
        UserStatics.isWinApiCalled = false;
        ItemSpwner.inst.RemoveAllItems();
    }


    #region ClickMethods
    public void StartBtnClick()
    {
        gamePlayPanel.Activate();
    }

    public void PauseBtnClick()
    {
        ItemSpwner.inst.StopCoroutine(ItemSpwner.inst.SpwanItems());
        StopCoroutine(StartTime());
        pausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeBtnClick()
    {
        Time.timeScale = 1;
        pausePanel.gameObject.SetActive(false);
    }

    public void HomeBtnClick()
    {

        Application.OpenURL("http://52.172.51.202/gamelist.html?customer_id=" + UserStatics.customer_id + "&card_no=" + UserStatics.card_no + "&account_id=" + UserStatics.account_id + "&site_id=" + UserStatics.site_id);
        //Application.OpenURL("http://www.triangulargamestudio.com/cardList/gamelist.html?customer_id=" + UserStatics.customer_id + "&card_no=" + UserStatics.card_no + "&account_id=" + UserStatics.account_id + "&site_id=" + UserStatics.site_id);
        Application.Quit();

      //  SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
      //  Time.timeScale = 1;
    }

    public void RestartBtnClick()
    {
        ResetGame();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        //StartBtnClick();
    }
    public void LimitPanalCloseBtnClick()
    {
        limitExceedPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void WinCloseBtnClick()
    {
        winPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void ResetTimeScale()
    {
        Time.timeScale = 1;
    }
    public void SoundToggle()
    {
        AudioPlayer.effectsOn = !AudioPlayer.effectsOn;
    }

    #endregion

    public void StartMoveObjects()
    {
        gameStarted = true;
        dynamicBg.SetActive(true);
        staticBg.SetActive(false);
        landAnim.Play();
        ItemSpwner.inst.StartSpwan();
        StartCoroutine(StartTime());
    }

    IEnumerator StartTime()
    {
        while (gameStarted && currTime > 0)
        {
            currTime = Mathf.Clamp(currTime, 0, totalTime);
            yield return new WaitForSeconds(Time.deltaTime * .5f);
            currTime -= Time.deltaTime * 0.5f;
            //print("currTime : " + currTime);
            fillImg.fillAmount = currTime / totalTime;
        }
        if(currTime <= 0) OnGameOver();
    }

    private void OnGameOver()
    {
        if (highScore.Value < currScore.Value)
        {
            highScore.Value = currScore.Value;
            PlayerPrefs.SetInt("HighScore", highScore.ValueAsInt);
        }
        //gameOverPanel.Activate();
        ItemSpwner.inst.RemoveAllItems();
        AudioPlayer.PlaySFX("GameOver");
        ApiCall.inst.CallApi(false);
    }

    public void UpdateScore(Item item)
    {
        if (item.itemType == ItemType.Energy) currTime += item.score;
        else if (item.itemType == ItemType.Enemy) currTime += item.score;
        else currScore.Value += item.score;
        currScore.Value = currScore.Value >= 0 ? currScore.Value : 0;
        if (!UserStatics.isWinApiCalled && currScore.Value >= winScore)
        {
            ApiCall.inst.CallApi(true);
            UserStatics.isWinApiCalled = true;
        }
    }
  
    private void GetParmsAndSave()
    {
        string url = "http://www.triangulargamestudio.com/cardList/outerspace/?customer_id=357752&card_no=T5MJNMLO&account_id=753888&site_id=1020&win_score=20.00&win_time=5";

    #if UNITY_EDITOR
        Dictionary<string, string> pairs = Utility.GetParametersFromURL(url);
    #else
        Dictionary<string, string> pairs = Utility.GetParametersFromURL(Application.absoluteURL);
    #endif
        if (pairs != null)
        {
            UserStatics.customer_id = pairs[Utility.customer_id];
            UserStatics.account_id = pairs[Utility.account_id];
            UserStatics.card_no = pairs[Utility.card_no];
            UserStatics.site_id = pairs[Utility.site_id];

           totalTime = float.Parse(pairs[Utility.win_time]);
            winScore = float.Parse(pairs[Utility.win_score]);
            print("userName :" + UserStatics.customer_id);
        }
    }
}
