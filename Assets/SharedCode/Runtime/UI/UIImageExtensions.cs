using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIImageExtensions : MonoBehaviour {
    Image _img;
    Image img {
        get {
            if (_img==null)
            {
                _img = GetComponent<Image>();
                originalOpacity = _img.color.a;
            }
            return _img;
        }
    }

    public bool setTransparentIfEmpty;
    public bool disableIfEmpty;
    float originalOpacity = 0;

    void OnEnable() {
        if (img.sprite==null)
        {
            if (setTransparentIfEmpty) img.color = img.color.transparent();

            if (disableIfEmpty) gameObject.SetActive(false);
        }
    } 
}
