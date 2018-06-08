using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistAndEnvLayersManager : TileColorManager {

    public GameObject alephButtonPrefab;

    private List<GameObject>[] tilesInMistLayer;
    private List<GameObject>[] popUpsListArray;



	// Use this for initialization
	new void Start () {
        base.Start();
        InitTilesInQuadMist();
        InitPopUpsListArray();
        GetTileCopiesFromScene();

        //SetTileGoldMaterial(20, 20);

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void InitTilesInQuadMist()
    {
        int nbQuads = AMWRef.GetNbOfQuadrants();
        tilesInMistLayer = new List<GameObject>[nbQuads];

        for (int i = 0; i < nbQuads; i++)
        {
            tilesInMistLayer[i] = new List<GameObject>();
        }
    }

    void GetTileCopiesFromScene()
    {
        int h = AMWRef.GetHeight();
        int w = AMWRef.GetWidth();
        int nQuad, nbTilesRowCol;

        GameObject cellObject;

        for(int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                string objectName = "MistLayer/TileBoard/SM_Env_FloorTile_01(Clone) (" + ((i * 36) + j + 1) + ")";
                cellObject = GameObject.Find(objectName);
                nQuad = AMWRef.GetQuad(i, j);

                // Copies original colors from the environment tile layer
                nbTilesRowCol = AMWRef.GetNbOfTilesInARowOrColumn();
                int x = i % nbTilesRowCol;
                int y = j % nbTilesRowCol;
                cellObject.GetComponent<Renderer>().material = tilesMaterialMatrix[nQuad, (nbTilesRowCol * x) + y];

                tilesInMistLayer[nQuad].Add(cellObject);
            }
        }
    }

    private void InitPopUpsListArray()
    {
        int nbQuads = AMWRef.GetNbOfQuadrants();
        popUpsListArray = new List<GameObject>[nbQuads];

        for (int i = 0; i < nbQuads; i++)
        {
            popUpsListArray[i] = new List<GameObject>();
        }
    }



    public new void SetTileGoldMaterial(int x, int y)
    {
        SetTileMaterial(x, y, goldMat);
        SetGoldPopUp(x, y);
    }

    public void SetGoldPopUp(int x, int y)
    {
        int quadrant = AMWRef.GetQuad(x, y);
        int nbTilesRowCol = AMWRef.GetNbOfTilesInARowOrColumn();

        GameObject popUpIcon = Instantiate(alephButtonPrefab) as GameObject;
        popUpIcon.GetComponent<AlephIconController>().SetPoint(x, y);

        x = x % nbTilesRowCol;
        y = y % nbTilesRowCol;
        GameObject tile = tilesInMistLayer[quadrant][(nbTilesRowCol * x) + y];
        popUpIcon.transform.position = tile.GetComponent<Renderer>().bounds.center + new Vector3(0, 2f, 0);
        popUpsListArray[quadrant].Add(popUpIcon);
    }

    public new void RestoreTileMaterial(int x, int y)
    {
        int q = AMWRef.GetQuad(x, y);

        RemoveGoldPopUp(x, y);

        Material mat = quadMaterialArray[q];
        SetTileMaterial(x, y, mat);
    }

    private void RemoveGoldPopUp(int x, int y)
    {
        int quadrant = AMWRef.GetQuad(x, y);

        int length = popUpsListArray[quadrant].Count;
        for (int i = 0; i < length; i++)
        {
            GameObject popUpIcon = popUpsListArray[quadrant][i];
            if (popUpIcon != null && popUpIcon.GetComponent<AlephIconController>().IsOnPoint(x, y))
            {
                popUpsListArray[quadrant].RemoveAt(i);
                Destroy(popUpIcon);
            }
        }
    }

    private new void SetTileMaterial(int x, int y, Material mat)
    {
        int quadrant = AMWRef.GetQuad(x, y);
        int nbTilesRowCol = AMWRef.GetNbOfTilesInARowOrColumn();
        x = x % nbTilesRowCol;
        y = y % nbTilesRowCol;

        SetEnvironmentLayerTileChanges(quadrant, (nbTilesRowCol * x) + y, mat);
        SetMistLayerTileChanges(quadrant, (nbTilesRowCol * x) + y, mat);
        tilesMaterialMatrix[quadrant, (nbTilesRowCol * x) + y] = mat;
    }


    // MARK A QUAD

    public new void SetMarkedMaterial(int quadrant)
    {
        int nbTiles = listTiles[quadrant].Count;
        for (int i = 0; i < nbTiles; i++)
        {
            // If tile color is the same as quad, change color, because it isn't an special colored tile
            if (quadMaterialArray[quadrant].Equals(tilesMaterialMatrix[quadrant, i]))
            {
                SetEnvironmentLayerTileChanges(quadrant, i, markedMat);
                SetMistLayerTileChanges(quadrant, i, markedMat);
            }
        }
    }

    public new void RestoreMarkedMaterial(int quadrant)
    {
        Material mat;
        int nbTiles = listTiles[quadrant].Count;
        for (int i = 0; i < nbTiles; i++)
        {
            mat = tilesMaterialMatrix[quadrant, i];
            SetEnvironmentLayerTileChanges(quadrant, i, mat);
            SetMistLayerTileChanges(quadrant, i, mat);
        }
    }

    // QUAD COLOR CHANGE

    public new void SetDefaultMaterial(int quadrant)
    {
        SetMaterial(quadrant, defaultMat);
    }

    public new void SetGoldMaterial(int quadrant)
    {
        SetMaterial(quadrant, goldMat);
    }

    public new void SetDangerMaterial(int quadrant)
    {
        SetMaterial(quadrant, dangerMat);
    }

    public new void SetMaterial(int quadrant, Material mat)
    {
        int nbTiles = listTiles[quadrant].Count;
        for (int i = 0; i < nbTiles; i++)
        {
            // If tile color is the same as quad, change color, because it isn't an special colored tile
            if (quadMaterialArray[quadrant].Equals(tilesMaterialMatrix[quadrant, i]))
            {
                // Tile material is updated
                tilesMaterialMatrix[quadrant, i] = mat;
                SetEnvironmentLayerTileChanges(quadrant, i, mat);
                SetMistLayerTileChanges(quadrant, i, mat);
            }
        }

        // Quadrant material is updated in order to being restored with RestoreMaterial()
        quadMaterialArray[quadrant] = mat;
    }

    private void SetMistLayerTileChanges(int quadrant, int tile, Material mat)
    {
        GameObject mistTile = tilesInMistLayer[quadrant][tile];
        mistTile.GetComponent<Cell>().GetRenderer().material = mat;
    }
}
