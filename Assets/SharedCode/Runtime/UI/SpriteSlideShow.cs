using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteSlideShow : MonoBehaviour {

    public Sprite[] sprites;
    public Image img;
    public float wait = .15f;
    int currentSpriteIndex = 0;

    public bool loop = true;

    void OnEnable() {
        StartCoroutine("SlideShow_c");
    }

    IEnumerator SlideShow_c()
    {
        currentSpriteIndex = 0;
        while (true)
        {
            img.sprite = sprites[currentSpriteIndex];
            yield return new WaitForSeconds(wait);
            currentSpriteIndex++;
            if (currentSpriteIndex >= sprites.Length) { currentSpriteIndex = 0; }
            if (currentSpriteIndex == 0 && !loop) break;
        }
    }
}
