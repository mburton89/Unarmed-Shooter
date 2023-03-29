using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SpeedGauge : MonoBehaviour
{
    public static SpeedGauge Instance;
    public TextMeshProUGUI speedtext;

    private void Awake()
    {
        Instance = this;
    }
}
