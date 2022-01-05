using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISwitch))]
public abstract class UISwitchExtension : MonoBehaviour
{
    public virtual void Init(UISwitch uISwitch) { }
    public abstract void OnSwitchValChanged(bool isOn);
}
