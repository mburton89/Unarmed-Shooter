using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomguyHealthManager : MonoBehaviour
{
    public static DoomguyHealthManager Instance;

    public DoomHealthPortraitManager portrait100;
    public DoomHealthPortraitManager portrait50;
    public DoomHealthPortraitManager portrait10;

    [HideInInspector] public DoomHealthPortraitManager activeManager;

    public HulaGirl hulaGirl;

    private void Awake()
    {
        Instance = this;
        activeManager = portrait100;
    }

    public void ShowCorrectHealhPortait(int currentHealth, int maxHealth)
    {
        float damagePercentage = (float)currentHealth / (float)maxHealth;

        if (damagePercentage > 0.5f)
        {
            ActivatePortrait100();
        }
        else if (damagePercentage>0.1f)
        {
            ActivatePortrait50();
        }
        else
        {
            ActivatePortrait10();
        }
    }

    public void ShowHappy()
    { 
        activeManager.ShowHappy();
    }

    public void ShowSad()
    {
        activeManager.ShowSad();
        hulaGirl.Animate();
    }


    public void ActivatePortrait100()
    {
        portrait100.gameObject.SetActive(true);
        portrait50.gameObject.SetActive(false);
        portrait10.gameObject.SetActive(false);

        activeManager = portrait100;
    }

    public void ActivatePortrait50()
    {
        portrait100.gameObject.SetActive(false);
        portrait50.gameObject.SetActive(true);
        portrait10.gameObject.SetActive(false);

        activeManager = portrait50;
    }

    public void ActivatePortrait10()
    {
        portrait100.gameObject.SetActive(false);
        portrait50.gameObject.SetActive(false);
        portrait10.gameObject.SetActive(true);

        activeManager = portrait10;
    }
}
