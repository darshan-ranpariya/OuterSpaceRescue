using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UpdateOnMove : MonoBehaviour 
{
    IUpdateOnMove[] subscribers = new IUpdateOnMove[0];
    Vector3 lastPos;
    Vector3 currPos;
    public bool updateInEditor;

    void OnEnable()
    {
        #if UNITY_EDITOR
        if (!updateInEditor) return;
        #endif

        subscribers = GetComponents<IUpdateOnMove>(); 
        //print(subscribers.Length);
    }
    public void Update()
    {
        #if UNITY_EDITOR
        if (!updateInEditor) return;
        #endif

        if (subscribers.Length == 0) return; 
        
        currPos = transform.position;
        if ((lastPos.x != currPos.x) || (lastPos.y != currPos.y) || (lastPos.z != currPos.z))
        {
            for (int i = 0; i < subscribers.Length; i++)
            {
                subscribers[i].OnMove();
            }
        } 
        lastPos = currPos;  
    } 
}

public interface IUpdateOnMove
{
    void OnMove();
}
