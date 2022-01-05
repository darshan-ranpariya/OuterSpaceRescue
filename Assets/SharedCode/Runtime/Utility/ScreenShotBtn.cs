using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotBtn : MonoBehaviour {


    public Screenshot.GameName gameName;
    public Button btn;
    public UIButton uIBtn;

    private void OnEnable()
    {
        if (btn) btn.onClick.AddListener(Take);
        if (uIBtn) uIBtn.Clicked += Take;
    }

    private void OnDisable()
    {
        if (btn) btn.onClick.RemoveListener(Take);
        if (uIBtn) uIBtn.Clicked -= Take;
    }

    void Take()
    {
        print("taking snapshot");
        Screenshot.Take(gameName);
    }
}
