using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapListner : MonoBehaviour
{
    public UIButton leftBtn;
    public UIButton rightBtn;

    public static event Action<bool> OnClick;
    public static event Action<bool> OnPressed;
    public static event Action<bool> OnExit;

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            OnClick?.Invoke(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            OnClick?.Invoke(true);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            OnClick?.Invoke(false);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            OnExit?.Invoke(false);
        }

    }


    private void FixedUpdate()
    {
        switch (leftBtn.state)
        {
            case UIButton.State.Normal:
                OnExit?.Invoke(true);
                break;
            case UIButton.State.Pressed:
                OnClick?.Invoke(true);
                if (!UIManager.inst.gameStarted) UIManager.inst.StartMoveObjects();
                break;
        }
        switch (rightBtn.state)
        {
            case UIButton.State.Normal:
                OnExit?.Invoke(false);
                break;
            case UIButton.State.Pressed:
                OnClick?.Invoke(false);
                if (!UIManager.inst.gameStarted) UIManager.inst.StartMoveObjects();
                break;
        }
    }
}
