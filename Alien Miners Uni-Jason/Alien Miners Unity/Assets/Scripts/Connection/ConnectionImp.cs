using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class ConnectionImp : Connection {


    ConnectionProperties cp;
    private byte[] data;

    public override void Initialize()
    {
        cp = new ConnectionProperties();
        data = new byte[1024];
    }

    public override void Send(bool send, object ev)
    {
        string info = "";
        if (ev.GetType() == typeof(System.String))
        {
            info = (string)ev;
        }
        /*else if (ev.GetType() == typeof(GameEvent))
        {
            //showGameEventStructure(ev);
            //A CAMBIAR------------\
            info = ((GameEvent)ev).toJSONObject().ToString();
            //---------------------/
        }*/

        if (send)
        {
            data = Encoding.ASCII.GetBytes(info);
            cp.getSocketClient().Send(data, data.Length);
        }
        else { Debug.Log(info); }
    }

    public override string Receive()
    {
        IPEndPoint sender = cp.getSender();


        string dataSocket = "";
        try
        {
            data = cp.getSocketServer().Receive(ref sender);
            dataSocket = Encoding.ASCII.GetString(data, 0, data.Length);
        }
        catch (System.Exception e) { /*Debug.Log(e.Message);*/ }

        //if (p.Name == "action") { Debug.Log(p.toJSONObject().ToString()); }
        return dataSocket;
    }

    /* --- HERRAMIENTAS --- */


    //---------------------- POSIBLEMENTE PRESCINDIBLE
    /*private void showGameEventStructure(GameEvent ge)
    {
        Debug.Log(ge.toJSONObject().ToString());
        string GameEvent = ge.name + "(";

        foreach (string p in ge.Params)
        {
            GameEvent += p + ", ";
        }
        GameEvent = GameEvent.Substring(0, GameEvent.Length - 2);
        GameEvent += ")";

        GameEvent += ")";
        Debug.Log(GameEvent);
    }
    //----------------------*/

}
