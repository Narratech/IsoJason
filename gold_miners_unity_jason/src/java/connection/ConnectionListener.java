package connection;

import java.net.DatagramPacket;
import java.util.ArrayList;
import java.util.Arrays;

import org.json.JSONException;
import org.json.JSONObject;

import mining.ExternalEnvironment;

public class ConnectionListener extends Thread{
	
	private static ConnectionListener instance;
	private static volatile boolean running = false;
	
    public void run() {
    	running = true;
    	this.receive();
    }
    
    /*public static ConnectionListener getInstance() {
    	if(instance == null)
    		instance = new ConnectionListener();
    	return instance;
    }
    
    public void stopListening() {
    	running = false;
    }*/
    
	/**
	 * Muestra por consola el comando recibido
	 * Se lleva una lista de comandos conocidos por si "surge" alguno no controlado
	 * 
	 * @param direction
	 * @param data
	 * @throws JSONException 
	 */
	@SuppressWarnings("unused")
	private void show(String direction, String data) throws JSONException {
		if (!data.isEmpty()) {
			//System.out.println(data);
			ArrayList<String> security_names = new ArrayList<String>();
			security_names.addAll(Arrays.asList("move", "turn", "environment"));
			JSONObject json = new JSONObject(data);
			String name = json.getString("name");
			
			if (security_names.contains(name)) {
				if (!name.equalsIgnoreCase("registerCell"))
					System.out.println(direction + " " + data);
			} else 
				System.out.println("WARNING NEW COMMAND " + direction + " " + data);
		}
	}

    
	public void receive() {
	    try {
	    	while(running){		    	
	    		String receivedSentence = ExternalConnector.getInstance().receiveEnvironmentMsg();

		        ExternalEnvironment.getInstance().performReceivedMessage(receivedSentence);
	    	}
	      } catch (InterruptedException ie) {
	    	  Thread.currentThread().interrupt();
	      } catch (Exception e) {
	    	  System.out.println("Error Server: " + e.getMessage());
	      }
	  }


}
