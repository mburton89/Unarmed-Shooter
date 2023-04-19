using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomHealthPortraitManager : MonoBehaviour
{
    public DoomguyHealthPortrait idle;
    public DoomguyHealthPortrait happy;
    public DoomguyHealthPortrait sad;

    public void ShowIdle()
    {
        idle.transform.localScale = Vector3.one;
        happy.transform.localScale = Vector3.zero;
        sad.transform.localScale = Vector3.zero;
    }

    public void ShowHappy()
    {
        idle.transform.localScale = Vector3.zero;
        happy.transform.localScale = Vector3.one;
        sad.transform.localScale = Vector3.zero;
        StartCoroutine(ShowIdleDelay());
    }

    public void ShowSad()
    {
        idle.transform.localScale = Vector3.zero;
        happy.transform.localScale = Vector3.zero;
        sad.transform.localScale = Vector3.one;
        StartCoroutine(ShowIdleDelay());
    }

    private IEnumerator ShowIdleDelay()
    {
        yield return new WaitForSeconds(1);
        ShowIdle();
    }
}
