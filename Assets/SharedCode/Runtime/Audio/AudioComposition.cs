using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioComposition : MonoBehaviour
{
    [System.Serializable]
    public class Part
    {
        public AudioClip clip;
        public float volume = 1;
        public bool loop;
        public int limitLoopIterations;
    }

    public AudioSource audioComp;
    public Part[] parts = new Part[] { };
    public float fadeInTime, fadeOutTime;
    public bool playOnEnable,loop;

    void OnEnable() {
        fadeInTime = Mathf.Clamp(fadeInTime, 0, totalLength());
        fadeOutTime = Mathf.Clamp(fadeOutTime, 0, totalLength());
        if (playOnEnable) Play();
    }

    public void Play() {
        if (parts.Length == 0) return; 

        if (audioComp == null) try { audioComp = GetComponent<AudioSource>(); } catch { return; }
        audioComp.playOnAwake = false; 
        audioComp.loop = false; 

        StopAllCoroutines(); 
        StartCoroutine(composite()); 
        StartCoroutine(fadeIn_c()); 
    }

    public void Stop()
    {
        if (audioComp == null) return;

        StopAllCoroutines();
        StartCoroutine(fadeOut_c());
    }

    float totalLength() {
        float l = 0,pl=0;

        for (int i = 0; i < parts.Length; i++)
        { 
            pl = parts[i].clip.length;
            if (parts[i].loop)
            {
                if (parts[i].limitLoopIterations != 0) pl = pl * parts[i].limitLoopIterations;
                else return 9999;
            }
            l += pl;
        }
        return l;
    }

    IEnumerator fadeIn_c() {
        audioComp.Play();
        for (int i = 0; i < fadeInTime*30; i++)
        {
            audioComp.volume = Mathf.Lerp(0, parts[0].volume, i / (fadeInTime * 30f));
            yield return new WaitForSeconds(0.03f);
        }
        audioComp.volume = parts[0].volume;
    }

    IEnumerator composite() {
        for (int i = 0; i < parts.Length; i++)
        {
            int loopIterations = 1;
            if (parts[i].loop && parts[i].limitLoopIterations != 0) loopIterations = parts[i].limitLoopIterations;
            while (loopIterations>0)
            {
                audioComp.volume = parts[i].volume;
                audioComp.clip = parts[i].clip;
                audioComp.Play();

                if (parts[i].loop && parts[i].limitLoopIterations != 0) loopIterations--;
                if (!parts[i].loop) loopIterations--;
                  
                if (!loop && i == parts.Length-1 && loopIterations == 0) StartCoroutine(fadeOut_c(Mathf.Clamp(parts[i].clip.length- fadeOutTime,0, parts[i].clip.length))); 

                yield return new WaitForSeconds(parts[i].clip.length);
            }
        }
        if(loop) StartCoroutine(composite());
    }

    IEnumerator fadeOut_c(float delay=0) {
        if (delay != 0) yield return new WaitForSeconds(delay);
        float cachedVol = audioComp.volume;
        for (int i = 0; i < fadeOutTime * 30; i++)
        {
            audioComp.volume = Mathf.Lerp(cachedVol, 0, i / (fadeOutTime * 30f));
            yield return new WaitForSeconds(0.03f);
        }
        audioComp.volume = 0;
        audioComp.Stop();
    }
}


#region editor
#if UNITY_EDITOR  
[CustomEditor(typeof(AudioComposition))]
public class Audio_CompositionEditor : Editor
{
    public AudioComposition script;

    void OnEnable()
    {
        script = target as AudioComposition;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal(); 
        if (GUILayout.Button("Play")) script.Play();
        if (GUILayout.Button("Stop")) script.Stop();
        EditorGUILayout.EndHorizontal();

    }
}
#endif
#endregion