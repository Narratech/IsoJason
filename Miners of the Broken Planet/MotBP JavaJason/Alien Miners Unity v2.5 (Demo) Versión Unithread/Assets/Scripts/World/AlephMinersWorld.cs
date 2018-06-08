using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlephMinersWorld : GoldMinersWorld {

    const int nbQuads = 9;
    const int dimQuad = 12;
    public List<GameObject> [] tilesInQuad = new List<GameObject>[nbQuads];
    private string[] agClasses;
    public GameObject[] classModels = new GameObject[3];

    private string[] agPersonalities;

    [HideInInspector]
    public GameObject[] agModels;
    public GameObject enemyModel;

    [HideInInspector]
    public GameObject[] enModels;
    private int nbEnemies = 2;
    private Location[] enPos;

    public void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    public override void Start ()
    {
        InitTilesInQuad();
        InitAgents();
        InitEnemies();
        agWithGold = new HashSet<int>();
        enPos = new Location[nbEnemies];

        switch (simID)
        {
            case 3:
                World3();
                break;
            default:
                World3();
                break;
        }

        //PlaceGold();
        //PlaceObstacles();
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        GetComponent<AMWorldUpdater>().CheckEvents();
        textCount.text = "Aleph in depot: " + goldsInDepot.ToString();
    }


    // INITIALIZATION METHODS
    private void InitAgents()
    {
        InitAgentsClasses();
        InitAgentsPersonalities();
    }
    private void InitAgentsClasses()
    {
        agClasses = new string[5];
        // Class of each miner
        agClasses[0] = "soldier";
        agClasses[1] = "explorer";
        agClasses[2] = "explorer";
        agClasses[3] = "collector";
        agClasses[4] = "collector";

        agModels = new GameObject[agClasses.Length];

        for(int i = 0; i < agModels.Length; i++)
        {
            if (agClasses[i] == "soldier")
                agModels[i] = Instantiate(classModels[0]) as GameObject;
            else if (agClasses[i] == "explorer")
                agModels[i] = Instantiate(classModels[1]) as GameObject;
            else if (agClasses[i] == "collector")
                agModels[i] = Instantiate(classModels[2]) as GameObject;
        }
    }
    
    private void InitAgentsPersonalities()
    {
        agPersonalities = new string[5];
        // Class of each miner
        agPersonalities[0] = "coward";
        agPersonalities[1] = "renegade";
        agPersonalities[2] = "dissident";
        agPersonalities[3] = "renegade";
        agPersonalities[4] = "coward";
    }

    private void InitEnemies()
    {
        enModels = new GameObject[nbEnemies];
        for (int i = 0; i < enModels.Length; i++)
        {
            enModels[i] = Instantiate(enemyModel) as GameObject;
        }
    }

    public void InitTilesInQuad()
    {
        for(int i = 0; i < nbQuads; i++)
        {
            tilesInQuad[i] = new List<GameObject>();
        }
    }

    // ALIEN MINERS INIT METHODS

    protected new void InstantiateCells()
    {
        cellsBoard = new Cell[width, height];
        int nQuad = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                nQuad = GetQuad(i, j);
                //We wake up (Cell.Awake() ) this new Cell by making an instance of CellPrefab
                GameObject cellObject = Instantiate(cellPrefab) as GameObject;
                cellsBoard[i, j] = InstantiateCell(i, j, nQuad, cellObject);
                tilesInQuad[nQuad].Add(cellObject);
            }
        }
    }

    public int GetQuad(int x, int y)
    {
        return (y / dimQuad * 3) + (x / dimQuad);
    }

    protected Cell InstantiateCell(int x, int y, int nQuad, GameObject cellObject)
    {
        // Sets the position every Cell.gold will take as reference for calculating their own position (at their Start() )
        Vector3 cellSize = cellObject.GetComponent<Renderer>().bounds.size;
        cellObject.transform.position = new Vector3(y * cellSize.x, 0f, x * cellSize.y);
        cellObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        // Sets the quadrant to which belongs the cell
        cellObject.GetComponent<Cell>().SetQuad(nQuad);
        cellObject.GetComponent<Cell>().SetPosition(x, y);

        Cell celdaScript = cellObject.GetComponent<Cell>();
        return celdaScript;
    }

    protected void PlaceAliens()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (data[i, j] == ENEMY)
                    PlaceOnView(GoldMinersWorld.ENEMY, i, j);
            }
        }
    }


    // AGENT ACTIONS

    public new bool Move(Direction dir, int ag)
    {
        Location l = GetAgPos(ag);
        switch (dir)
        {
            case Direction.UP:
                if (IsFree(l.x, l.y - 1))
                {
                    SetAgPos(ag, l.x, l.y - 1);
                }
                else
                    return false;
                break;
            case Direction.DOWN:
                if (IsFree(l.x, l.y + 1))
                {
                    SetAgPos(ag, l.x, l.y + 1);
                }
                else
                    return false;
                break;
            case Direction.RIGHT:
                if (IsFree(l.x + 1, l.y))
                {
                    SetAgPos(ag, l.x + 1, l.y);
                }
                else
                    return false;
                break;
            case Direction.LEFT:
                if (IsFree(l.x - 1, l.y))
                {
                    SetAgPos(ag, l.x - 1, l.y);
                }
                else
                    return false;
                break;
        }
        return true;
    }

    public bool MoveEnemy(Direction dir, int ag)
    {
        Location l = GetAlPos(ag);
        int x = l.x;
        int y = l.y;
        switch (dir)
        {
            case Direction.UP:
                if (IsFree(OBSTACLE, x, y - 1) && IsFreeOfAlien(x, y - 1))
                {
                    SetEnPos(ag, x, y - 1);
                }
                else
                    return false;
                break;
            case Direction.DOWN:
                if (IsFree(OBSTACLE, x, y + 1) && IsFreeOfAlien(x, y + 1))
                {
                    SetEnPos(ag, x, y + 1);
                }
                else
                    return false;
                break;
            case Direction.RIGHT:
                if (IsFree(OBSTACLE, x + 1, y) && IsFreeOfAlien(x + 1, y))
                {
                    SetEnPos(ag, x + 1, y);
                }
                else
                    return false;
                break;
            case Direction.LEFT:
                if (IsFree(OBSTACLE, x - 1, y) && IsFreeOfAlien(x - 1, y))
                {
                    SetEnPos(ag, x - 1, y);
                }
                else
                    return false;
                break;
        }
        return true;
    }

    private void MoveOnView(int obj, int ag, Location oldLoc, Location newLoc)
    {
        Vector3 startMarker = new Vector3(oldLoc.y * 3 - 1.5f, 0, oldLoc.x * 3 - 1.5f);
        Vector3 endMarker = new Vector3(newLoc.y * 3 - 1.5f, 0, newLoc.x * 3 - 1.5f);
        int xDif = newLoc.x - oldLoc.x;
        int yDif = newLoc.y - oldLoc.y;

        if (obj == AGENT)
            startMarker = agModels[ag].transform.position;
        
        else if (obj == ENEMY)
            startMarker = enModels[ag].transform.position;
        
        //endMarker = startMarker + new Vector3(xDif * 3, 0f, yDif * 3);
        StartCoroutine(Movement(obj, ag, startMarker, endMarker));
    }

    IEnumerator Movement(int obj, int ag, Vector3 start, Vector3 end)
    {
        float time = 0, speed = 0.5f;//, timeToReach = Vector3.Distance(start, end)/speed;
        float lerpValue;
        GameObject character;

        if (obj == AGENT)
            character = agModels[ag];
        else //if (obj == ENEMY)
            character = enModels[ag];

        Vector3 targetPostition = new Vector3(end.x, character.transform.position.y, end.z);
        character.transform.LookAt(targetPostition);

        character.GetComponentInChildren<Animator>().SetFloat("Speed_f", 1);
        character.GetComponentInChildren<Animator>().SetBool("Static_b", true);

        // move object from start to end
        while ((character.transform.position != end) && (time < speed))
        {
            time += Time.deltaTime;
            lerpValue = (time / speed);
            character.transform.position = Vector3.Lerp(start, end, lerpValue);
            yield return null;
        }
        // set the position of to the endMarker
        character.transform.position = end;
        character.GetComponentInChildren<Animator>().SetFloat("Speed_f", 0);
    }

    private bool IsFreeOfAlien(int x, int y)
    {
        return (data[x, y] & ENEMY) == 0;
    }


    public bool Kill(int killerObj, int killerID, int targetID)
    {
        Location kilLoc;
        Location tarLoc;

        if (killerObj == AGENT)
        {
            kilLoc = GetAgPos(killerID);
            tarLoc = GetAlPos(targetID);
            if ((kilLoc.x == tarLoc.x) && (kilLoc.y == tarLoc.y))
            {
                if (agClasses[killerID] == "soldier")
                {
                    Remove(GoldMinersWorld.ENEMY, tarLoc);
                    enPos[targetID] = null;
                }

                return true;
            }
        }
        else if (killerObj == ENEMY)
        {
            kilLoc = GetAlPos(killerID);
            tarLoc = GetAgPos(targetID);
            if ((kilLoc.x == tarLoc.x) && (kilLoc.y == tarLoc.y))
            {
                if (agClasses[targetID] != "soldier")
                {
                    Remove(GoldMinersWorld.AGENT, tarLoc);
                    agPos[targetID] = null;

                    return true;
                }
            }
        }

        return false;
    }


    public bool Steal(int ag)
    {
        Location l = GetAgPos(ag);
        if (HasObject(GoldMinersWorld.GOLD, l))
        {
            if (!IsCarryingGold(ag))
            {
                Remove(GoldMinersWorld.GOLD, l);
                return true;
            }
            else
            {
                new Logger(logHandler).LogWarning("Steal", "Agent " + (ag + 1) + " is trying the pick gold, but it is already carrying gold!");
            }
        }
        else
        {
            new Logger(logHandler).LogWarning("Steal", "Agent " + (ag + 1) + " is trying the pick gold, but there is no gold at " + l.x + "x" + l.y + "!");
        }
        return false;
    }

    // GETTERS FOR CLASSES, IDs OR NAMES OF ALIENS, AND ENVIRONMENT INFO

    public string GetAgClass(int ag)
    {
        return agClasses[ag];
    }

    public string GetAgPersonality(int ag)
    {
        return agPersonalities[ag];
    }

    public int GetNbOfAliens()
    {
        return nbEnemies;
    }

    /* public string GetAlBasedOnPos(int x, int y)
     {
         Location l;
         string alienName = "";
         bool found = false;

         int i = -1;
         while (i < alPos.Length && !found)
         {
             i++;
             l = GetAlPos(i);
             if (l != null)
             {
                 if (l.x.Equals(x) && l.y.Equals(y))
                     found = true;
             }
         }

         if (found)
         {
             alienName = "alien" + (i + 1).ToString();
         }

         return alienName;
     }*/

    public new int GetAgIdBasedOnName(string agName)
    {
        return (Convert.ToInt32(agName.Substring(5))) - 1;
    }

    public string GetAgNameBasedOnId(int id)
    {
        return "miner" + id;
    }

    public int GetEnIdBasedOnName(string enName)
    {
        return (Convert.ToInt32(enName.Substring(5))) - 1;
    }
    public string GetEnNameBasedOnId(int id)
    {
        return "alien" + id;
    }
    public string GetEnemyNameBasedOnPos(int x, int y)
    {
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();

        //return world.GetAlBasedOnPos(x, y);
        return world.GetNameBasedOnPos(AlephMinersWorld.ENEMY, x, y);
    }

    public string GetAgentNameBasedOnPos(int x, int y)
    {
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();

        //return world.GetAlBasedOnPos(x, y);
        return world.GetNameBasedOnPos(AlephMinersWorld.AGENT, x, y);
    }


    public string GetNameBasedOnPos(int obj, int x, int y)
    {
        Location l;
        string name = "";
        bool found = false;
        Location[] aArray;

        if (obj == AGENT)
            aArray = agPos;
        else
            aArray = enPos;

        int i = -1;
        while (i < aArray.Length && !found)
        {
            i++;
            l = aArray[i];
            if (l != null)
            {
                if (l.x.Equals(x) && l.y.Equals(y))
                    found = true;
            }
        }

        if (found)
        {
            if (obj == AGENT)
                name = "agent" + (i + 1).ToString();
            else if (obj == ENEMY)
                name = "alien" + (i + 1).ToString();
        }

        return name;
    }

    public Location GetAlPos(int ag)
    {
        try
        {
            if (enPos[ag].x == -1)
                return null;
            else
                return (Location)enPos[ag].Clone();
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public int GetNbOfQuadrants()
    {
        return nbQuads;
    }

    public int GetNbOfTilesInARowOrColumn()
    {
        return dimQuad;
    }

    public void GetQuadCoordinates(int quad, out int x1, out int y1, out int x2, out int y2)
    {
        x1 = 0; y1 = 0; x2 = 0; y2 = 0;
        switch (quad)
        {
            case 0:
                x1 = 0; y1 = 0; x2 = 11; y2 = 11;
                break;
            case 1:
                x1 = 0; y1 = 12; x2 = 11; y2 = 23;
                break;
            case 2:
                x1 = 0; y1 = 24; x2 = 11; y2 = 35;
                break;
            case 3:
                x1 = 12; y1 = 0; x2 = 23; y2 = 11;
                break;
            case 4:
                x1 = 12; y1 = 12; x2 = 23; y2 = 23;
                break;
            case 5:
                x1 = 12; y1 = 24; x2 = 23; y2 = 35;
                break;
            case 6:
                x1 = 24; y1 = 0; x2 = 35; y2 = 11;
                break;
            case 7:
                x1 = 24; y1 = 12; x2 = 35; y2 = 23;
                break;
            case 8:
                x1 = 24; y1 = 24; x2 = 35; y2 = 35;
                break;
        }
    }

    //MODIFIERS OF THE ENVIRONMENT VIEW AND MODEL
    public new void SetAgPos(int ag, int x, int y)
    {
        Location oldLoc = GetAgPos(ag);

        base.SetAgPosGridWorld(ag, x, y);
        //Removes from the agent's old position and adds to the new one on the VIEW
        if (oldLoc == null)
            agModels[ag].transform.position = new Vector3(agPos[ag].y * 3 - 1.5f, 0, agPos[ag].x * 3 - 1.5f);
        else
        {
            Location newLoc = new Location(x, y);
            MoveOnView(AGENT, ag, oldLoc, newLoc);
        }
    }

    public void SetEnPos(int ag, int x, int y)
    {
        Location oldLoc = GetAlPos(ag);

        SetEnPosGridWorld(ag, new Location(x, y));
        //Removes from the agent's old position and adds to the new one on the VIEW
        if (oldLoc == null)
            enModels[ag].transform.position = new Vector3(enPos[ag].y * 3 - 1.5f, 0, enPos[ag].x * 3 - 1.5f);
        else
        {
            Location newLoc = new Location(x, y);
            MoveOnView(ENEMY, ag, oldLoc, newLoc);
        }
    }
    public void SetEnPosGridWorld(int ag, Location l)
    {
        Location oldLoc = GetAlPos(ag);
        if (oldLoc != null)
        {
            Remove(ENEMY, oldLoc.x, oldLoc.y);
        }
        enPos[ag] = l;
        Add(ENEMY, l.x, l.y);
    }

    //HARDCODED ENVIRONMENT

    public new void World3()
    {
        Initialize(36, 36, 5);
        InstantiateCells();
        SetDepot(16, 16);
        SetAgPos(0, 14, 20);
        SetAgPos(1, 20, 15);
        SetAgPos(2, 17, 20);
        SetAgPos(3, 20, 19);
        SetAgPos(4, 13, 15);
        SetEnPos(0, 11, 11);
        SetEnPos(1, 24, 0);
        Add(GOLD, 7, 20);
        Add(GOLD, 1, 10);
        Add(GOLD, 30, 5);
        Add(GOLD, 25, 30);
        Add(GOLD, 20, 29);
        Add(GOLD, 15, 21);
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
