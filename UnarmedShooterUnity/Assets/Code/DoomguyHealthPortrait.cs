using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoomguyHealthPortrait : MonoBehaviour
{
    public List<Sprite> sprites;
    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(SwapSprites());
    }

    private IEnumerator SwapSprites()
    {
        int rand = Random.Range(0,sprites.Count);
        image.sprite = sprites[rand];

        float randSeconds = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(randSeconds);

        StartCoroutine(SwapSprites());
    }
}
