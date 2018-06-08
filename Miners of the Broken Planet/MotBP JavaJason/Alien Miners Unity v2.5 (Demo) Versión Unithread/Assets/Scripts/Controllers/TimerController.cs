using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

    public AlephMinersWorld AMWRef;

    public Text time;
    public Text gameOver;

    private bool end = false;

    private float minutes;
    private float seconds;
    private float millisecs;


	// Use this for initialization
	void Start () {
        minutes = 2;
        seconds = 0;
        millisecs = 0;

        gameOver.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (!end)
        {
            TimeLeft();

            if (AMWRef.GetGoldsInDepot() >= 5)
            {
                DisplayGameOverText("MISIÓN SUPERADA");
                end = true;
            }
            else if (minutes <= 0 && seconds <= 0)
            {
                DisplayGameOverText("HAS FRACASADO");
                end = true;
            }
        }
    }

    private void DisplayGameOverText(string v)
    {
        gameOver.text = v;
    }

    private void TimeLeft()
    {
        if (millisecs <= 0)
        {
            if (seconds <= 0)
            {
                minutes--;
                seconds = 59;
            }
            else if (seconds >= 0)
            {
                seconds--;
            }

            millisecs = 100;
        }

        millisecs -= Time.deltaTime * 100;

        //Debug.Log(string.Format("{0}:{1}:{2}", minutes, seconds, (int)miliseconds));
        time.text = string.Format("{0}:{1}:{2}", minutes, seconds, (int)millisecs);
    }
}
