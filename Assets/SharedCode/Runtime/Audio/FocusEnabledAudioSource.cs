using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FocusEnabledAudioSource : MonoBehaviour
{
    float cachedVol = -1;
    AudioSource source;
    //void OnApplicationFocus(bool inFocus)
    //{
    //    if (source == null) source = GetComponent<AudioSource>();
    //    if (cachedVol == -1) cachedVol = source.volume;
    //    if (inFocus)
    //    {
    //        source.volume = cachedVol;
    //    }
    //    else
    //    {
    //        source.volume = 0;
    //    }
    //}
    IEnumerator Start()
    {
        if (source == null) source = GetComponent<AudioSource>();
        if (cachedVol == -1) cachedVol = source.volume;
        while (true)
        {
            if (Application.isFocused)
            {
                if (source.volume < cachedVol)
                {
                    source.volume += .03f;
                    if (source.volume > cachedVol)
                    {
                        source.volume = cachedVol;
                    }
                }
            }
            else
            {
                if (source.volume > 0)
                {
                    source.volume -= .03f;
                    if (source.volume < 0)
                    {
                        source.volume = 0;
                    }
                }
            }
            yield return new WaitForSeconds(0.03f);
        }
    }
}
