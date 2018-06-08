using System.Net;
using System.Net.Sockets;

public class ConnectionProperties {

    private UdpClient socketClient;
    private UdpClient socketServer;
    private IPEndPoint sender;

    public ConnectionProperties() {
        string IP = "127.0.0.1";
        int exitPort = 10000;
        int enterPort = 10001;
        int ttl = 10;

        socketClient = new UdpClient(IP, exitPort);
        socketServer = new UdpClient(new IPEndPoint(IPAddress.Any, enterPort));
        socketServer.Client.ReceiveTimeout = ttl;
        sender = new IPEndPoint(IPAddress.Any, exitPort);
    }

    public UdpClient GetClientSocket() {
        return socketClient;
    }

    public UdpClient GetServerSocket() {
        return socketServer;
    }

    public IPEndPoint GetSender() {
        return sender;
    }

}
