using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWorld : MonoBehaviour {

    protected int width, height, nbAgs;

    public const int CLEAN = 0;
    public const int AGENT = 2;
    public const int OBSTACLE = 4;

    protected int[,] data = null;
    protected Location[] agPos;

    protected Random random = new Random();


    public void Initialize(int width, int height, int nbAgs) {
        this.width = width;
        this.height = height;
        this.nbAgs = nbAgs;

        data = new int[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                data[i, j] = CLEAN;
            }
        }

        agPos = new Location[nbAgs];
        for (int i = 0; i < agPos.Length; i++) {
            agPos[i] = new Location(-1, -1);
        }
    }

    public class Location
    {
        public int x;
        public int y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Location Clone()
        {
            return new Location(this.x, this.y);
        }
    };

    /*
    protected GridWorld(int w, int h, int nbAgs)
    {
        width = w;
        height = h;

        // int data
        data = new int[width,height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                data[i,j] = CLEAN;
            }
        }

        agPos = new Location[nbAgs];
        for (int i = 0; i < agPos.Length; i++)
        {
            agPos[i] = new Location(-1, -1);
        }
    }*/

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public int [,] GetData() {
        return data;
    }

    public int GetNbOfAgs()
    {
        return agPos.Length;
    }

    public bool InGrid(Location l)
    {
        return InGrid(l.x, l.y);
    }

    public bool InGrid(int x, int y)
    {
        return y >= 0 && y < height && x >= 0 && x < width;
    }

    public bool HasObject(int obj, Location l)
    {
        return InGrid(l.x, l.y) && (data[l.x,l.y] & obj) != 0;
    }
    public bool HasObject(int obj, int x, int y)
    {
        return InGrid(x, y) && (data[x,y] & obj) != 0;
    }

    // gets how many objects of some kind are in the grid
    public int CountObjects(int obj)
    {
        int c = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (HasObject(obj, i, j))
                {
                    c++;
                }
            }
        }
        return c;
    }

    public void Set(int value, int x, int y)
    {
        data[x,y] = value;
    }

    public virtual void Add(int value, Location l)
    {
        Add(value, l.x, l.y);
    }

    public void Add(int value, int x, int y)
    {
        data[x,y] |= value;
    }

    public void AddWall(int x1, int y1, int x2, int y2)
    {
        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                Add(OBSTACLE, x, y);
            }
        }
    }

    public virtual void Remove(int value, Location l)
    {
        Remove(value, l.x, l.y);
    }

    public void Remove(int value, int x, int y)
    {
        data[x,y] &= ~value;
    }

    public void SetAgPos(int ag, Location l)
    {
        Location oldLoc = GetAgPos(ag);
        if (oldLoc != null)
        {
            Remove(AGENT, oldLoc.x, oldLoc.y);
        }
        agPos[ag] = l;
        Add(AGENT, l.x, l.y);
    }

    public virtual void SetAgPos(int ag, int x, int y)
    {
        SetAgPos(ag, new Location(x, y));
    }

    public Location GetAgPos(int ag)
    {
        try
        {
            if (agPos[ag].x == -1)
                return null;
            else
                return (Location)agPos[ag].Clone();
        }
        catch (System.Exception e)
        {
            return null;
        }
    }

    /** returns the agent at location l or -1 if there is not one there */
    public int GetAgAtPos(Location l)
    {
        return GetAgAtPos(l.x, l.y);
    }

    /** returns the agent at x,y or -1 if there is not one there */
    public int GetAgAtPos(int x, int y)
    {
        for (int i = 0; i < agPos.Length; i++)
        {
            if (agPos[i].x == x && agPos[i].y == y)
            {
                return i;
            }
        }
        return -1;
    }

    /** returns true if the location l has no obstacle neither agent */
    public bool IsFree(Location l)
    {
        return IsFree(l.x, l.y);
    }

    /** returns true if the location x,y has neither obstacle nor agent */
    public bool IsFree(int x, int y)
    {
        return InGrid(x, y) && (data[x,y] & OBSTACLE) == 0 && (data[x,y] & AGENT) == 0;
    }

    /** returns true if the location l has not the object obj */
    public bool IsFree(int obj, Location l)
    {
        return InGrid(l.x, l.y) && (data[l.x,l.y] & obj) == 0;
    }
    /** returns true if the location x,y has not the object obj */
    public bool IsFree(int obj, int x, int y)
    {
        return InGrid(x, y) && (data[x,y] & obj) == 0;
    }

    public bool IsFreeOfObstacle(Location l)
    {
        return IsFree(OBSTACLE, l);
    }
    public bool IsFreeOfObstacle(int x, int y)
    {
        return IsFree(OBSTACLE, x, y);
    }

    /** returns a random free location using isFree to test the availability of some possible location (it means free of agents and obstacles) */
    protected Location GetFreePos()
    {
        for (int i = 0; i < (GetWidth() * GetHeight() * 5); i++)
        {
            int x = Random.Range(0, GetWidth());
            int y = Random.Range(0, GetHeight());
            Location l = new Location(x, y);
            if (IsFree(l))
            {
                return l;
            }
        }
        return null; // not found
    }

    /** returns a random free location using isFree(object) to test the availability of some possible location */
    protected Location GetFreePos(int obj)
    {
        for (int i = 0; i < (GetWidth() * GetHeight() * 5); i++)
        {
            int x = Random.Range(0, GetWidth());
            int y = Random.Range(0, GetHeight());
            Location l = new Location(x, y);
            if (IsFree(obj, l))
            {
                return l;
            }
        }
        return null; // not found
    }
}
