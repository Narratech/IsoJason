using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public GameObject tablero;
    public GameObject mainCamera;
    private GoldMinersWorld tableroScript;
	// Use this for initialization
	void Start () {
        tableroScript = tablero.GetComponent<GoldMinersWorld>();
        float xDim = tableroScript.GetWidth();
        float yDim = tableroScript.GetHeight();
        float x, y;
        x = (float)((xDim / 2) - 0.5);
        y = (float)((yDim / 2) - 0.5);
        // Set camera at the center of the board
        transform.position = transform.position + new Vector3(x, y, 0);
        // Set size to fit the board in screen
        Camera.main.orthographicSize = Mathf.Max(xDim, yDim) / 2 + (float)0.5; ;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        
	}
}
