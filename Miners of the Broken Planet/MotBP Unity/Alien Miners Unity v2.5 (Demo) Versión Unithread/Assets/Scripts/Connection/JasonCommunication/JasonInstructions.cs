using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JasonInstructions {

    // ADD PERCEPTS
    public static void AddPercept(string percept) {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"addpercept\",\"parameters\":{\"what\":\"" + percept + "\"}}");
    }
    public static void AddPercept(string agName, string percept) {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"addpercept\",\"parameters\":{\"what\":\"" + percept + "\",\"who\":\"" + agName + "\"}}");
    }

    // REMOVE PERCEPTS
    public static void RemovePercept(string percept) {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"removepercept\",\"parameters\":{\"what\":\"" + percept + "\"}}");
    }
    public static void RemovePercept(string agName, string percept) {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"removepercept\",\"parameters\":{\"what\":\"" + percept + "\",\"who\":\"" + agName + "\"}}");
    }

    // REMOVE PERCEPTS BY UNIFICATION
    public static void RemovePerceptsByUnif(string percept)
    {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"removeperceptsbyunif\",\"parameters\":{\"what\":\"" + percept + "\"}}");
    }
    public static void RemovePerceptsByUnif(string agName, string percept)
    {
        Debug.Log(percept);
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"removeperceptsbyunif\",\"parameters\":{\"what\":\"" + percept + "\",\"who\":\"" + agName + "\"}}");
    }

    // CLEAR PERCEPTS
    public static void ClearPercepts() {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"clearpercepts\",\"parameters\":{}}");
    }
    public static void ClearPercepts(string agName) {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"clearpercepts\",\"parameters\":{\"who\":\"" + agName + "\"}}");
    }

    // INFORM AGENTS ENVIRONMENT CHANGED
    public static void InformAgsEnvironmentChanged() {
        ExternalConnector.GetInstance().SendEnvironmentMsgToMAS("{\"name\":\"informchanges\",\"parameters\":{}}");
    }

}
