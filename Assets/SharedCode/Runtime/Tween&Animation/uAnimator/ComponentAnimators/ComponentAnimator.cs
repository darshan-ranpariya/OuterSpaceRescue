using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ComponentAnimatorState
{
    [Range(0,1)]
    public float value;
    public ComponentAnimatorValueModifier[] valueModifiers;
}

public abstract class ComponentAnimatorProperties<StateType> : MonoBehaviour where StateType : ComponentAnimatorState
{
    public List<StateType> states = null;

    public ComponentAnimatorValueModifier[] valueModifiers;

    void OnValidate()
    {
        if (states == null || states.Count < 2)
        {
            states = new List<StateType> { Activator.CreateInstance<StateType>(), Activator.CreateInstance<StateType>() };
            states[0].value = 0;
            states[1].value = 1;
        }
    }
}

public class ComponentAnimatorNoState : ComponentAnimatorState { }
public abstract class ComponentAnimatorNoProperties : ComponentAnimatorProperties<ComponentAnimatorNoState>{ }

public abstract class ComponentAnimatorValueModifier : MonoBehaviour
{
    public abstract float GetModifiedValue(int componentIndex, int totalComponents, float value);
} 

public abstract class ComponentAnimator<ComponentType, StateType, PropertiesType> : uAnimator
    where StateType : ComponentAnimatorState
    where PropertiesType : ComponentAnimatorProperties<StateType>
{
    public PropertiesType properties;
    public ComponentType[] components;
    bool statesSorted = false;

    public override void Animate(float value)
    {
        if (properties == null || properties.states.Count < 1) return;
        if (components == null || components.Length == 0) return;

#if UNITY_EDITOR
        if (!Application.isPlaying) statesSorted = false;
#endif
        if (!statesSorted)
        {
            properties.states.Sort((a, b) => { return (int)((a.value - b.value) * 1000000); });
            statesSorted = true;
        }

        StateType s1 = properties.states[0];
        StateType s2 = properties.states[properties.states.Count - 1];

        float v = 0;
        float vs = 0;
        for (int i = 0; i < components.Length; i++)
        {
            v = value;
            if (properties.valueModifiers != null && properties.valueModifiers.Length > 0)
            {
                for (int vmi = 0; vmi < properties.valueModifiers.Length; vmi++)
                {
                    v = properties.valueModifiers[vmi].GetModifiedValue(i, components.Length, v);
                }
            }
            vs = v;

            if (properties.states.Count > 2)
            {
                for (int s = 0; s < properties.states.Count; s++)
                {
                    if (properties.states[s].value >= v)
                    {
                        s2 = properties.states[s];
                        if (s > 0) s1 = properties.states[s - 1];
                        else s1 = s2;
                        break;
                    }
                }
                if (s2.value == s1.value) vs = 0;
                else vs = (v - s1.value) * 1f / (s2.value - s1.value);
            }

            if (s2.valueModifiers != null && s2.valueModifiers.Length > 0)
            {
                for (int vmi = 0; vmi < s2.valueModifiers.Length; vmi++)
                {
                    vs = s2.valueModifiers[vmi].GetModifiedValue(i, components.Length, vs);
                }
            }
            //Debug.LogFormat("{0} : {1} :: {2}-{3}", i, v, s1.value, s2.value);
            AnimateComponent(components[i], s1, s2, vs);
        }
    }
    public abstract void AnimateComponent(ComponentType component, StateType s1, StateType s2, float f);
}