using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public abstract class uAnimator : MonoBehaviour
{
    public float m_value = 0;
    public float value
    {
        get { return m_value; }
        set
        {
            m_value = value;
            Animate(value);
        }
    }
    public abstract void Animate(float value);
}

public abstract class uAnimatorExt : MonoBehaviour 
{
    public uAnimator m_anim = null;
    public uAnimator anim
    {
        get
        {
            try
            {
                if (m_anim == null) m_anim = GetComponent<uAnimator>();
                return m_anim;
            }
            catch
            {
                return m_anim;
            }
        }
    }
}

