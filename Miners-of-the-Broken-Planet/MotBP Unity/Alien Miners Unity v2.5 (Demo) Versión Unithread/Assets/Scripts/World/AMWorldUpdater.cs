using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AMWorldUpdater : GMWorldUpdater {

    public MistAndEnvLayersManager MELCRef;

    static ILogHandler logHandler;
    Logger logger = new Logger(logHandler);


    // Use this for initialization
    new void Start()
    {
        Init();
    }

    // Executed when game stops
    new void OnApplicationQuit()
    {
        EndSimulation();
        ExternalConnector.GetInstance().SendStopToMAS();
    }

    public new void CheckEvents()
    {
        string[] strArray = ExternalConnector.GetInstance().ReceiveFromMAS();
        string tag = strArray[0];
        string msg = strArray[1];
        string id = strArray[2];

        if (tag != "" && msg != "")
        {
            JSONObject js = new JSONObject(msg);
            string name = js["name"].str;

            JSONObject param = js["parameters"];

            if (tag == "Environment")
            {
                if (name == "do" && id != "")
                    ActionRequest(param, id);
                else
                    SendSuccededAction(false, id);
            }
            else if (tag == "Information")
            {
                if (name == "stateRequest")
                    MentalActionRequest(param, id);
                else if (name == "msgReceived")
                    ChangeDisplayedEnvironmentInfo(param);
            }
        }
    }

    // ACTION REQUEST (ENVIRONMENT)
    protected void ActionRequest(JSONObject param, string idRequester)
    {
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();

        bool success = ParseAction(param);

        if (success)
        {
            string agName = param["who"].str;
            if (agName.Contains("alien"))
                UpdateEnemyPercept(world.GetEnIdBasedOnName(agName));
            else
                UpdateAgPercept(GetAgIdBasedOnName(agName));
        }

        SendSuccededAction(success, idRequester);
    }

    protected new bool ParseAction(JSONObject param)
    {
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();

        string direction = null;
        string strParams = param["otherParameters"].str;
        strParams = strParams.Substring(1, strParams.Length - 2);
        strParams = strParams.Replace(" ", "");

        String[] parameters = strParams.Split(',');
        string actionName = parameters[0];

        string agentName = param["who"].str;
        int ag = GetAgIdBasedOnName(agentName);
        // Vars for "kill" action parsing
        string targetName;
        int targetID = -1;
        // Who requests to kill
        int killerObj = 0;

        if (actionName == "right" || actionName == "left" || actionName == "down" || actionName == "up")
        {
            direction = actionName.ToUpper();
            actionName = "move";
        }
        else if (actionName == "kill")
        {
            int x, y;
            try
            {
                x = Int32.Parse(parameters[2]);
                y = Int32.Parse(parameters[3]);

                if (parameters[1].Equals("alien"))
                {
                    killerObj = AlephMinersWorld.AGENT;
                    targetName = world.GetEnemyNameBasedOnPos(x, y);
                    targetID = world.GetEnIdBasedOnName(targetName);

                }
                else if (parameters[1].Equals("ally"))
                {
                    killerObj = AlephMinersWorld.ENEMY;
                    targetName = world.GetAgentNameBasedOnPos(x, y);
                    targetID = GetAgIdBasedOnName(targetName);
                }
            }
            catch
            {
                return false;
            }
        }

        // Action Execution
        try
        {
            switch (actionName)
            {
                case "move":
                    AlephMinersWorld.Direction dir = (AlephMinersWorld.Direction)Enum.Parse(typeof(AlephMinersWorld.Direction), direction);
                    if (agentName.Contains("alien"))
                        return world.MoveEnemy(dir, ag);
                    else
                        return world.Move(dir, ag);

                case "pick":
                    return world.Pick(ag);

                case "drop":
                    return world.Drop(ag);

                case "kill":
                    return world.Kill(killerObj, ag, targetID);
                case "steal":
                    return world.Steal(ag);
                case "skip":
                    return true;
                default:
                    return false;
            }
        }
        catch
        {
            return false;
        }
    }

    protected void SendSuccededAction(bool success, string id)
    {
        if (success)
            SendEnvironmentChanges("{\"name\":\"actionResponse\",\"parameters\":{\"success\":\"yes\"}}", id);
        else
            SendEnvironmentChanges("{\"name\":\"actionResponse\",\"parameters\":{\"success\":\"no\"}}", id);
    }


    // MENTAL ACTION REQUEST (INFORMATION)
    protected void MentalActionRequest(JSONObject param, string idRequester)
    {
        string worldState = "";
        bool success = ParseMentalAction(param, ref worldState);

        SendResultMentalAction(success, worldState, idRequester);
    }

    protected new bool ParseMentalAction(JSONObject param, ref string worldState)
    {
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();

        worldState = ToStringState(world, world.GetData());

        return true;
    }

    protected string ToStringState(AlephMinersWorld world, int[,] state)
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

    protected void SendResultMentalAction(bool success, string worldState, string id)
    {
        if (success)
            SendInformationResults("{\"name\":\"mentalActionResponse\",\"parameters\":{\"state\":\"" + worldState + "\",\"success\":\"yes\"}}", id);
        else
            SendInformationResults("{\"name\":\"mentalActionResponse\",\"parameters\":{\"state\":\"" + worldState + "\",\"success\":\"no\"}}", id);
    }

    // CHANGE DISPLAYED ENVIRONMENT INFO (INFORMATION)
    protected void ChangeDisplayedEnvironmentInfo(JSONObject param)
    {
        string str = param["message"].str;
        PerformChangesOnEnvironment(str);
    }

    protected void PerformChangesOnEnvironment(string str)
    {
        try
        {
            int strIniLen = "msg_sent(minerX,".Length;
            string content = str.Substring(strIniLen, str.Length - (1 + strIniLen));
            if (content.Contains("goldInQuadrant"))
            {
                if (!content.Contains("clear"))
                {
                    int contentIniLen = "goldInQuadrant(".Length;
                    int q = Int32.Parse(content.Substring(contentIniLen, content.Length - (1 + contentIniLen)));
                    MELCRef.SetGoldMaterial(q);
                }
                else
                {
                    int contentIniLen = "clear(goldInQuadrant(".Length;
                    int q = Int32.Parse(content.Substring(contentIniLen, content.Length - (2 + contentIniLen)));
                    MELCRef.SetDefaultMaterial(q);
                }

            }
            else if (content.Contains("alienInQuadrant"))
            {
                if (!content.Contains("killed"))
                {
                    int contentIniLen = "alienInQuadrant(".Length;
                    int q = Int32.Parse(content.Substring(contentIniLen, content.Length - (1 + contentIniLen)));
                    MELCRef.SetDangerMaterial(q);
                }
                else
                {
                    int contentIniLen = "killed(alienInQuadrant(".Length;
                    int q = Int32.Parse(content.Substring(contentIniLen, content.Length - (2 + contentIniLen)));
                    MELCRef.SetDefaultMaterial(q);
                }

            }
            else if (content.Contains("gold") && !content.Contains("committed"))
            {
                if (!content.Contains("picked"))
                {
                    int contentIniLen = "gold(".Length;
                    string xy = content.Substring(contentIniLen, content.Length - (1 + contentIniLen));
                    string[] splittedXY = xy.Split(',');
                    int x = Int32.Parse(splittedXY[0]);
                    int y = Int32.Parse(splittedXY[1]);
                    MELCRef.SetTileGoldMaterial(x, y);
                }
                else
                {
                    int contentIniLen = "picked(gold(".Length;
                    string xy = content.Substring(contentIniLen, content.Length - (2 + contentIniLen));
                    string[] splittedXY = xy.Split(',');
                    int x = Int32.Parse(splittedXY[0]);
                    int y = Int32.Parse(splittedXY[1]);
                    MELCRef.RestoreTileMaterial(x, y);
                }
            }
            else
            {

                strIniLen = "msg_sent(".Length;
                str = str.Substring(strIniLen);
                string[] strArray = new string[2];
                strArray[0] = str.Substring(0, "minerX".Length);
                strArray[1] = str.Substring("minerX,".Length);
                strArray[1] = strArray[1].Substring(0, strArray[1].Length - 1);

                // "[MinerX]: Whatever is said in the message"
                string textStr = "[" + strArray[0] + "]: " + strArray[1];

                textLeaderReceivedMsgs.GetComponent<TextLogControl>().LogText(textStr, Color.white);
            }

        }
        catch { }
    }


    // WRAPPER
    protected void SendEnvironmentChanges(string msg, string id)
    {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS(msg, id);
    }
    protected void SendInformationResults(string msg, string id)
    {
        ExternalConnector.GetInstance().SendInformationMsgToMAS(msg, id);
    }


    // INITIALIZATION OF MAS
    public new void Init()
    {
        //Thread.Sleep(1000);
        InitWorld();
        SendEnvironmentChanges("{\"name\":\"environmentdone\",\"parameters\":{}}");
    }

    protected new void InitWorld()
    {
        try
        {
            AlephMinersWorld world = GetComponent<AlephMinersWorld>();
            JasonInstructions.ClearPercepts();
            Thread.Sleep(500);
            JasonInstructions.AddPercept("gsize(" + world.simID + "," + world.GetWidth() + "," + world.GetHeight() + ")");
            JasonInstructions.AddPercept("depot(" + world.simID + "," + world.GetDepot().x + "," + world.GetDepot().y + ")");
            JasonInstructions.AddPercept("num_quads(" + "9" + ")");

            UpdateAgsPercept();

            JasonInstructions.InformAgsEnvironmentChanged();
        }
        catch (System.Exception e)
        {
            logger.LogWarning("InitWorld", "Error creating world " + e);
        }
    }


    protected new void UpdateAgsPercept()
    {
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();
        for (int i = 0; i < world.GetNbOfAgs(); i++)
        {
            UpdateAgPercept(i);
        }
        for (int i = 0; i < world.GetNbOfAliens(); i++)
        {
            UpdateEnemyPercept(i);
        }
    }

    protected new void UpdateAgPercept(int ag)
    {
        UpdateAgPercept("miner" + (ag + 1), ag);
    }

    protected void UpdateEnemyPercept(int ag)
    {
        UpdateEnemyPercept("alien" + (ag + 1), ag);
    }

    protected new void UpdateAgPercept(string agName, int ag)
    {
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();

        // Agent perceives where it is
        GridWorld.Location l = world.GetAgPos(ag);
        if (!ReferenceEquals(l, null))
        {
            // Deletes what an agents knows about its perceptions (what it see)
            JasonInstructions.ClearPercepts(agName);
            // Agent perceives where it is
            JasonInstructions.AddPercept(agName, "pos(" + l.x + "," + l.y + ")");
            // Agent perceives which class it is
            JasonInstructions.AddPercept(agName, "class(" + world.GetAgClass(ag) + ")");
            // Agent perceives which personality it has
            JasonInstructions.AddPercept(agName, "personality(" + world.GetAgPersonality(ag) + ")");

            // If it's carring gold, it also perceives that
            if (world.IsCarryingGold(ag))
            {
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
            if (world.GetAgClass(ag).Equals("soldier") || world.GetAgClass(ag).Equals("explorer"))
                UpdateAgIncreasedPercepts(agName, l.x, l.y);
        }
        else
            JasonInstructions.AddPercept(agName, "dead");
    }

    protected void UpdateAgIncreasedPercepts(string agName, int x, int y)
    {
        UpdateAgPercept(agName, x - 2, y - 2);
        UpdateAgPercept(agName, x - 2, y - 1);
        UpdateAgPercept(agName, x - 2, y);
        UpdateAgPercept(agName, x - 2, y + 1);
        UpdateAgPercept(agName, x - 2, y + 2);

        UpdateAgPercept(agName, x - 1, y - 2);
        UpdateAgPercept(agName, x - 1, y + 2);

        UpdateAgPercept(agName, x, y - 2);
        UpdateAgPercept(agName, x, y + 2);

        UpdateAgPercept(agName, x + 1, y - 2);
        UpdateAgPercept(agName, x + 1, y + 2);

        UpdateAgPercept(agName, x + 2, y - 2);
        UpdateAgPercept(agName, x + 2, y - 1);
        UpdateAgPercept(agName, x + 2, y);
        UpdateAgPercept(agName, x + 2, y + 1);
        UpdateAgPercept(agName, x + 2, y + 2);
    }

    protected void UpdateEnemyPercept(string agName, int ag)
    {
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();

        GridWorld.Location l = world.GetAlPos(ag);
        if (!ReferenceEquals(l, null))
        {
            // Deletes what an agents knows about its perceptions (what it see)
            JasonInstructions.ClearPercepts(agName);

            // Agent perceives where it is
            JasonInstructions.AddPercept(agName, "pos(" + l.x + "," + l.y + ")");
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
        else
            JasonInstructions.AddPercept(agName, "dead");
    }

    protected new void UpdateAgPercept(string agName, int x, int y)
    {
        // Error Control
        AlephMinersWorld world = GetComponent<AlephMinersWorld>();
        if (world == null || !world.InGrid(x, y)) return;

        if (world.HasObject(AlephMinersWorld.OBSTACLE, x, y))
        {
            JasonInstructions.AddPercept(agName, "cell(" + x + "," + y + ",obstacle)");
        }
        else
        {
            if (world.HasObject(AlephMinersWorld.GOLD, x, y))
                JasonInstructions.AddPercept(agName, "cell(" + x + "," + y + ",gold)");

            if (world.HasObject(AlephMinersWorld.ENEMY, x, y))
                JasonInstructions.AddPercept(agName, "cell(" + x + "," + y + ",alien)");

            if (world.HasObject(AlephMinersWorld.AGENT, x, y))
                JasonInstructions.AddPercept(agName, "cell(" + x + "," + y + ",ally)");
        }
    }

    protected new void EndSimulation()
    {
        JasonInstructions.AddPercept("end_of_simulation(" + 1 + ",0)");
        JasonInstructions.InformAgsEnvironmentChanged();
    }



}
