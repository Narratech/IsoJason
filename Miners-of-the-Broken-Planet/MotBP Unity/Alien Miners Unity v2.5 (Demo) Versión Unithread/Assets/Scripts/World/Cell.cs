using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    
    private Renderer rend;

    private bool libre;

    private int quad;
    public int posX;
    public int posY;

    private GameObject agente;
    public Object agentPrefab;

    private GameObject oro;
    public Object goldPrefab;

    private GameObject obstacle;
    public Object obstaclePrefab;

    private GameObject enemy;
    public Object enemyPrefab;

    public Renderer GetRenderer() {
        return rend;
    }

	// We construct Celda using AWAKE when a instance of celdaObject is created in Tablero
    // We need to build the model BEFORE the view, because PlaceGold() will check if Cell.EstaLibre() and Celda.HayOro()
	void Awake () {
        rend = GetComponent<Renderer>();
        libre = true;

        /*agente = Instantiate(agentPrefab) as GameObject;
        agente.SetActive(false);*/

        oro = Instantiate(goldPrefab) as GameObject;
        oro.SetActive(false);

        obstacle = Instantiate(obstaclePrefab) as GameObject;
        obstacle.SetActive(false);

        /*enemy = Instantiate(enemyPrefab) as GameObject;
        enemy.SetActive(false);*/
    }
    // We solve gold position (VIEW) AFTER Tablero had ended its Start()
    void Start () {
        oro.transform.position = gameObject.transform.GetComponent<Renderer>().bounds.center + new Vector3(0,0.5f,0);
        //agente.transform.position = gameObject.transform.GetComponent<Renderer>().bounds.center + new Vector3(0, 0, 0);
        obstacle.transform.position = gameObject.transform.GetComponent<Renderer>().bounds.center + new Vector3(0, 0.5f, 0);
        //enemy.transform.position = gameObject.transform.GetComponent<Renderer>().bounds.center + new Vector3(0, 0.5f, 0);*/
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    /*
    public bool HayOro() {
        return oro.activeInHierarchy;
    }*/

    public int GetQuad()
    {
        return quad;
    }

    public void SetQuad(int quad)
    {
        this.quad = quad;
    }

    public int GetX()
    {
        return posX;
    }

    public int GetY()
    {
        return posY;
    }

    public void SetX(int x)
    {
        posX = x;
    }

    public void SetY(int y)
    {
        posY = y;
    }

    public void SetPosition(int x, int y)
    {
        SetX(x);
        SetY(y);
    }

    public void PlaceHere(int value) {
        if (value == GoldMinersWorld.AGENT)
            PlaceAgent();
        else if (value == GoldMinersWorld.GOLD)
            PlaceGold();
        else if (value == GoldMinersWorld.OBSTACLE)
            PlaceObstacle();
        else if (value == GoldMinersWorld.ENEMY)
            PlaceEnemy();
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

    private void PlaceEnemy() {
        enemy.SetActive(true);
    }

    public void RemoveFromHere(int value) {
        if (value == GoldMinersWorld.AGENT)
            RemoveAgent();
        else if (value == GoldMinersWorld.GOLD)
            RemoveGold();
        else if (value == GoldMinersWorld.OBSTACLE)
            RemoveObstacle();
        else if (value == GoldMinersWorld.ENEMY)
            RemoveEnemy();
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

    private void RemoveEnemy() {
        enemy.SetActive(false);
    }
}

//Comentado todo lo que implica tener referencias de agentes en las casillas
