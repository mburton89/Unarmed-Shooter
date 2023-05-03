using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Holds the dialogue. Creatable w/ right click -> Create -> Dialogue
[CreateAssetMenu(fileName = "Dialogue", menuName ="Dialogue", order =1)]
public class TextData : ScriptableObject
{
    public Text[] dialogue; 
}
