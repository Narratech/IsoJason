using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

    public GameObject MistLayer;
    bool mistLayerActive = true;
    private int pressedKey = 1;
    private int activationDelay = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("1"))
            pressedKey = 1;
        else if (Input.GetKey("2"))
            pressedKey = 2;
        else if (Input.GetKey("3"))
            pressedKey = 3;
        else if (Input.GetKey("4"))
            pressedKey = 4;
        else if (Input.GetKey("5"))
            pressedKey = 5;
        else if (Input.GetKey("0"))
            SwitchMistLayerState();

        if(activationDelay > 0)
            activationDelay = (activationDelay - 1) % Int32.MaxValue;
    }

    private void SwitchMistLayerState()
    {
        if (activationDelay <= 0)
        {
            if (mistLayerActive)
            {
                mistLayerActive = false;
                MistLayer.SetActive(mistLayerActive);
            }
            else
            {
                mistLayerActive = true;
                MistLayer.SetActive(mistLayerActive);
            }
            activationDelay = 10;
        }
    }

    public int GetPressedKey()
    {
        return pressedKey;
    }

    public void SetPressedKey(int key)
    {
        this.pressedKey = key;
    }
}
