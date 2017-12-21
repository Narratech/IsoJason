using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JasonInstructions {

    // ADD PERCEPTS
    public static void AddPercept(string percept) {
        ConnectionImp.GetInstance().SendToMAS(true, "(EnvironmentTag){\"name\":\"addpercept\",\"parameters\":{\"what\":\"" + percept + "\"}}");
    }
    public static void AddPercept(string agName, string percept) {
        ConnectionImp.GetInstance().SendToMAS(true, "(EnvironmentTag){\"name\":\"addpercept\",\"parameters\":{\"what\":\"" + percept + "\",\"who\":\"" + agName + "\"}}");
    }

    // REMOVE PERCEPTS
    public static void RemovePercept(string percept) {
        ConnectionImp.GetInstance().SendToMAS(true, "(EnvironmentTag){\"name\":\"removepercept\",\"parameters\":{\"what\":\"" + percept + "\"}}");
    }
    public static void RemovePercept(string agName, string percept) {
        ConnectionImp.GetInstance().SendToMAS(true, "(EnvironmentTag){\"name\":\"removepercept\",\"parameters\":{\"what\":\"" + percept + "\",\"who\":\"" + agName + "\"}}");
    }

    // CLEAR PERCEPTS
    public static void ClearPercepts() {
        ConnectionImp.GetInstance().SendToMAS(true, "(EnvironmentTag){\"name\":\"clearpercepts\",\"parameters\":{}}");
    }
    public static void ClearPercepts(string agName) {
        ConnectionImp.GetInstance().SendToMAS(true, "(EnvironmentTag){\"name\":\"clearpercepts\",\"parameters\":{\"who\":\"" + agName + "\"}}");
    }

    // INFORM AGENTS ENVIRONMENT CHANGED
    public static void InformAgsEnvironmentChanged() {
        ConnectionImp.GetInstance().SendToMAS(true, "(EnvironmentTag){\"name\":\"informchanges\",\"parameters\":{}}");
    }

}
