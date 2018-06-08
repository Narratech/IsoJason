package connection;

import java.net.DatagramSocket;
import java.net.SocketException;

public class ConnectionProperties {
	
	private String Address;
	private int exitPort;
	private int enterPort;
	private DatagramSocket serverSocket;
	
	public ConnectionProperties() {
		this.Address = "127.0.0.1";
		this.exitPort = 10000;
		this.enterPort = 10001;
		
    	try {
			this.serverSocket = new DatagramSocket(this.exitPort);
		} catch (SocketException e) {
			e.printStackTrace();
		}

	}

	public String getAddress() {
		return Address;
	}

	public int getExitPort() {
		return exitPort;
	}

	public int getEnterPort() {
		return enterPort;
	}

	public DatagramSocket getServerSocket() {
		return serverSocket;
	}	
	
	
}
