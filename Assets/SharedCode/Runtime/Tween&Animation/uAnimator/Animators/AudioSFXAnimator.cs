using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSFXAnimator : uAnimator
{
    [Serializable]
    public class AudioTrigger
    {
        public AudioClip clip; 
        [Range(0,1)]
        public float triggerValue = .01f;
    }

    public AudioTrigger[] audios;
    float lastTrigger = -1;
    float lastValue = -1;

    public override void Animate(float value)
    {
        if (lastValue > value)
        {
            lastTrigger = -1;
        }
        else
        {
            for (int i = audios.Length - 1; i >= 0; i--)
            {
                var aud = audios[i];
                //Debug.LogFormat("Loop i:{0} v:{1} t:{2} l:{3}", i, value, aud.triggerVal, lastTrigger);
                if (value >= aud.triggerValue)
                {
                    if (lastTrigger != aud.triggerValue)
                    {
                        lastTrigger = aud.triggerValue;
                        if (aud.clip != null)
                        {
                            if (Application.isPlaying
                                && gameObject.activeInHierarchy
                                && value > 0 
                                && value < 1)
                                //Debug.LogFormat("Play {0} {1}".WithColorTag(Color.red), i, aud.clip.name);
                                AudioPlayer.PlaySFX(aud.clip);
                        }
                    }
                    break;
                }
            }
        }
        lastValue = value;
    }
}