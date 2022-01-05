using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Looper : LooperBase 
{
    public int startFrom, endAt, increaseBy;
    public UnityEvent startEv;
    public UnityEventInt ev;
    public UnityEvent endEv;
    public uNumber currentI;
	
    public override void StartLoop()
    { 
        base.startVal = startFrom;
        base.endVal = endAt;
        base.incrementVal = increaseBy;
        base.StartLoop();
        startEv.Invoke();
    } 

    public override void InvokeEvent(int i)
    {
        currentI.Value = i;
        ev.Invoke(i);
    }

    public override void EndEvent()
    {
        endEv.Invoke();
    }
}
