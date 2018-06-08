using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlephIconController : MonoBehaviour {

    public AlephMinersWorld AMWRef;
    public KeyboardController KCRef;

    int xPos;
    int yPos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SendPickAlephOrder()
    {
        int key = KCRef.GetPressedKey();

        if (AMWRef.GetAgClass(key - 1) == "collector")
        {
            JasonInstructions.AddPercept("miner" + key, "go_to_pos(" + xPos + "," + yPos + ")");
            //this.gameObject.SetActive(false);
        }
    }

    public void SetPoint(int x, int y)
    {
        xPos = x;
        yPos = y;
    }

    public bool IsOnPoint(int x, int y)
    {
        return (xPos == x) && (yPos == y);
    }
}
