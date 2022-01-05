using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LocalizedBetTypeSpriteCollections
{
    public Localization.AvailableLanguage language;
    public SpriteCollection sprites;
}

public class uStringComponent_BetTypeSprite : uStringComponent 
{
    public Image img;
    public SpriteCollection defaultCollection;
    public List<LocalizedBetTypeSpriteCollections> collections;

    public override void Handle(ref string s)
    {
        if (img != null)
        {
            Sprite sp = null;
            SpriteCollection collection = null;
            if (collections.Count > 0)
            {
                for (int i = 0; i < collections.Count; i++)
                {
                    if (collections[i].language.ToString().ToLower().Equals(Localization.CurrentLanguage.name.ToLower()))
                    {
                        collection = collections[i].sprites;
                    }
                }
            }
            if (collection == null) 
            {
                collection = defaultCollection;
            }
            sp = collection.GetSprite(s, System.StringComparison.CurrentCultureIgnoreCase);
            if (sp != null) img.sprite = sp;
            else img.sprite = SpriteCollection.transparentSprite;
        }
    } 

    void OnValidate()
    {
        if (img == null) img = GetComponent<Image>();
    }
}