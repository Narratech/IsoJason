using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agente : MonoBehaviour {

    public int id;
    bool oro;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddOro(){
        oro = true;
    }

    public bool TieneOro() {
        return oro;
    }
}
