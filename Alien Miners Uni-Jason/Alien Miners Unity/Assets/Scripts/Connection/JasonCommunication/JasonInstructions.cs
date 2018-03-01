using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JasonInstructions {

    // ADD PERCEPTS
    public static void AddPercept(string percept) {
		ExternalConnector.GetInstance ().sendEnvironmentToMAS ("{\"name\":\"addpercept\",\"parameters\":{\"what\":\"" + percept + "\"}}");
    }
    public static void AddPercept(string agName, string percept) {
		ExternalConnector.GetInstance ().sendEnvironmentToMAS ("{\"name\":\"addpercept\",\"parameters\":{\"what\":\"" + percept + "\",\"who\":\"" + agName + "\"}}");
    }

    // REMOVE PERCEPTS
    public static void RemovePercept(string percept) {
		ExternalConnector.GetInstance ().sendEnvironmentToMAS ("{\"name\":\"removepercept\",\"parameters\":{\"what\":\"" + percept + "\"}}");
    }
    public static void RemovePercept(string agName, string percept) {
		ExternalConnector.GetInstance ().sendEnvironmentToMAS ("{\"name\":\"removepercept\",\"parameters\":{\"what\":\"" + percept + "\",\"who\":\"" + agName + "\"}}");
    }

    // CLEAR PERCEPTS
    public static void ClearPercepts() {
		ExternalConnector.GetInstance ().sendEnvironmentToMAS ("{\"name\":\"clearpercepts\",\"parameters\":{}}");
    }
    public static void ClearPercepts(string agName) {
		ExternalConnector.GetInstance ().sendEnvironmentToMAS ("{\"name\":\"clearpercepts\",\"parameters\":{\"who\":\"" + agName + "\"}}");
    }

    // INFORM AGENTS ENVIRONMENT CHANGED
    public static void InformAgsEnvironmentChanged() {
		ExternalConnector.GetInstance ().sendEnvironmentToMAS ("{\"name\":\"informchanges\",\"parameters\":{}}");
    }

}
