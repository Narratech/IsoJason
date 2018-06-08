using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public AlephMinersWorld AMWRef;
    public MistAndEnvLayersManager MELCRef;
    public KeyboardController KBCRef;

    public Texture2D defaultTexture;
    public Texture2D alephTexture;
    public Texture2D uiTexture;
    public Texture2D quadrantsTexture;

    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    Ray ray;
    int uiLayerMask = 1 << 5;
    int tileLayerMask = 1 << 8;
    int alephIconsMask = 1 << 9;
    int hittedQuad = 0;
    int lastQuad = -1;

    bool checkedClick = false;
    int delayClick = 0;

    // Use this for initialization
    void Start () {
        Cursor.SetCursor(defaultTexture, hotSpot, cursorMode);
    }
	
	// Update is called once per frame
	void Update () {
        if (AlephIconsLayerRaycastHitter())
        {
            Cursor.SetCursor(alephTexture, hotSpot, cursorMode);
        }
        else if (delayClick <= 0 && TileLayerRaycastHitter()) // && !checkedClick )
        {
            Cursor.SetCursor(quadrantsTexture, hotSpot, cursorMode);
            SetNewQuadrantClickChecker();
        }
        else if (lastQuad != -1) {
            UnmarkLastQuadrant();
        }
        /*if (UILayerRaycastHitter())
        {
            Cursor.SetCursor(uiTexture, hotSpot, cursorMode);
        }*/
        else
            Cursor.SetCursor(defaultTexture, hotSpot, cursorMode);

        if (delayClick > 0)
            delayClick = delayClick - 1;

        checkedClick = false;
    }

    private bool TileLayerRaycastHitter()
    {
        bool hitted = false;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 300, tileLayerMask))
        {
            hitted = true;
            hittedQuad = hit.transform.GetComponent<Cell>().GetQuad();

            if (hittedQuad != lastQuad)
            {
                Debug.Log(hittedQuad);
                if (lastQuad != -1)
                    MELCRef.RestoreMarkedMaterial(lastQuad);

                MELCRef.SetMarkedMaterial(hittedQuad);

                lastQuad = hittedQuad;
            }
        }
        return hitted;
    }

    private void SetNewQuadrantClickChecker()
    {
        int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

        if (Input.GetMouseButtonDown(0)) {
            int key = KBCRef.GetPressedKey();
            // Each key means an NPC
            if (key >= 1 && key <= 5)
            {
                string agName = AMWRef.GetComponent<AlephMinersWorld>().GetAgNameBasedOnId(key);
                AMWRef.GetQuadCoordinates(hittedQuad, out x1, out y1, out x2, out y2);
                JasonInstructions.AddPercept(agName, "set_new_quadrant(quadrant(" + y1 + "," + x1 + "," + y2 + "," + x2 + "))");
                checkedClick = true;
            }
        }
    }

    private bool AlephIconsLayerRaycastHitter()
    {
        bool hitted = false;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500, alephIconsMask))
        {
            int key = KBCRef.GetPressedKey();

            if (AMWRef.GetAgClass(key - 1) == "collector")
            {
                SendToAlephClickChecker(hit);
                hitted = true;
            }
        }

        return hitted;
    }

    private void UnmarkLastQuadrant()
    {
        MELCRef.RestoreMarkedMaterial(lastQuad);
        lastQuad = -1;
    }

    private void SendToAlephClickChecker(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            hit.transform.GetComponent<AlephIconController>().SendPickAlephOrder();
            delayClick = 30;
            checkedClick = true;
        }
    }


    private bool UILayerRaycastHitter()
    {
        bool hitted = false;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        

        if (hit.transform.tag == "UI")
        {
            hitted = true;
        }

        return hitted;
    }

}
