using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorManager : MonoBehaviour {
    public AlephMinersWorld AMWRef;
    protected List<GameObject>[] listTiles;

    public Material markedMat;
    public Material defaultMat;
    public Material dangerMat;
    public Material goldMat;
    protected Material[,] tilesMaterialMatrix;
    protected Material[] quadMaterialArray;

    // Use this for initialization
    protected void Start()
    {
        int nbQuads = AMWRef.GetNbOfQuadrants();
        int nbTilesRowCol = AMWRef.GetNbOfTilesInARowOrColumn();
        listTiles = new List<GameObject>[nbQuads];
        tilesMaterialMatrix = new Material[nbQuads, nbTilesRowCol * nbTilesRowCol];

        listTiles = AMWRef.tilesInQuad;

        InitQuadMaterialArray(); 
        InitTilesMaterialMatrix();
    }

    private void InitQuadMaterialArray()
    {
        int nbQuads = AMWRef.GetNbOfQuadrants();
        quadMaterialArray = new Material[nbQuads];

        for (int i = 0; i < nbQuads; i++)
        {
            quadMaterialArray[i] = defaultMat;
        }
    }

    private void InitTilesMaterialMatrix()
    {
        int height = AMWRef.GetHeight();
        int width = AMWRef.GetWidth();

        int nbQuads = AMWRef.GetNbOfQuadrants();
        int nbTilesRowCol = AMWRef.GetNbOfTilesInARowOrColumn();
        for (int i = 0; i < nbQuads; i++)
        {
            for (int j = 0; j < nbTilesRowCol*nbTilesRowCol; j++)
            {
                tilesMaterialMatrix[i, j] = AMWRef.tilesInQuad[i][j].GetComponent<Cell>().GetRenderer().sharedMaterial;
            }
        }
    }

    /*public void ResetMaterial(int quadrant)
    {
        Material mat = quadMaterialArray[quadrant];
        SetMaterial(quadrant, mat);
        
    }*/

    public void SetTileGoldMaterial(int x, int y)
    {
        SetTileMaterial(x, y, goldMat);
    }

    public void RestoreTileMaterial(int x, int y)
    {
        int q = AMWRef.GetQuad(x, y);
        Material mat = quadMaterialArray[q];
        SetTileMaterial(x, y, mat);
    }

    protected void SetTileMaterial(int x, int y, Material mat)
    {
        int quadrant = AMWRef.GetQuad(x, y);
        int nbTilesRowCol = AMWRef.GetNbOfTilesInARowOrColumn();
        x = x % nbTilesRowCol;
        y = y % nbTilesRowCol;
        /*GameObject tile = AMWRef.tilesInQuad[quadrant][(nbTilesRowCol * x) + y];
        tile.GetComponent<Cell>().GetRenderer().material = mat;*/
        SetEnvironmentLayerTileChanges(quadrant, (nbTilesRowCol * x) + y, mat);
        tilesMaterialMatrix[quadrant, (nbTilesRowCol * x) + y] = mat;
    }


    // MARK A QUAD

    public void SetMarkedMaterial(int quadrant)
    {
        int nbTiles = listTiles[quadrant].Count;
        for (int i = 0; i < nbTiles; i++)
        {
            // If tile color is the same as quad, change color, because it isn't an special colored tile
            if (quadMaterialArray[quadrant].Equals(tilesMaterialMatrix[quadrant, i]))
                SetEnvironmentLayerTileChanges(quadrant, i, markedMat);
        }
    }

    public void RestoreMarkedMaterial(int quadrant)
    {
        Material mat;
        int nbTiles = listTiles[quadrant].Count;
        for (int i = 0; i < nbTiles; i++)
        {
            mat = tilesMaterialMatrix[quadrant, i];
            SetEnvironmentLayerTileChanges(quadrant, i, mat);
        }
    }

    // QUAD COLOR CHANGE

    public void SetDefaultMaterial(int quadrant)
    {
        SetMaterial(quadrant, defaultMat);
    }

    public void SetGoldMaterial(int quadrant)
    {
        SetMaterial(quadrant, goldMat);
    }

    public void SetDangerMaterial(int quadrant)
    {
        SetMaterial(quadrant, dangerMat);
    }

    protected void SetMaterial(int quadrant, Material mat)
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
            }
        }

        // Quadrant material is updated in order to being restored with RestoreMaterial()
        quadMaterialArray[quadrant] = mat;
    }

    protected void SetEnvironmentLayerTileChanges(int quadrant, int tileIdx, Material mat)
    {
        GameObject tile = AMWRef.tilesInQuad[quadrant][tileIdx];
        tile.GetComponent<Cell>().GetRenderer().material = mat;
    }
}
