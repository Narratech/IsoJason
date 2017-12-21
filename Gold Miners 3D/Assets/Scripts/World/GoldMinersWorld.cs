using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldMinersWorld : GridWorld {

    ILogHandler logHandler;

    //Variables accesibles desde el Inspector
    public int initialNbGolds;
    public int simID;
    public Object cellPrefab;
    public Object agentPrefab;
    public Object obstaclePrefab;

    public Text textCount;

    //Variables del modelo
    public const int GOLD = 16;
    public const int DEPOT = 32;
    public const int ENEMY = 64;
    protected int goldsInDepot;

    private Cell[,] cellsBoard;
    private HashSet<int> agWithGold;
    //Location of depot where gold is dropped
    private Location depot;
    public enum Direction {
        UP, DOWN, RIGHT, LEFT
    };


    // Use this for initialization
    void Start() {
        agWithGold = new HashSet<int>();
        switch (simID) {
            case 1:
                World1();
                break;
            case 2:
                World2();
                break;
            case 3:
                World3();
                break;
            case 4:
                WorldCustom();
                break;
            default:
                World1();
                break;
        }

        PlacingGold();
        PlacingAgents();
        PlacingObstacles();

	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<GMWorldUpdater>().CheckEvents();
        textCount.text = "Gold in depot: " + goldsInDepot.ToString();
	}



    //Use this for initialize the board game
    private void InstanciateCells () {
        cellsBoard = new Cell[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                    cellsBoard[i, j] = InstanciateCell(i, j);
            }
        }
    }

    private Cell InstanciateCell(int x, int y)
    {
        //We wake up (Cell.Awake() ) this new Celda by making an instance of CeldaPrefab
        GameObject cellObject = Instantiate(cellPrefab) as GameObject;
        // We set the position every Celda.oro will take as reference for calculating their own position (at their Start() )
        cellObject.transform.position = new Vector3(x, y, 0f);
        Cell celdaScript = cellObject.GetComponent<Cell>();
        return celdaScript;
    }

    private void InitializeDepot(int x, int y) {
        SetDepot(x, y);
        goldsInDepot = 0;
    }

    private void PlacingGold() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (IsFree(i, j) && HasObject(GoldMinersWorld.GOLD, i, j))
                    PlaceOnView(GoldMinersWorld.GOLD, i, j);
            }
        }
    }

    private void PlacingAgents() {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (data[i,j] == AGENT /*&& HasObject(GoldMinersWorld.AGENT, i, j)*/)
                    PlaceOnView(GoldMinersWorld.AGENT, i, j);
            }
        }
    }

    private void PlacingObstacles() {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (data[i, j] == OBSTACLE /*&& HasObject(GoldMinersWorld.AGENT, i, j)*/)
                    PlaceOnView(GoldMinersWorld.OBSTACLE, i, j);
            }
        }
    }



    //Overrides GridWorld functions Add, Remove and SetAgPos for updating the view from Tablero

    public override void SetAgPos(int ag, int x, int y) {
        Location oldLoc = GetAgPos(ag);

        base.SetAgPos(ag, x, y);
        //Removes from the agent's old position and adds to the new one on the VIEW
        if(oldLoc != null)
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
        cellsBoard[x, y].RemoveFromHere(value);
    }

    void PlaceOnView(int value, int x, int y) {
        cellsBoard[x, y].PlaceHere(value);
    }



    // Metodos de control del tablero (Adaptación de los métodos originales)

    public Location GetDepot() {
        return depot;
    }

    public int GetGoldsInDepot() {
        return goldsInDepot;
    }

    public bool IsAllGoldsCollected() {
        return goldsInDepot == initialNbGolds;
    }

    public void SetInitialNbGolds(int i) {
        initialNbGolds = i;
    }

    public int GetInitialNbGolds() {
        return initialNbGolds;
    }

    public bool IsCarryingGold(int ag) {
        return agWithGold.Contains(ag);
    }

    public void SetDepot(int x, int y) {
        depot = new Location(x, y);
        data[x, y] = DEPOT;
        cellsBoard[x, y].rend.material.color = Color.red;
        goldsInDepot = 0;
    }

    public void SetAgCarryingGold(int ag) {
        agWithGold.Add(ag);
    }

    public void SetAgNotCarryingGold(int ag) {
        agWithGold.Remove(ag);
    }

    // Métdodos de accion de los agentes
    public bool Move(Direction dir, int ag) {
        Location l = GetAgPos(ag);
        switch (dir){
            case Direction.UP:
                if (IsFree(l.x, l.y - 1)) {
                    SetAgPos(ag, l.x, l.y - 1);
                }
                else
                    return false;
                break;
            case Direction.DOWN:
                if (IsFree(l.x, l.y + 1)) {
                    SetAgPos(ag, l.x, l.y + 1);
                }
                else
                    return false;
                break;
            case Direction.RIGHT:
                if (IsFree(l.x + 1, l.y)) {
                    SetAgPos(ag, l.x + 1, l.y);
                }
                else
                    return false;
                break;
            case Direction.LEFT:
                if (IsFree(l.x - 1, l.y)) {
                    SetAgPos(ag, l.x - 1, l.y);
                }
                else
                    return false;
                break;
        }
        return true;
    }

    public bool Pick(int ag){
        Location l = GetAgPos(ag);
        if (HasObject(GoldMinersWorld.GOLD, l)){
            if (!IsCarryingGold(ag)) {
                Remove(GoldMinersWorld.GOLD, l);
                SetAgCarryingGold(ag);
                return true;
            }
            else { 
                new Logger(logHandler).LogWarning("Pick", "Agent " + (ag + 1) + " is trying the pick gold, but it is already carrying gold!");
            }
        }
        else {
            new Logger(logHandler).LogWarning("Pick", "Agent " + (ag + 1) + " is trying the pick gold, but there is no gold at " + l.x + "x" + l.y + "!");
        }
        return false;
    }

    public bool Drop(int ag){
        Location l = GetAgPos(ag);
        if (IsCarryingGold(ag)) {
            if (l.x == depot.x && l.y == depot.y) {
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



    public void World1() {
        Initialize(21, 21, 4);
        InstanciateCells();
        SetDepot(5, 7);
        SetAgPos(0, 1, 0);
        SetAgPos(1, 20, 0);
        SetAgPos(2, 3, 20);
        SetAgPos(3, 20, 20);
        Add(GOLD, 2, 7);
        Add(GOLD, 10, 15);
        SetInitialNbGolds(CountObjects(GOLD));
    }

    /** un escenario con oro pero sin obstáculos */
    public void World2() {
        Initialize(35, 35, 4);
        InstanciateCells();
        SetDepot(5, 27);
        SetAgPos(0, 1, 0);
        SetAgPos(1, 20, 0);
        SetAgPos(2, 3, 20);
        SetAgPos(3, 20, 20);
        Add(GOLD, 20, 13);
        Add(GOLD, 15, 20);
        Add(GOLD, 1, 1);
        Add(GOLD, 3, 5);
        Add(GOLD, 24, 24);
        Add(GOLD, 20, 20);
        Add(GOLD, 20, 21);
        Add(GOLD, 20, 22);
        Add(GOLD, 20, 23);
        Add(GOLD, 20, 24);
        Add(GOLD, 19, 20);
        Add(GOLD, 19, 21);
        Add(GOLD, 34, 34);
        SetInitialNbGolds(CountObjects(GOLD));
    }

    public void WorldCustom() {
        Initialize(21, 21, 4);
        InstanciateCells();
        SetDepot(7, 6);
        SetAgPos(0, 1, 0);
        SetAgPos(1, 20, 0);
        SetAgPos(2, 3, 20);
        SetAgPos(3, 20, 20);
        Add(GOLD, 2, 7);
        Add(GOLD, 10, 15);
        Add(GOLD, 10, 20);
        Add(GOLD, 10, 19);
        Add(GOLD, 20, 18);
        Add(GOLD, 20, 17);
        Add(GOLD, 20, 16);
        Add(OBSTACLE, 12, 3);
        Add(OBSTACLE, 13, 3);
        Add(OBSTACLE, 14, 3);
        Add(OBSTACLE, 15, 3);
        Add(OBSTACLE, 18, 3);
        Add(OBSTACLE, 19, 3);
        Add(OBSTACLE, 20, 3);
        Add(OBSTACLE, 14, 8);
        Add(OBSTACLE, 15, 8);
        Add(OBSTACLE, 16, 8);
        Add(OBSTACLE, 17, 8);
        Add(OBSTACLE, 19, 8);
        Add(OBSTACLE, 20, 8);
        SetInitialNbGolds(CountObjects(GOLD));
    }

    /** el escenario 3, el más jugoso, con oro y obstáculos a saco */
    public void World3() {
        Initialize(35, 35, 4);
        InstanciateCells();
        SetDepot(16, 16);
        SetAgPos(0, 1, 0);
        SetAgPos(1, 20, 0);
        SetAgPos(2, 6, 26);
        SetAgPos(3, 20, 20);
        Add(GOLD, 20, 13);
        Add(GOLD, 15, 20);
        Add(GOLD, 1, 1);
        Add(GOLD, 3, 5);
        Add(GOLD, 24, 24);
        Add(GOLD, 20, 20);
        Add(GOLD, 20, 21);
        Add(GOLD, 2, 22);
        Add(GOLD, 2, 12);
        Add(GOLD, 19, 2);
        Add(GOLD, 14, 4);
        Add(GOLD, 34, 34);

        Add(OBSTACLE, 12, 3);
        Add(OBSTACLE, 13, 3);
        Add(OBSTACLE, 14, 3);
        Add(OBSTACLE, 15, 3);
        Add(OBSTACLE, 18, 3);
        Add(OBSTACLE, 19, 3);
        Add(OBSTACLE, 20, 3);
        Add(OBSTACLE, 14, 8);
        Add(OBSTACLE, 15, 8);
        Add(OBSTACLE, 16, 8);
        Add(OBSTACLE, 17, 8);
        Add(OBSTACLE, 19, 8);
        Add(OBSTACLE, 20, 8);

        Add(OBSTACLE, 12, 32);
        Add(OBSTACLE, 13, 32);
        Add(OBSTACLE, 14, 32);
        Add(OBSTACLE, 15, 32);
        Add(OBSTACLE, 18, 32);
        Add(OBSTACLE, 19, 32);
        Add(OBSTACLE, 20, 32);
        Add(OBSTACLE, 14, 28);
        Add(OBSTACLE, 15, 28);
        Add(OBSTACLE, 16, 28);
        Add(OBSTACLE, 17, 28);
        Add(OBSTACLE, 19, 28);
        Add(OBSTACLE, 20, 28);

        Add(OBSTACLE, 3, 12);
        Add(OBSTACLE, 3, 13);
        Add(OBSTACLE, 3, 14);
        Add(OBSTACLE, 3, 15);
        Add(OBSTACLE, 3, 18);
        Add(OBSTACLE, 3, 19);
        Add(OBSTACLE, 3, 20);
        Add(OBSTACLE, 8, 14);
        Add(OBSTACLE, 8, 15);
        Add(OBSTACLE, 8, 16);
        Add(OBSTACLE, 8, 17);
        Add(OBSTACLE, 8, 19);
        Add(OBSTACLE, 8, 20);

        Add(OBSTACLE, 32, 12);
        Add(OBSTACLE, 32, 13);
        Add(OBSTACLE, 32, 14);
        Add(OBSTACLE, 32, 15);
        Add(OBSTACLE, 32, 18);
        Add(OBSTACLE, 32, 19);
        Add(OBSTACLE, 32, 20);
        Add(OBSTACLE, 28, 14);
        Add(OBSTACLE, 28, 15);
        Add(OBSTACLE, 28, 16);
        Add(OBSTACLE, 28, 17);
        Add(OBSTACLE, 28, 19);
        Add(OBSTACLE, 28, 20);

        Add(OBSTACLE, 13, 13);
        Add(OBSTACLE, 13, 14);

        Add(OBSTACLE, 13, 16);
        Add(OBSTACLE, 13, 17);

        Add(OBSTACLE, 13, 19);
        Add(OBSTACLE, 14, 19);

        Add(OBSTACLE, 16, 19);
        Add(OBSTACLE, 17, 19);

        Add(OBSTACLE, 19, 19);
        Add(OBSTACLE, 19, 18);

        Add(OBSTACLE, 19, 16);
        Add(OBSTACLE, 19, 15);

        Add(OBSTACLE, 19, 13);
        Add(OBSTACLE, 18, 13);

        Add(OBSTACLE, 16, 13);
        Add(OBSTACLE, 15, 13);

        // labirinto
        Add(OBSTACLE, 2, 32);
        Add(OBSTACLE, 3, 32);
        Add(OBSTACLE, 4, 32);
        Add(OBSTACLE, 5, 32);
        Add(OBSTACLE, 6, 32);
        Add(OBSTACLE, 7, 32);
        Add(OBSTACLE, 8, 32);
        Add(OBSTACLE, 9, 32);
        Add(OBSTACLE, 10, 32);
        Add(OBSTACLE, 10, 31);
        Add(OBSTACLE, 10, 30);
        Add(OBSTACLE, 10, 29);
        Add(OBSTACLE, 10, 28);
        Add(OBSTACLE, 10, 27);
        Add(OBSTACLE, 10, 26);
        Add(OBSTACLE, 10, 25);
        Add(OBSTACLE, 10, 24);
        Add(OBSTACLE, 10, 23);
        Add(OBSTACLE, 2, 23);
        Add(OBSTACLE, 3, 23);
        Add(OBSTACLE, 4, 23);
        Add(OBSTACLE, 5, 23);
        Add(OBSTACLE, 6, 23);
        Add(OBSTACLE, 7, 23);
        Add(OBSTACLE, 8, 23);
        Add(OBSTACLE, 9, 23);
        Add(OBSTACLE, 2, 29);
        Add(OBSTACLE, 2, 28);
        Add(OBSTACLE, 2, 27);
        Add(OBSTACLE, 2, 26);
        Add(OBSTACLE, 2, 25);
        Add(OBSTACLE, 2, 24);
        Add(OBSTACLE, 2, 23);
        Add(OBSTACLE, 2, 29);
        Add(OBSTACLE, 3, 29);
        Add(OBSTACLE, 4, 29);
        Add(OBSTACLE, 5, 29);
        Add(OBSTACLE, 6, 29);
        Add(OBSTACLE, 7, 29);
        Add(OBSTACLE, 7, 28);
        Add(OBSTACLE, 7, 27);
        Add(OBSTACLE, 7, 26);
        Add(OBSTACLE, 7, 25);
        Add(OBSTACLE, 6, 25);
        Add(OBSTACLE, 5, 25);
        Add(OBSTACLE, 4, 25);
        Add(OBSTACLE, 4, 26);
        Add(OBSTACLE, 4, 27);
        SetInitialNbGolds(CountObjects(GOLD));
    }

}
