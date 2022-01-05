using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullGroup : MonoBehaviour
{
    List<ICullObject> objects = new List<ICullObject>();
    public Transform objectsParent;

    int loc = 0; //last objects count
    Vector3 lpp = Vector3.zero; //last parent position

    public void UpdateObjectsList()
    {
        objects.Clear();
        for (int i = 0; i < objectsParent.childCount; i++)
        {
            ICullObject co = objectsParent.GetComponent<ICullObject>();
            if (co == null) co = gameObject.AddComponent<CullObject>();
            objects.Add(co);
        }
        loc = objects.Count;
    }

    void Update()
    {
        if (loc != objectsParent.childCount)
        {
            UpdateObjectsList();
        }
        if (Vector3.Distance(objectsParent.position, lpp) > 0.1f)
        {

        }
    }
}
