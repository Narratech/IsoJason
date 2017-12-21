using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMWorldUpdater : MonoBehaviour {

    static ILogHandler logHandler;
    Logger logger = new Logger(logHandler);

    private const string ENV_TAG = "(EnvironmentTag)";
    private const string MENTACT_TAG = "(MentalActionTag)";

    private void Awake() {
        Connection.GetInstance().Initialize();
    }

    // Use this for initialization
    void Start () {
        Init();
	}

    public void CheckEvents() {
        string msg = Connection.GetInstance().ReceiveFromMAS();

        if (msg != "") {
            JSONObject js = new JSONObject(msg);
            string name = js["name"].str;

            JSONObject param = js["parameters"];

            if (name == "do") {
                ActionRequest(param);
            }
            else if (name == "requestState") {
                MentalActionRequest(param);
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

        if (actionName != "[pick]" && actionName != "[drop]" && actionName != "[skip]")
        {
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
            SendToMAS(ENV_TAG, "{\"name\":\"actionResponse\",\"parameters\":{\"success\":\"yes\"}}");
        else
            SendToMAS(ENV_TAG, "{\"name\":\"actionResponse\",\"parameters\":{\"success\":\"no\"}}");
    }

    // MENTAL ACTION REQUEST (MENTAL-ACTION)
    private void MentalActionRequest(JSONObject param) {
        string worldState = "";
        bool success = ParseMentalAction(param, ref worldState);

        SendResultMentalAction(success, worldState);
    }

    private bool ParseMentalAction(JSONObject param, ref string worldState) {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();

        worldState = ToStringState(world, world.GetData());

        return true;
    }

    private string ToStringState(GoldMinersWorld world, int [,] state ) {
        int w = world.GetWidth();
        int h = world.GetHeight();
        String msg = "(" + w + ")(" + h + ")[";

        for(int i = 0; i < w; i++) {
            for(int j = 0; j < h; j++) {
                msg += state[i,j].ToString() + ",";
            }
        }

        // Removes the last " , "
        msg = msg.Substring(0, msg.Length-1) + "]";


        return msg ;
    }

    private void SendResultMentalAction(bool success, string worldState) {
        if (success)
            SendToMAS(MENTACT_TAG, "{\"name\":\"mentalActionResponse\",\"parameters\":{\"state\":\"" + worldState + "\",\"success\":\"yes\"}}");
        else
            SendToMAS(MENTACT_TAG, "{\"name\":\"mentalActionResponse\",\"parameters\":{\"state\":\"" + worldState + "\",\"success\":\"no\"}}");
    }

    // WRAPPER
    private void SendToMAS(string tag, string msg) {
        Connection.GetInstance().SendToMAS(true, tag + msg);
    }


    // INITIALIZATION OF MAS
    public void Init() {
        InitWorld();
        SendToMAS(ENV_TAG, "{\"name\":\"environmentdone\",\"parameters\":{}}");
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

    // Al fin llegamos a lo que significa actualizar la percepción de un 
    //agente (saber donde está, si lleva oro, que hay alrededor...)
    private void UpdateAgPercept(string agName, int ag) {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();

        //Por si acaso borrar las percepciones de este agente concreto
        JasonInstructions.ClearPercepts(agName);

        // El agente percibe donde está ÉL MISMO en el tablero :)
        GridWorld.Location l = world.GetAgPos(ag);
        JasonInstructions.AddPercept(agName, "pos(" + l.x + "," + l.y + ")");

        // El agente percibe si está portando oro, en caso de que lo porte
        //String carringGold
        if (world.IsCarryingGold(ag)) {
            JasonInstructions.AddPercept(agName, "carrying_gold");
        }

        // El agente percibe la información de lo que hay en su propia casilla
        // y en las 8 adyacentes en todas direcciones
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
        // Control de errores, que no haya modelo o que la casilla dicha 
        //no esté en el modelo
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

    public void EndSimulation() {
        JasonInstructions.AddPercept("end_of_simulation(" + 1 + ",0)");
        JasonInstructions.InformAgsEnvironmentChanged();
        // Al terminar la simulación adios modelo y adios vista (la escondo)
        //if (view != null) view.setVisible(false);
        //WorldModel.destroy();
    }


    protected int GetAgIdBasedOnName(string agName) {
        return (Convert.ToInt32(agName.Substring(5))) - 1;
    }

}