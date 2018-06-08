﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMWorldUpdater : MonoBehaviour {

    static ILogHandler logHandler;
    Logger logger = new Logger(logHandler);

    private void Awake() {
        Connection.GetInstance().Initialize();
    }

    // Use this for initialization
    void Start () {
        Init();
	}

	// Executed when game stops
	void OnApplicationQuit(){
		EndSimulation();
		ExternalConnector.GetInstance().sendStopToMAS();
	}

    public void CheckEvents() {
		string[] msg = ExternalConnector.GetInstance().receiveFromMAS();

		if (msg[0] != "" && msg[1] != "") {
			JSONObject js = new JSONObject(msg[1]);
			string name = js["name"].str;

			JSONObject param = js["parameters"];

			if (msg [0] == "Environment") {
				if (name == "do") 
					ActionRequest (param);
			}
			else if (msg [0] == "Information"){
                if (name == "stateRequest")
                    MentalActionRequest(param);
                else if (name == "publishReceived")
                    PublishReceivedRequest(param);
                else if (name == "publishSent")
                    PublishSentRequest(param);
            }
        }
    }

    // ACTION REQUEST (ENVIRONMENT)
    private void ActionRequest(JSONObject param) {
        bool success = ParseAction(param);
        
        SendSuccededAction(success);

        string agName = param["who"].str;
        UpdateAgPercept(GetAgIdBasedOnName(agName));
    }

    private bool ParseAction(JSONObject param) {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();

        string direction = null;
        string actionName = param["otherParameters"].str;
        string agentName = param["who"].str;
        int ag = GetAgIdBasedOnName(agentName);

        if (actionName != "[pick]" && actionName != "[drop]" && actionName != "[skip]") {
            direction = actionName.Substring(1, actionName.Length - 2).ToUpper();
            actionName = "move";
        }
        else
            actionName = actionName.Substring(1, actionName.Length - 2);

        switch (actionName) {
            case "move":
                try {
                    GoldMinersWorld.Direction dir = (GoldMinersWorld.Direction)Enum.Parse(typeof(GoldMinersWorld.Direction), direction);
                    return world.Move(dir, ag);
                }
                catch {
                    return false;
                }
                break;

            case "pick":
                try {
                    world.Pick(ag);
                }
                catch {
                    return false;
                }
                break;

            case "drop":
                try {
                    world.Drop(ag);
                }
                catch {
                    return false;
                }
                break;
            case "skip":
                return true;
            default:
                return false;
        }
        return true;
    }

    private void SendSuccededAction(bool success) {
        if (success)
			SendEnvironmentChanges("{\"name\":\"actionResponse\",\"parameters\":{\"success\":\"yes\"}}");
        else
			SendEnvironmentChanges("{\"name\":\"actionResponse\",\"parameters\":{\"success\":\"no\"}}");
    }

    // MENTAL ACTION REQUEST (INFORMATION)
    private void MentalActionRequest(JSONObject param)
    {
        string worldState = "";
        bool success = ParseMentalAction(param, ref worldState);

        SendResultMentalAction(success, worldState);
    }

    private bool ParseMentalAction(JSONObject param, ref string worldState)
    {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();

        worldState = ToStringState(world, world.GetData());

        return true;
    }

    private string ToStringState(GoldMinersWorld world, int[,] state)
    {
        int w = world.GetWidth();
        int h = world.GetHeight();
        String msg = "(" + w + ")(" + h + ")[";

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                msg += state[i, j].ToString() + ",";
            }
        }

        // Removes the last " , "
        msg = msg.Substring(0, msg.Length - 1) + "]";

        return msg;
    }

    private void SendResultMentalAction(bool success, string worldState)
    {
        if (success)
            SendInformationResults("{\"name\":\"mentalActionResponse\",\"parameters\":{\"state\":\"" + worldState + "\",\"success\":\"yes\"}}");
        else
            SendInformationResults("{\"name\":\"mentalActionResponse\",\"parameters\":{\"state\":\"" + worldState + "\",\"success\":\"no\"}}");
    }

    // PUBLISH RECEIVED REQUEST (INFORMATION)
    private void PublishReceivedRequest(JSONObject param)
    {
        string str = param["message"].str;
        GetComponent<GoldMinersWorld>().textLeaderReceivedMsgs.GetComponent<TextLogControl>().LogText(str, Color.black);
	}

	// PUBLISH SENT REQUEST (INFORMATIOn)
	private void PublishSentRequest(JSONObject param)
    {
        string str = param["message"].str;
        GetComponent<GoldMinersWorld>().textLeaderSentMsgs.GetComponent<TextLogControl>().LogText(str, Color.black);
    }


    // WRAPPER
	private void SendEnvironmentChanges(string msg) {
		ExternalConnector.GetInstance().sendEnvironmentToMAS(msg);
	}
	private void SendInformationResults(string msg) {
		ExternalConnector.GetInstance().sendInformationToMAS(msg);
	}


    // INITIALIZATION OF MAS
    public void Init() {
        InitWorld();
		SendEnvironmentChanges("{\"name\":\"environmentdone\",\"parameters\":{}}");
    }

    private void InitWorld() {
        try
        {
            GoldMinersWorld world = GetComponent<GoldMinersWorld>();

            JasonInstructions.ClearPercepts(); 
            JasonInstructions.AddPercept("gsize(" + world.simID + "," + world.GetWidth() + "," + world.GetHeight() + ")");
            JasonInstructions.AddPercept("depot(" + world.simID + "," + world.GetDepot().x + "," + world.GetDepot().y + ")");


            UpdateAgsPercept();

            JasonInstructions.InformAgsEnvironmentChanged();
        }
        catch (System.Exception e)
        {
            logger.LogWarning("InitWorld", "Error creating world " + e);
        }
    }


    private void UpdateAgsPercept() {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();
        for (int i = 0; i < world.GetNbOfAgs(); i++) {
            UpdateAgPercept(i);
        }
    }

    private void UpdateAgPercept(int ag) {
        UpdateAgPercept("miner" + (ag + 1), ag);
    }


    private void UpdateAgPercept(string agName, int ag) {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();

		// Deletes what an agents knows about its perceptions (what it see)
        JasonInstructions.ClearPercepts(agName);

        // Agent perceive where it is
        GridWorld.Location l = world.GetAgPos(ag);
        JasonInstructions.AddPercept(agName, "pos(" + l.x + "," + l.y + ")");

        // If it's carring gold, it also perceives that
        if (world.IsCarryingGold(ag)) {
            JasonInstructions.AddPercept(agName, "carrying_gold");
        }

        // Agent perceives what it is in its own cell, and the 8 cells surrounding it.
        UpdateAgPercept(agName, l.x - 1, l.y - 1);
        UpdateAgPercept(agName, l.x - 1, l.y);
        UpdateAgPercept(agName, l.x - 1, l.y + 1);
        UpdateAgPercept(agName, l.x, l.y - 1);
        UpdateAgPercept(agName, l.x, l.y);
        UpdateAgPercept(agName, l.x, l.y + 1);
        UpdateAgPercept(agName, l.x + 1, l.y - 1);
        UpdateAgPercept(agName, l.x + 1, l.y);
        UpdateAgPercept(agName, l.x + 1, l.y + 1);
    }

    private void UpdateAgPercept(string agName, int x, int y) {
        // Error Control
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();
        if (world == null || !world.InGrid(x, y)) return;

        if (world.HasObject(GoldMinersWorld.OBSTACLE, x, y)) {
            JasonInstructions.AddPercept(agName, "cell(" + x + "," + y + ",obstacle)");
        }
        else {
            if (world.HasObject(GoldMinersWorld.GOLD, x, y)) {
                JasonInstructions.AddPercept(agName, "cell(" + x + "," + y + ",gold)");
            }
            if (world.HasObject(GoldMinersWorld.ENEMY, x, y)) {
                JasonInstructions.AddPercept(agName, "cell(" + x + "," + y + ",enemy)");
            }
            if (world.HasObject(GoldMinersWorld.AGENT, x, y)) {
                JasonInstructions.AddPercept(agName, "cell(" + x + "," + y + ",ally)");
            }
        }
    }

    private void EndSimulation() {
        JasonInstructions.AddPercept("end_of_simulation(" + 1 + ",0)");
        JasonInstructions.InformAgsEnvironmentChanged();
    }


    protected int GetAgIdBasedOnName(string agName) {
        return (Convert.ToInt32(agName.Substring(5))) - 1;
    }

}