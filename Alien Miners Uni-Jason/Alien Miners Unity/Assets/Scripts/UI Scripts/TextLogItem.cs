using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLogItem : MonoBehaviour {

    public void SetText(string text, Color colour)
    {
        Text textContent = GetComponent<Text>();
        textContent.text = text;
        textContent.color = colour;
    }
}
