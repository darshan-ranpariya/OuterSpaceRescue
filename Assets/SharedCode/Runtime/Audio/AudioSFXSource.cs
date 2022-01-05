using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioSFXSource : MonoBehaviour
{
    AudioSource _aud;
    AudioSource aud
    {
        get
        {
            if (_aud == null)
            {
                _aud = GetComponent<AudioSource>();
            }
            return _aud;
        }
    }

    public string audioKey = "";

    void OnEnable() {
        if (aud.clip==null)
        {
            if (!string.IsNullOrEmpty(audioKey) && AudioPlayer.Audios.ContainsKey(audioKey))
            {
                aud.clip = AudioPlayer.Audios[audioKey].clip;
            }
        }
        if (aud.clip!=null && aud.playOnAwake)
        {
            aud.Play();
        }
        aud.mute = !AudioPlayer.effectsOn; 
    } 
}
