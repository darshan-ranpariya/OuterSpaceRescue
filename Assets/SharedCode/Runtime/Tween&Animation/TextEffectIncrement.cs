using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextEffectIncrement : MonoBehaviour { 
	public Text textComp;
	public float defDuration = 1;
	float currDuration = 1;
    public double startVal = 0;
    public double targetVal = 0;
    double currentVal = 0;
	public string prefix = string.Empty;

	public bool beginOnEnable;

	void OnEnable(){
        //Begin (0, 1000000, 0.6f);
		if (beginOnEnable) {
			Begin ();
		}
	}

    public void Begin(double fromVal, double toVal, float duration = 0)
	{
		Set(fromVal, toVal, duration);
		Begin ();
	}

	public void Set(double fromVal, double toVal, float duration = 0)
	{
		currDuration = duration == 0 ? defDuration : duration;
		startVal = currentVal = fromVal;
		targetVal = toVal;
    } 

	void Begin()
	{ 
		StopCoroutine ("Effect");
		StartCoroutine ("Effect");
	}

    public AudioSource audio; 
    public AudioClip loopSound,endSound;
	IEnumerator Effect()
	{
        //		print (startVal + " to " + targetVal);
		float t = 0;
        if (audio!=null)
        {
            audio.playOnAwake = false;
            audio.loop = true;
            if (loopSound != null)
            {
                audio.clip = loopSound;
                audio.Stop(); 
                audio.Play();
            }
        }

        string f = "0";
        int d = 0;
        try { d = startVal.ToString().Split('.')[1].Length; }
        catch { }
        if (d > 2) f = "0.000";
        else if (d > 1) f = "0.00";
        else if (d > 0) f = "0.0";

        while (t < currDuration) 
		{
			currentVal = Extensions.Lerp (startVal, targetVal, t / currDuration);	
			//textComp.text = currentVal.ToFormattedString(true, false, prefix);
			textComp.text = string.Format("{0} {1}", prefix, currentVal.ToString(f));
            t += Time.deltaTime;
			yield return null;
		}	
		currentVal = targetVal;
        textComp.text = currentVal.ToFormattedString(true, false, prefix);
        if (audio!=null)
        { 
            audio.loop = false;
            audio.Stop();
            if(endSound!=null) audio.PlayOneShot(endSound);
        }
	}
}
