using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour {

    //Variables accesibles desde el Inspector
    public int dimFilas;
    public int dimColumnas;
    public int numAgentes;
    public int initialNbGolds;
    public Object celdaPrefab;
    public Object agentePrefab;

    //Variables del modelo
    protected int goldsInDepot;
    private Celda[,] tableroCeldas;
    public struct Location
    {
        public int x;
        public int y;
    };
    private Location[] listaPosAgentes;
    private Agente[] listaAgentes;
    private HashSet<int> agWithGold;
    //Location of depot where gold is dropped
    private Location depot;
    private enum Direccion
    {
        UP, DOWN, RIGHT, LEFT
    };

    // Use this for initialization
    void Start() {
        InicializarTablero(dimFilas, dimColumnas);
        //METODO QUE CALCULE POSICION DEL DEPOSITO
        //InicializarDeposito(x, y);
        ColocarOro();
        //ColocarAgentes();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PonerEnCelda(int fila, int columna, string objeto) {
        tableroCeldas[fila, columna].PonerAqui(objeto);
    }

    void QuitarDeCelda(string objeto, int fila, int columna) {
        tableroCeldas[fila, columna].QuitarDeAqui(objeto);
    }

    //Use this for initialize the board game
    private void InicializarTablero (int filas, int columnas) {
        tableroCeldas = new Celda[filas, columnas];
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
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
        int colocados = 0, max = dimFilas * dimColumnas; ;
        int p;
        while (colocados < initialNbGolds)
        {
            int i = 0;
            while(i < dimFilas && colocados < initialNbGolds) {
                int j = 0;
                while (j < dimColumnas && colocados < initialNbGolds) {
                    if (tableroCeldas[i, j].EstaLibre() && !tableroCeldas[i, j].HayOro()) {
                        p = Random.Range(0, max);
                        if (p < max*0.10) {
                            PonerEnCelda(i, j, "oro");
                            colocados++;
                        }
                        
                    }
                    j++;
                }
                i++;
            } 
        }
    }

    private void ColocarAgentes() {

    }


    //Metodos de control del tablero (Adaptación de los métodos originales)
   /* bool LlevaOro(int ag) {
        return listaAgentes[ag].TieneOro();
    }
    void DarOro(int ag) {
        listaAgentes[ag].AddOro();
    }*/

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
        depot.x = x;
        depot.y = y;
    }
    public void SetAgCarryingGold(int ag) {
        agWithGold.Add(ag);
    }
    public void SetAgNotCarryingGold(int ag) {
        agWithGold.Remove(ag);
    }


    bool Move(Direccion dir, int ag) {
        Location pos = listaPosAgentes[ag];
        int xAct = pos.x;
        int yAct = pos.y;

        switch (dir){
            case Direccion.UP:
                if (tableroCeldas[xAct, yAct - 1].EstaLibre()) {
                    QuitarDeCelda("agente", xAct, yAct);
                    PonerEnCelda(xAct, yAct - 1, "agente");
                    listaPosAgentes[ag].y = yAct - 1;
                }
                break;
            case Direccion.DOWN:
                if (tableroCeldas[xAct, yAct + 1].EstaLibre()) {
                    QuitarDeCelda("agente", xAct, yAct);
                    PonerEnCelda(xAct, yAct + 1, "agente");
                    listaPosAgentes[ag].y = yAct + 1;
                }
                break;
            case Direccion.RIGHT:
                if (tableroCeldas[xAct + 1, yAct].EstaLibre()) {
                    QuitarDeCelda("agente", xAct, yAct);
                    PonerEnCelda(xAct + 1, yAct, "agente");
                    listaPosAgentes[ag].y = xAct + 1;
                }
                break;
            case Direccion.LEFT:
                if (tableroCeldas[xAct - 1, yAct].EstaLibre()) {
                    QuitarDeCelda("agente", xAct, yAct);
                    PonerEnCelda(xAct - 1, yAct, "agente");
                    listaPosAgentes[ag].y = xAct - 1;
                }
                break;
        }
        return true;
    }

    bool Pick(int ag){
        Location pos = listaPosAgentes[ag];
        int x = pos.x;
        int y = pos.y;
        if (tableroCeldas[x,y].HayOro()) {
            if (IsCarryingGold(ag)) {
                QuitarDeCelda("oro",x, y);
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
        Location pos = listaPosAgentes[ag];
        if (IsCarryingGold(ag)) {
            if (pos.Equals(depot)) {
                goldsInDepot++;
                //logger
            }
            else {
                PonerEnCelda(pos.x, pos.y, "oro");
            }
            SetAgNotCarryingGold(ag);
            return true;
        }
        return false;
    }

}
