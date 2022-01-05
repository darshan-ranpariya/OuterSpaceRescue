using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedImage : MonoBehaviour {
    //[System.Serializable]
	//public class LocalizedSprite
 //   {
 //       public LanguageName language;
 //       public Sprite sprite;
 //   }
 //   public Image imageComp;
 //   public Sprite fallbackSprite;
 //   [Space]
 //   public LocalizedSprite[] localizedSprites;

 //   void OnEnable()
 //   {
 //       if (imageComp == null )
 //       {
 //           imageComp = GetComponent<Image>();
 //       }
 //       if (imageComp == null) return;

 //       Localization.CurrentLanguageUpdated += UpdateSprite;
 //       if (fallbackSprite != null)
 //       {
 //           if (imageComp != null) fallbackSprite = imageComp.sprite;
 //       }
 //       UpdateSprite();
 //       //TBA
 //       new Delayed.Action(() => {
 //           if (imageComp != null && imageComp.sprite != null)
 //           {
 //               UpdateSprite();
 //           }
 //       }, 1);
 //   }

 //   void OnDisable()
 //   {
 //       Localization.CurrentLanguageUpdated -= UpdateSprite;
 //   }

 //   void UpdateSprite()
 //   {
 //       if (localizedSprites.Length > 0)
 //       {
 //           for (int i = 0; i < localizedSprites.Length; i++)
 //           {
 //               if(localizedSprites[i].language.ToString().ToLower().Contains(Localization.CurrentLanguage.name.ToLower()))
 //               {
 //                   if (localizedSprites[i].sprite != null)
 //                   {
 //                       imageComp.sprite = localizedSprites[i].sprite;
 //                   }
 //               }
 //           }
 //       }
 //   }

 //   private void OnValidate()
 //   {
 //       if (imageComp == null) imageComp = GetComponent<Image>();
 //   }

}
