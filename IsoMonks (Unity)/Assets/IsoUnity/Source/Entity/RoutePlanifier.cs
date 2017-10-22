using UnityEngine;
using System.Collections.Generic;

public class RoutePlanifier
{

    private static Dictionary<Mover, Stack<Cell>> routes = new Dictionary<Mover, Stack<Cell>>();

    public static bool planifyRoute(Mover mover, Cell destination)
    {
        return planifyRoute(mover, destination, 0);
	}

    public static bool planifyRoute(Mover mover, Cell destination, int distance)
    {
        if (routes.ContainsKey(mover))
        {
			//return false;
            Stack<Cell> ruta = calculateRoute(routes[mover].Peek(), destination, mover, distance);
			//Stack<Cell> ruta = new Stack<Cell>();
			//ruta.Push (destination);
            ruta.Push(routes[mover].Peek());
            routes[mover] = ruta;
            return ruta != null;
		}else{
			/*Stack<Cell> ruta = new Stack<Cell>();
			ruta.Push (destination);*/
            Stack<Cell> ruta = calculateRoute(mover.Entity.Position, destination, mover, distance);

			if(ruta!=null){
                ruta.Push(mover.Entity.Position);
				//ruta.Pop(); //Quito en la que estoy
                routes.Add(mover, ruta);
			}

			return ruta!=null;
		}
	}

    public static Cell next(Mover mover)
    {
		Cell nextCell = null;

        if (routes.ContainsKey(mover))
        {
            if (routes[mover].Count == 0)
                routes.Remove(mover);
			else{
                if (routes[mover].Peek() != mover.Entity.Position)
                    routes.Remove(mover);
				else{
                    routes[mover].Pop();
                    if (routes[mover].Count == 0)
                        routes.Remove(mover);
					else
                        nextCell = routes[mover].Peek();
				}

			}
		}
		return nextCell;
	}

	public static void cancelRoute(Entity entity){
		/*if(routes.ContainsKey(entity)){
			routes[entity].Pop();
			return routes[entity].Peek();
		}*/
	}

	private static void reconstruyeCamino(Stack<Cell> route, int celda, Cell[] anterior, Cell[] celdas, Dictionary<Cell,int> cellToPos)
	{
		int posAnterior = -1;
		if (anterior[celda] != null) 
			posAnterior = cellToPos[anterior[celda]];

		if(posAnterior!=-1){
			route.Push(celdas[celda]);
			reconstruyeCamino(route, posAnterior, anterior,celdas,cellToPos);
		}
	}

	private static List<Cell> GetSurroundCellsAtRadius(Cell to, int distance){
		List<Cell> cells = new List<Cell>();
		GetSurroundCellsAtRadius(to, distance, cells);
		return cells;
	}

	private static void GetSurroundCellsAtRadius(Cell to, int distance, List<Cell> cells){
		if(distance < 0)
			return;

		if(!cells.Contains(to)){
			cells.Add(to);
			foreach(Cell c in to.Map.getNeightbours(to))
				if(c!= null)
					GetSurroundCellsAtRadius(c, distance-1, cells);
		}
	}

    private static Stack<Cell> calculateRoute(Cell from, Cell to, Mover mover, int distance)
    {
			
		Cell[] cells = from.Map.GetComponentsInChildren<Cell>();
		Dictionary<Cell,int> cellToPos = new Dictionary<Cell, int>();
		for(int i = 0; i<cells.Length; i++)
			cellToPos[cells[i]] = i;

		Heap<float> abierta = new Heap<float>(cells.Length);

		bool[] cerrada = new bool[cells.Length];
		for (int i = 0; i < cells.Length; i++)
			cerrada[i] = false;
		
		float[] f = new float[cells.Length];
		float[] g = new float[cells.Length];
		Cell[] anterior = new Cell[cells.Length];
		for (int i = 0; i < cells.Length; i++)
			anterior[i] = null;
		
		int posInicial = cellToPos[from];
		
		g[posInicial] = 0;
		f[posInicial] = estimaMeta(from, to);
		anterior[posInicial] = null;
		abierta.push(posInicial + 1, f[posInicial]);

		List<Cell> ends = GetSurroundCellsAtRadius(to, distance);
		
		while (!abierta.isEmpty()){
			
			int candidata = abierta.top().elem - 1;
			abierta.pop();
			Cell celdaCandidata = cells[candidata];
			
			if (ends.Contains(celdaCandidata)){
				Stack<Cell> ruta = new Stack<Cell>();
				reconstruyeCamino(ruta, candidata, anterior, cells, cellToPos);
				return ruta;
			}

            Cell[] accesibles = getCeldasAccesibles(celdaCandidata, mover);
			cerrada[candidata] = true;
			foreach (Cell accesible in accesibles)
			{
				int posAccesible = cellToPos[accesible];
				
				float posibleG = g[candidata] + estimaAvance(celdaCandidata, accesible);
				float posibleF = posibleG + estimaMeta(accesible, to);
				
				if (cerrada[posAccesible] && posibleF >= f[posAccesible])
					continue;
				
				if (!abierta.contains(posAccesible + 1) || posibleF < f[posAccesible])
				{
					anterior[posAccesible] = celdaCandidata;
					g[posAccesible] = posibleG;
					f[posAccesible] = posibleF;
					abierta.modify(posAccesible + 1, f[posAccesible]); // Modifica inserta si no esta dentro
				}
			}
		}
		
		return null;
	}

    private static Cell[] getCeldasAccesibles(Cell celda, Mover mover)
    {

		Cell[] vecinas = celda.Map.getNeightbours(celda);
		List<Cell> accesibles = new List<Cell>();
		foreach(Cell c in vecinas){
			if(c!=null){
                if (mover.CanMoveTo(celda, c))
					accesibles.Add(c);
			}
		}

		return accesibles.ToArray() as Cell[];
	}



	private static float estimaAvance(Cell celdaCandidata, Cell accesible){
		return Vector3.Distance(celdaCandidata.transform.position, accesible.transform.position);
	}

	private static float estimaMeta(Cell accesible, Cell to){
		return Vector3.Distance(accesible.transform.position, to.transform.position);
	}
}

