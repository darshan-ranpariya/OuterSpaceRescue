using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIButton))]
public class UIBtnExt_ScaleAnim : UIBtnExt
{
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);
    public Vector3 howeredScale = new Vector3(1f, 1f, 1f);
    public Vector3 pressedScale = new Vector3(1.1f, 1.1f, 1f);
    public Vector3 disabledScale = new Vector3(1f, 1f, 1f);
    public float time = 1f;

    public Transform target;

    public override void OnButtonUpdated()
    {
        if (targetBtn.interactable)
        {
            switch (targetBtn.state)
            {
                case UIButton.State.Normal:
                    target.localScale = normalScale; 
                    break;
                case UIButton.State.Howered:
                    StopAllCoroutines();
                    if (gameObject.activeInHierarchy) StartCoroutine(Scale(howeredScale));
                    break;
                case UIButton.State.Pressed:
                    StopAllCoroutines();
                    if(gameObject.activeInHierarchy) StartCoroutine(Scale(pressedScale));
                    break;
                default:
                    break;
            }
        }
        else
        {
            target.localScale = disabledScale;
        }
    }

    IEnumerator Scale(Vector3 v)
    {
        target.localScale = Vector3.Lerp(target.localScale, v, time);
        yield return new WaitForSeconds(time);
    }

    public void OnValidate()
    {
        if (target == null) target = this.transform;
    }
}
