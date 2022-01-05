using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHelper : MonoBehaviour
{
    public void SetPosition(Transform refTransform)
    {
        transform.position = refTransform.position;
    }
}
