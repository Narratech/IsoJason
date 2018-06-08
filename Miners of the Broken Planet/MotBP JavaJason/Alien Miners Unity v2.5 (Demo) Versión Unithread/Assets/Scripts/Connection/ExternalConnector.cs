using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExternalConnector {
	private const string EnvironmentTAG = "(Environment)";
	private const string InformationTAG = "(Information)";
    private const string ID = "ID";
    private const string IDTAGDefaultString = "(id: )";
    private const string StopTAG = "(STOP)";

	private static ExternalConnector instance;

	public static ExternalConnector GetInstance() {
        if (instance == null)
        {
            instance = new ExternalConnector();
        }
		return instance;
	}

    //Wrapper
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SendEnvironmentMsgToMAS(string msg)
    {
        SendEnvironmentMsgToMAS(msg, "");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SendEnvironmentMsgToMAS(string msg, string id){
        if(id == "")
		    Connection.GetInstance().Send(true, EnvironmentTAG + msg);
        else
            Connection.GetInstance().Send(true, EnvironmentTAG + "(" + ID + ": " + id + ")" + msg);
    }

    // Wrapper
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SendInformationMsgToMAS(string msg)
    {
        SendInformationMsgToMAS(msg, "");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SendInformationMsgToMAS(string msg, string id){
        if (id == "")
            Connection.GetInstance().Send(true, InformationTAG + msg);
        else
            Connection.GetInstance().Send(true, InformationTAG + "(" + ID + ": " + id + ")" + msg);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SendStopToMAS(){
		Connection.GetInstance().Send(true, StopTAG);
        Connection.GetInstance().CloseConnection();
	}

    [MethodImpl(MethodImplOptions.Synchronized)]
    public string[] ReceiveFromMAS(){
		string data = Connection.GetInstance().Receive(); 
		string[] msg = new string[3];
        int idLabbelLength = Int32.MaxValue.ToString().Length + IDTAGDefaultString.Length;
        msg[0] = ""; 
		msg[1] = "";
        msg[2] = "";

        if (data.Contains(EnvironmentTAG)){
			msg[0] = EnvironmentTAG.Substring (1, EnvironmentTAG.Length - 2);

            if (data.Contains(ID))
            {
                msg[1] = data.Substring(EnvironmentTAG.Length + idLabbelLength);
                msg[2] = data.Substring(EnvironmentTAG.Length + 5, idLabbelLength - IDTAGDefaultString.Length);
            }
            else
                msg[1] = data.Substring(EnvironmentTAG.Length);
        }
		else if (data.Contains(InformationTAG)){
			msg[0] = InformationTAG.Substring (1, InformationTAG.Length - 2);

            if (data.Contains(ID))
            {
                msg[1] = data.Substring(InformationTAG.Length + idLabbelLength);
                msg[2] = data.Substring(InformationTAG.Length + 5, idLabbelLength - IDTAGDefaultString.Length);
            }
            else
                msg[1] = data.Substring(InformationTAG.Length);
        }

		return msg;
	}

}
