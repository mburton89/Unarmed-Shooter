using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD Instance;
    public List<Image> miniHealthBars;
    public List<Image> miniShieldBars;
    public TextMeshProUGUI waveText;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayHealth(int currentArmor, int currentHealth)
    {
        for (int i = 0; i < miniHealthBars.Count; i++)
        {
            bool isEnabled = i < currentHealth;
            miniHealthBars[i].enabled = isEnabled;
        }

        for (int i = 0; i < miniShieldBars.Count; i++)
        {
            bool isEnabled = i < currentArmor;
            miniShieldBars[i].enabled = isEnabled;
        }
    }

    public void DisplayWave(int currentWave)
    {
        waveText.SetText("Wave: " + currentWave);
    }
}
