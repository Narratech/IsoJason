using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celda : MonoBehaviour {

    private bool libre;
    //private Agente agente;
    private GameObject oro;
    public Object oroPrefab;

	// We construct Celda using AWAKE when a instance of celdaObject is created in Tablero
    // We need to build the model BEFORE the view, because ColocarOro() will check if Celda.EstaLibre() and Celda.HayOro()
	void Awake () {
        libre = true;
        //agente = null;
        oro = Instantiate(oroPrefab) as GameObject;
        oro.SetActive(false);
    }
    // We solve gold position (VIEW) AFTER Tablero had ended its Start()
    void Start () {
        oro.transform.position = gameObject.transform.position + new Vector3(0,0,-1);
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    /*
    public bool HayOro() {
        return oro.activeInHierarchy;
    }*/

    public void PlaceHere(int value) {
        if (value == GoldMinersWorld.AGENT)
            libre = false;
        else if (value == GoldMinersWorld.GOLD)
            oro.SetActive(true);
    }

    public void RemoveFromHere(int value) {
        if (value == GoldMinersWorld.AGENT)
            libre = true;
        else if (value == GoldMinersWorld.GOLD)
            oro.SetActive(false);
    }

}

//Comentado todo lo que implica tener referencias de agentes en las casillas
