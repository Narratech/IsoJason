using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMinersWorld : GridWorld {

    //Variables accesibles desde el Inspector
    public int initialNbGolds;
    public Object celdaPrefab;
    public Object agentePrefab;

    //Variables del modelo
    public const int GOLD = 16;
    public const int DEPOT = 32;
    public const int ENEMY = 64;
    protected int goldsInDepot;

    private Celda[,] tableroCeldas;
    private HashSet<int> agWithGold;
    //Location of depot where gold is dropped
    private Location depot;
    private enum Direccion
    {
        UP, DOWN, RIGHT, LEFT
    };

    // Use this for initialization
    void Start() {
        InicializarTablero(width, height);
        //METODO QUE CALCULE POSICION DEL DEPOSITO
        //InicializarDeposito(x, y);
        ColocarOro();
        //ColocarAgentes();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    //Use this for initialize the board game
    private void InicializarTablero (int filas, int columnas) {
        tableroCeldas = new Celda[filas, columnas];
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                    tableroCeldas[i, j] = InstanciarCeldas(i, j);
            }
        }
    }

    private Celda InstanciarCeldas(int x, int y)
    {
        //We wake up (Celda.Awake() ) this new Celda by making an instance of CeldaPrefab
        GameObject celdaObject = Instantiate(celdaPrefab) as GameObject;
        // We set the position every Celda.oro will take as reference for calculating their own position (at their Start() )
        celdaObject.transform.position = new Vector3(x, y, 0f);
        Celda celdaScript = celdaObject.GetComponent<Celda>();
        return celdaScript;
    }

    private void InicializarDeposito(int x, int y) {
        SetDepot(x, y);
        goldsInDepot = 0;
    }

    private void ColocarOro() {
        int placed = 0, max = width * height; ;
        int p;
        if (initialNbGolds <= max) {
            while (placed < initialNbGolds)
            {
                int i = 0;
                while (i < width && placed < initialNbGolds) {
                    int j = 0;
                    while (j < height && placed < initialNbGolds) {
                        if (IsFree(i, j) && !HasObject(GoldMinersWorld.GOLD, i, j)) {
                            p = Random.Range(0, max);
                            if (p < max * 0.10) {
                                PlaceOnView(GoldMinersWorld.GOLD, i, j);
                                placed++;
                            }

                        }
                        j++;
                    }
                    i++;
                }
            }
        }

    }

    private void ColocarAgentes() {

    }



    //Overrides GridWorld functions Add, Remove and SetAgPos for updating the view from Tablero

    public override void SetAgPos(int ag, int x, int y) {
        Location oldLoc = GetAgPos(ag);

        base.SetAgPos(ag, x, y);
        //Eliminacion y adición del agente en la vista
        RemoveFromView(GoldMinersWorld.AGENT, oldLoc.x, oldLoc.y);
        PlaceOnView(GoldMinersWorld.AGENT, x, y);  
    }

    public override void Remove(int value, Location l) {
        base.Remove(value, l);
        RemoveFromView(value, l.x, l.y);
    }

    public override void Add(int value, Location l) {
        base.Add(value, l);
        PlaceOnView(value, l.x, l.y);
    }

    void RemoveFromView(int value, int x, int y) {
        tableroCeldas[x, y].RemoveFromHere(value);
    }

    void PlaceOnView(int value, int x, int y) {
        tableroCeldas[x, y].PlaceHere(value);
    }



    //Metodos de control del tablero (Adaptación de los métodos originales)

    Location GetDepot() {
        return depot;
    }

    int GetGoldsInDepot() {
        return goldsInDepot;
    }

    bool IsAllGoldsCollected() {
        return goldsInDepot == initialNbGolds;
    }

    void SetInitialNbGolds(int i) {
        initialNbGolds = i;
    }

    int GetInitialNbGolds() {
        return initialNbGolds;
    }

    bool IsCarryingGold(int ag) {
        return agWithGold.Contains(ag);
    }

    void SetDepot(int x, int y)
    {
        depot = new Location(x, y);
        data[x, y] = DEPOT;
    }

    public void SetAgCarryingGold(int ag) {
        agWithGold.Add(ag);
    }

    public void SetAgNotCarryingGold(int ag) {
        agWithGold.Remove(ag);
    }

    // Métdodos de accion de los agentes
    bool Move(Direccion dir, int ag) {
        Location l = GetAgPos(ag);
        switch (dir){
            case Direccion.UP:
                if (IsFree(l)) {
                    SetAgPos(ag, l.x, l.y);
                }
                break;
            case Direccion.DOWN:
                if (IsFree(l)) {
                    SetAgPos(ag, l.x, l.y);
                }
                break;
            case Direccion.RIGHT:
                if (IsFree(l)) {
                    SetAgPos(ag, l.x, l.y);
                }
                break;
            case Direccion.LEFT:
                if (IsFree(l)) {
                    SetAgPos(ag, l.x, l.y);
                }
                break;
        }
        return true;
    }

    bool Pick(int ag){
        Location l = GetAgPos(ag);
        if (HasObject(GoldMinersWorld.GOLD, l)){
            if (IsCarryingGold(ag)) {
                Remove(GoldMinersWorld.GOLD, l);
                SetAgCarryingGold(ag);
                return true;
            }
            else {
                //new Logger().LogWarning("Pick", "Agent " + (ag + 1) + " is trying the pick gold, but it is already carrying gold!");
                //logger.warning("Agent " + (ag + 1) + " is trying the pick gold, but it is already carrying gold!");
            }
        }
        else {
            //logger.warning("Agent " + (ag + 1) + " is trying the pick gold, but there is no gold at " + l.x + "x" + l.y + "!");
        }
        return false;
    }

    bool Drop(int ag){
        Location l = GetAgPos(ag);
        if (IsCarryingGold(ag)) {
            if (l.Equals(depot)) {
                goldsInDepot++;
                //logger
            }
            else {
                Add(GoldMinersWorld.GOLD, l);
            }
            SetAgNotCarryingGold(ag);
            return true;
        }
        return false;
    }

}
