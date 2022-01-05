using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class uAnimatorPlayer : uAnimatorExt
{
    public float duration = 1;
    public float speed = 1;
    public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
    public bool startOnEnable;
    public bool loop;
    public float initialDelay;
    private float timeElapsed;

    void OnEnable()
    {
        if (startOnEnable)
        {
            Resume();
        }
    }
     
    public void Restart()
    { 
        Play();
    }

    public void Play()
    {
        Pause();
        timeElapsed = 0;
        anim.value = 0;
        Resume();
    }

    public void Stop()
    {
        Pause();
        timeElapsed = duration;
        anim.value = 1;
    }

    public void Reset()
    {
        Pause();
        timeElapsed = 0;
        anim.value = 0;
    }
    Coroutine cr;
    public void Resume()
    {
        if (cr != null) return;
        cr = CommonMono.instance.StartCoroutine(this.Tween());
        //StartCoroutine("Tween");
    }

    public void Pause()
    {
        if (cr == null) return;
        StopCoroutine(cr);
        cr = null;
        //StopCoroutine("Tween");
    }

    IEnumerator Tween()
    {
        while (anim.value < 1)
        {
            timeElapsed += Time.deltaTime * speed;
            if (timeElapsed < initialDelay) anim.value = 0;
            else anim.value = curve.Evaluate((timeElapsed - initialDelay) / duration);
            yield return null;
        }
        anim.value = 1;
        cr = null;
        if (loop) Restart();
    }
}
