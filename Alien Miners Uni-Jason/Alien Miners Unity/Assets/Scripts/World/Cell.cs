using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    
    private Renderer rend;

    private bool libre;

    private GameObject agente;
    public Object agentPrefab;

    private GameObject oro;
    public Object goldPrefab;

    private GameObject obstacle;
    public Object obstaclePrefab;

    public Renderer GetRenderer() {
        return rend;
    }

	// We construct Celda using AWAKE when a instance of celdaObject is created in Tablero
    // We need to build the model BEFORE the view, because ColocarOro() will check if Celda.EstaLibre() and Celda.HayOro()
	void Awake () {
        rend = GetComponent<Renderer>();
        libre = true;

        agente = Instantiate(agentPrefab) as GameObject;
        agente.SetActive(false);

        oro = Instantiate(goldPrefab) as GameObject;
        oro.SetActive(false);

        obstacle = Instantiate(obstaclePrefab) as GameObject;
        obstacle.SetActive(false);
    }
    // We solve gold position (VIEW) AFTER Tablero had ended its Start()
    void Start () {
        oro.transform.position = gameObject.transform.position + new Vector3(0,0,-1);
        agente.transform.position = gameObject.transform.position + new Vector3(0, 0, -1);
        obstacle.transform.position = gameObject.transform.position + new Vector3(0, 0, -1);
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
            PlaceAgent();
        else if (value == GoldMinersWorld.GOLD)
            PlaceGold();
        else if (value == GoldMinersWorld.OBSTACLE)
            PlaceObstacle();
    }

    private void PlaceAgent() {
        libre = false;
        agente.SetActive(true);
    }

    private void PlaceGold() {
        oro.SetActive(true);
    }

    private void PlaceObstacle() {
        obstacle.SetActive(true);
    }


    public void RemoveFromHere(int value) {
        if (value == GoldMinersWorld.AGENT)
            RemoveAgent();
        else if (value == GoldMinersWorld.GOLD)
            RemoveGold();
        else if (value == GoldMinersWorld.OBSTACLE)
            RemoveObstacle();
    }

    private void RemoveAgent() {
        libre = true;
        agente.SetActive(false);
    }

    private void RemoveGold() {
        oro.SetActive(false);
    }

    private void RemoveObstacle() {
        obstacle.SetActive(false);
    }

}

//Comentado todo lo que implica tener referencias de agentes en las casillas
