using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DynamicSpriteCollection : MonoBehaviour
{
    [System.Serializable]
    public class SpriteElement
    {
        public string key;
        public Sprite sprite;
    }

    public event System.Action CollectionUpdated;

    [SerializeField]
    List<SpriteElement> sprites = new List<SpriteElement>();
    
    public void AddSprite(string _key, Sprite _sprite)
    {
        SpriteElement e = sprites.Find((s) => { return s.key.Equals(_key); });
        if (e == null)
        {
            sprites.Add(new SpriteElement()
            {
                key = _key,
                sprite = _sprite
            });
        }
        else
        {
            e.sprite = _sprite;
        }
        if (CollectionUpdated != null) CollectionUpdated();
    }

    public Sprite GetSprite(int _index)
    {
        try
        {
            return sprites[_index].sprite;
        }
        catch
        {
            return null;
        }
    }

    public Sprite GetSprite(string _name)
    {
        try
        {
            return sprites.Find((s)=> { return s.key.Equals(_name); }).sprite;
        }
        catch
        {
            return null;
        }
    }
}
