using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public abstract class ObservablVariableText<T> : MonoBehaviour
{
    public IObservableVariable<T> variable;
    protected Text textComp;
    protected TextMeshProUGUI textProComp;

    void OnEnable()
    {
        SetVariable();
        if(textComp==null) textComp = GetComponent<Text>();
        if(textProComp == null) textProComp = GetComponent<TextMeshProUGUI>();
        UpdateTextComp();
        variable.Updated += UpdateTextComp;
    }

    void OnDisable()
    { 
        variable.Updated -= UpdateTextComp;
    }

    public virtual void UpdateTextComp()
    {
        if (textComp != null) textComp.text = variable.Value.ToString();
        if (textProComp != null) textProComp.text = variable.Value.ToString();
    }

    public abstract void SetVariable(); 
}
