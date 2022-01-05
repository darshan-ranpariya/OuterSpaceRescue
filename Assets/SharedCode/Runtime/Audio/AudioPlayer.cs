using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AudioPlayer : MonoBehaviour {
    [System.Serializable]
    public class AudioPrefs
    { 
        public bool effectsOn;
        public bool musicOn;
    }

    [System.Serializable]
	public class Audio{
		public string key;
		public AudioClip clip; 
	}

    public static AudioPlayer _instance;
    public static AudioPlayer instance {
        get {
            if (_instance==null)
            {
                _instance = FindObjectOfType<AudioPlayer>();
            }
            return _instance;
        }
        set {
            _instance = value;
        }
    }

    public static AudioPrefs prefs;
    public static bool prefsLoaded = false;
    public static void LoadPrefs()
    {
        if (!prefsLoaded)
        {
            prefs = LocalData.Load<AudioPrefs>("audioPrefs");
            if (prefs == null)
            {
                prefs = new AudioPrefs();
                prefs.effectsOn = true;
                prefs.musicOn = true;
            }
            prefsLoaded = true;
        }
    }
    public static void SavePrefs()
    {
        LocalData.Save("audioPrefs", prefs, true);
    }

    public List<Audio> audios;
	public static Dictionary<string, Audio> Audios;

    [Space]
    public AudioSource fxSource;
    public AudioSource musicSource; 

    [Space]
    public bool playerReady = false;

    public static bool effectsOn
    {
        get
        {
            LoadPrefs();
            return prefs.effectsOn;
        }
        set
        {
            LoadPrefs();
            if (prefs.effectsOn != value)
            {
                prefs.effectsOn = value;
                SavePrefs();
            }
            if (instance != null)
            {
                instance.UpdatePlayers();
            }
        }
    }

    public static bool musicOn
    {
        get
        {
            LoadPrefs();
            return prefs.musicOn;
        }
        set
        {
            LoadPrefs();
            if (prefs.musicOn != value)
            {
                prefs.musicOn = value;
                SavePrefs();
            } 
            if (instance != null)
            {
                instance.UpdatePlayers();
            }
        }
    } 

    void OnEnable()
    {
        instance = this;
        Audios = new Dictionary<string, Audio>();
        for (int i = 0; i < audios.Count; i++) if (!Audios.ContainsKey(audios[i].key)) Audios.Add(audios[i].key, audios[i]);
        UpdatePlayers();
        playerReady = true;
        LocalTCPMessanger.Received += LocalTCPMessageReceived;
    }

	void OnDisable(){
        //print("AP Destroyed: " + transform.name);
        LocalTCPMessanger.Received -= LocalTCPMessageReceived;
    }

    private void LocalTCPMessageReceived(string obj)
    {
        if (obj.Equals("AudUp"))
        {
            prefsLoaded = false;
            UpdatePlayers();
        }
    }

    public static void PlaySFX(string key, float delay=0)
    {
        if (string.IsNullOrEmpty(key) || !Audios.ContainsKey(key)) return;
        instance.StartCoroutine(instance.play_c (Audios[key].clip, delay));
	}
    public static void PlaySFX(AudioClip clip, float delay = 0)
    { 
        instance.StartCoroutine(instance.play_c(clip, delay));
    }
    IEnumerator play_c(AudioClip clip, float delay)
    {
        if (!effectsOn) yield break;
        if (!instance.playerReady) yield break;
        if (delay>0)yield return new WaitForSeconds (delay);
        fxSource.PlayOneShot(clip);
#if UNITY_EDITOR
        Visualize(clip.name);
#endif
        //Debug.Log("Audio : ".WithColorTag(Color.magenta) + clip);
    }

	public static void SwitchFx()
    {
        effectsOn = !effectsOn;
    }
    public static void SetFx(bool on)
    {
        effectsOn = on;
    }
    public void SwitchFxIns()
    {
        effectsOn = !effectsOn;
    }
    public void SetFxIns(bool on)
    {
        effectsOn = on;
    }


    public static void PlayMusic( )
    {
        instance.PlayMusicIns();
    }
    public void PlayMusicIns( )
    {
        if (!musicOn) return;
        if (!instance.playerReady) return;
        musicSource.Play();
    }
    public static void StopMusic()
    {
        instance.StopMusicIns();
    }
    public void StopMusicIns()
    {
        if (!musicOn) return;
        if (!instance.playerReady) return;
        musicSource.gameObject.SetActive(false);
    }

    public static void SwitchMusic()
    {
        musicOn = !musicOn;
    }
    public static void SetMusic(bool on)
    {
        musicOn = on;
    }
    public void SwitchMusicIns()
    {
        musicOn = !musicOn;
    }
    public void SetMusicIns(bool on)
    {
        musicOn = on;
    }

    void UpdatePlayers()
    {
        musicSource.mute = !musicOn;
        fxSource.mute = !effectsOn;

        LocalTCPMessanger.Send("AudUp");
    }

    public Text visualizeTxtComp;
    void Visualize(string msg) {
        if (visualizeTxtComp!=null)
        {
            visualizeTxtComp.transform.parent.gameObject.SetActive(false);
            visualizeTxtComp.transform.parent.gameObject.SetActive(true);
            visualizeTxtComp.text = msg;
        }
    }
}
