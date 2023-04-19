using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HulaGirl : MonoBehaviour
{
    public List<Sprite> sprites;
    public Image image;
    public float secondsBetweenFrames;

    public void Animate()
    {
        StartCoroutine(AnimateCo());
    }

    private IEnumerator AnimateCo()
    { 
        foreach (var sprite in sprites)
        {
            image.sprite = sprite;
            yield return new WaitForSeconds(secondsBetweenFrames);
        }
        image.sprite = sprites[0];
    }
}
