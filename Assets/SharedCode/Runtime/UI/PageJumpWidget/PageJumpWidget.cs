using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageJumpWidget : MonoBehaviour 
{
    public Button nextButton;
    public Button prevButton;
    public Button lastButton;
    public Button firstButton;
    public UIInputField pageNoField;
    public Button jumpButton;
    public UISwitch[] pageSwitches;
    int currentPage, totalPages;
    public UnityEventInt switchPageEvt;

    void OnEnable()
    {
        if (nextButton) nextButton.onClick.AddListener(SwitchNext);
        if (prevButton) prevButton.onClick.AddListener(SwitchPrev);
        if (lastButton) lastButton.onClick.AddListener(SwitchLast);
        if (firstButton) firstButton.onClick.AddListener(SwitchFirst);
        if (jumpButton) jumpButton.onClick.AddListener(SwitchToInput);
        if (pageNoField != null) pageNoField.SubmitEvent.AddListener(OnPageNoFieldSubmit);
        for (int i = 0; i < pageSwitches.Length; i++)
        {
            pageSwitches[i].ThisSwitched += OnPageSwitchToggled;
        }
        UpdateWidget(currentPage, totalPages);
        
    }

    void OnDisable()
    {
        if (nextButton) nextButton.onClick.RemoveListener(SwitchNext);
        if (prevButton) prevButton.onClick.RemoveListener(SwitchPrev);
        if (lastButton) lastButton.onClick.RemoveListener(SwitchLast);
        if (firstButton) firstButton.onClick.RemoveListener(SwitchFirst);
        if (jumpButton) jumpButton.onClick.RemoveListener(SwitchToInput);
        if (pageNoField != null) pageNoField.SubmitEvent.RemoveListener(OnPageNoFieldSubmit);
        for (int i = 0; i < pageSwitches.Length; i++)
        {
            pageSwitches[i].ThisSwitched -= OnPageSwitchToggled;
        }
        
    }


    public void UpdateWidget(int _currentPage, int _totalPages)
    { 
        totalPages = Mathf.Clamp(_totalPages, 1, int.MaxValue);
        currentPage = Mathf.Clamp(_currentPage, 0, totalPages-1);

        if (prevButton!=null) prevButton.interactable = currentPage > 0;
        if (prevButton != null) nextButton.interactable = currentPage < (totalPages-1);
        if (firstButton != null) firstButton.interactable = currentPage > 0;
        if (lastButton != null) lastButton.interactable = currentPage < (totalPages - 1);

        int sp = (currentPage) - Mathf.CeilToInt(pageSwitches.Length / 2f) + 2;
        if (sp < 1 || totalPages < pageSwitches.Length) sp = 1;
        else if ((sp + pageSwitches.Length - 1) > totalPages) sp = totalPages - (pageSwitches.Length - 1);
 
        for (int i = 0; i < pageSwitches.Length; i++)
        {
            pageSwitches[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (sp + i).ToString();
            if ((sp + i - 1) == currentPage)
            {
                if (enabled && gameObject.activeInHierarchy) pageSwitches[i].ThisSwitched -= OnPageSwitchToggled;
                pageSwitches[i].Set(true);
                if (enabled && gameObject.activeInHierarchy) pageSwitches[i].ThisSwitched += OnPageSwitchToggled;
            }
            pageSwitches[i].gameObject.SetActive(i < totalPages);
        }

       // if(pageNoField!=null) pageNoField.placeholderText = (currentPage+1).ToString();
    }

    private void OnPageSwitchToggled(UISwitch sw, bool isOn)
    {
        if (isOn)
        {
            SwitchPage(int.Parse(sw.GetComponentInChildren<TMPro.TextMeshProUGUI>().text)-1);
        }
    }
    void SwitchNext()
    {
        SwitchPage(currentPage + 1);
    }
    void SwitchPrev()
    {
        SwitchPage(currentPage - 1);
    }
    private void SwitchFirst()
    {
        SwitchPage(0);
    }
    private void SwitchLast()
    {
        SwitchPage(totalPages - 1);
    }
    void SwitchToInput()
    {
        int p = 0;
        try { p = int.Parse(pageNoField.text); }
        catch { }
        SwitchPage(p-1);
    }
    void SwitchPage(int p)
    {
        switchPageEvt.Invoke(p);
    } 
    private void OnPageNoFieldSubmit(string arg0)
    {
        SwitchToInput();
    }
} 