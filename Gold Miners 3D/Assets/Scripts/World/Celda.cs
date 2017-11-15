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

    public bool HayOro() {
        return oro.activeInHierarchy;
    }

   /* public Agente GetAgente() {
        return agente;
    }*/

    public bool EstaLibre() {
        return libre;
    }

    public void PonerAqui(string objeto) {
        if (objeto == "agente"){
            //agente = (Agente)objeto;
            libre = false;
        }
        else if (objeto == "oro")
            oro.SetActive(true);
    }

    public void QuitarDeAqui(string objeto) {
        if (objeto == "oro")
            oro.SetActive(false);
        else if (objeto == "agente") {
            //agente = null;
            libre = true;
        }
    }

}

//Comentado todo lo que implica tener referencias de agentes en las casillas
