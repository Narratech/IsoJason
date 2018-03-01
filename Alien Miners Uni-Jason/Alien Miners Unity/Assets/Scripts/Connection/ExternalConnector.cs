using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalConnector {
	private const string EnvironmentTAG = "(Environment)";
	private const string InformationTAG = "(Information)";
	private const string StopTAG = "(STOP)";

	private static ExternalConnector instance;

	public static ExternalConnector GetInstance() {
		if (instance == null)
			instance = new ExternalConnector();
		return instance;
	}

	public void sendEnvironmentToMAS(string msg){
		Connection.GetInstance().Send(true, EnvironmentTAG + msg);
	}

	public void sendInformationToMAS(string msg){
		Connection.GetInstance().Send(true, InformationTAG + msg);
	}

	public void sendStopToMAS(){
		Connection.GetInstance ().Send (true, StopTAG);
	}

	public string[] receiveFromMAS(){
		string data = Connection.GetInstance().Receive(); 
		string[] msg = new string[2]; 
		msg [0] = ""; 
		msg [1] = "";

		if (data.Contains(EnvironmentTAG)){
			msg [0] = EnvironmentTAG.Substring (1, EnvironmentTAG.Length - 2);
			msg [1] = data.Substring(EnvironmentTAG.Length);
		}
		else if (data.Contains(InformationTAG)){
			msg [0] = InformationTAG.Substring (1, InformationTAG.Length - 2);
			msg [1] = data.Substring(InformationTAG.Length);
		}

		return msg;
	}
}
