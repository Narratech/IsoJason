using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    public KeyboardController KCRef;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectCharacter(int key)
    {
        KCRef.SetPressedKey(key);
    }

}
