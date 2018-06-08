using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour {

    public AlephMinersWorld AMWRef;
    public KeyboardController KCRef;
    public GameObject LicusPortrait;
    public GameObject AliciaPortrait;
    public GameObject AyanaPortrait;
    public GameObject HannahPortrait;
    public GameObject SimeonPortrait;
    private GameObject[] portraitsArray;

    private int flickerCounter = 0;

    //public GameObject[] CharacterFaces;

    private int lastKey = -1;
    private Color defaultColor;

	// Use this for initialization
	void Start () {
        portraitsArray = new GameObject[5];
        portraitsArray[0] = LicusPortrait;
        portraitsArray[1] = AliciaPortrait;
        portraitsArray[2] = AyanaPortrait;
        portraitsArray[3] = HannahPortrait;
        portraitsArray[4] = SimeonPortrait;
        DeactivatePortraits();
	}

    private void DeactivatePortraits()
    {
        for(int i = 0; i < portraitsArray.Length; i++)
        {
            portraitsArray[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateSelectedCharacter();
	}

    public void UpdateSelectedCharacter()
    {
        int key = KCRef.GetComponent<KeyboardController>().GetPressedKey();

        FlickeringLight(key);

        if (key != lastKey)
        {
            SwitchCharPicture(key);
            SwitchCamera(key);
            
            lastKey = key;
        }
    }

    private void FlickeringLight(int key)
    {
        GameObject[] charsArray = AMWRef.agModels;
        if (key == lastKey)
        {
            if(flickerCounter == 0)
            {
                Color c = charsArray[key-1].GetComponentInChildren<Light>().color;

                if (c.Equals(defaultColor))
                    charsArray[key - 1].GetComponentInChildren<Light>().color = Color.white;
                else
                    charsArray[key - 1].GetComponentInChildren<Light>().color = defaultColor;
            }

            IncreaseFlickerCounter();
        }
        else
        {
            if(lastKey > 0)
                charsArray[lastKey-1].GetComponentInChildren<Light>().color = defaultColor;

            defaultColor = charsArray[key-1].GetComponentInChildren<Light>().color;
            charsArray[key-1].GetComponentInChildren<Light>().color = Color.white;
            flickerCounter = 0;
        }
    }

    void IncreaseFlickerCounter()
    {
        flickerCounter = (flickerCounter + 1) % 10;
    }

    private void SwitchCamera(int key)
    {
        GameObject[] charsArray = AMWRef.agModels;

        for (int i = 0; i < charsArray.Length; i++)
        {
            charsArray[i].GetComponentInChildren<Camera>().enabled = false;
        }

        charsArray[key - 1].GetComponentInChildren<Camera>().enabled = true;
    }

    private void SwitchCharPicture(int key)
    {
        //string agClass = AMWRef.GetAgClass(key - 1);

        DeactivatePortraits();

        portraitsArray[key - 1].SetActive(true);
    }
}
