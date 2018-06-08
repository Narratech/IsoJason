package connection;

import java.util.concurrent.Semaphore;
import connection.Connection;
import mining.ExternalEnvironment;

public class NewExternalConnector extends Thread{
	private static NewExternalConnector instance;
	
	private final static String EnvironmentTAG = "(Environment)";
	private final static String InformationTAG = "(Information)";
	private final static String StopTAG = "(STOP)";
	private final static String IDExampleTAG = "(id: __________)";
	
	private volatile static Semaphore environmentSem = new Semaphore(0);
	private volatile static Semaphore informationSem = new Semaphore(0);
	private volatile static Semaphore checkedRecvdMsg = new Semaphore(1);
	
	private final Object lockSendInfo = new Object();
	private final Object lockSendEnv = new Object();
	private volatile static boolean hasID;
	private volatile static int msgID = 0;
	private volatile static String receivedEnvMsg = "";
	private volatile static String receivedInfoMsg = "";
	
	private volatile static boolean running = false;
	
	public static NewExternalConnector getInstance() throws InterruptedException{
		if (instance == null)
			instance = new NewExternalConnector();
		return instance;
	}
	
	@Override
	public void run(){
		running = true;
		try {
			receiveResponse();
			ExternalEnvironment.getInstance().stop();
		} catch (InterruptedException e) {
			Thread.currentThread().interrupt();
		}
	}
	
	
	/**
	 * Handles socket responses, distinguishing between "Environment" or "Information" messages
	 * @throws InterruptedException
	 */
	private void receiveResponse() throws InterruptedException{
		//synchronized (lockReceive) {
			while (running){
				String response = Connection.getInstance().receive();
				//System.out.println("FRESH MSG: "+ response);
				
				checkedRecvdMsg.acquire();
				
				if(response != "") {
					hasID = false;
					//System.out.println("HOLO");
					if(response.contains(EnvironmentTAG)){
						receivedEnvMsg = response;
						//System.out.println("HOLO ENV");
						if(receivedEnvMsg.contains("ID")) {
							//System.out.println("HOLO ENV ID");
							hasID = true;
							synchronized (lockSendEnv) {
								lockSendEnv.notifyAll();
							}
						}
						else
							environmentSem.release();
					}
					else if (response.contains(InformationTAG)){
						receivedInfoMsg = response;
						
						if(receivedInfoMsg.contains("ID")){
							hasID = true;
							synchronized (lockSendInfo) {
								lockSendInfo.notifyAll();
							}
						}
						else
							informationSem.release();
					}
					else if (response.contains(StopTAG)) {
						System.out.println("STOPPING CONNECTION");
						running = false;
					}
				}
			}
		//}
	}
	
	
	/**
	 * Awaits until an "Environment" message is received trough the socket
	 * @return <b>msg</b>: Environment Message
	 * @throws InterruptedException
	 */
	public String receiveEnvironmentMsg() throws InterruptedException{
		//System.out.println("SEMAFORO ENV: " + environmentSem.availablePermits());
		environmentSem.acquire();

		// Saving msg before we notify it can be cleared
		//System.out.println("MENSAJE: " + receivedEnvMsg);
		String msg = takeReceivedEnvMsg();
		//System.out.println("SEMAFORO BUCLE: " + checkedRecvdMsg.availablePermits());
		//System.out.println("MENSAJE: " + msg);
		
		return msg;
	}
	
	/**
	 * Awaits until an "Information" message is received trough the socket
	 * @return <b>msg</b>: Information Message
	 * @throws InterruptedException
	 */
	public String receiveInformationMsg() throws InterruptedException{
		informationSem.acquire();
		
		// Saving msg before we notify it can be cleared
		String msg = takeReceivedInfoMsg();
		//System.out.println(msg);
		
		return msg;
	}
	
	/**
	 * Adds the Environment header to the message, and sends it through Connection class
	 * @param body 
	 * @return
	 */
	public void sendEnvironmentMsg(String body){
		Connection.getInstance().send(EnvironmentTAG + body);
	}
	
	/**
	 * Adds the Information header to the message, and sends it through Connection class
	 * @param body 
	 * @return
	 */
	public void sendInformationMsg(String body){
		Connection.getInstance().send(InformationTAG + body);
	}
	
	
	/**
	 * Adds the Environment header, the ID of the requester, and sends it. Then, it awaits for response.
	 * @param body 
	 * @return Response message
	 * @throws InterruptedException 
	 */
	public String sendEnvironmentRequest(String body) throws InterruptedException{
		synchronized (lockSendEnv) {
			String id = generateId(), response = "";
			//System.out.println("SEND ENV REQ:" + id + body);
			Connection.getInstance().send(EnvironmentTAG + id + body);
			
			while(!receivedEnvMsg.contains(id)) {
				lockSendEnv.wait();
			}
			
			//System.out.println("MENSAJE REPLY: "+receivedEnvMsg);
			response = takeReceivedEnvMsg();
			
			
			return response;
		}
	}
	
	/**
	 * Adds the Information Tag (a header) to the message, the ID of the requester, and sends it. Then, it awaits for response.
	 * @param body 
	 * @return Response message
	 * @throws InterruptedException 
	 */
	public String sendInformationRequest(String body) throws InterruptedException{
		synchronized (lockSendInfo) {
			String id = generateId(), response = "";
			//System.out.println("SEND INFO REQ:"+id+body);
			Connection.getInstance().send(InformationTAG + id +  body);
			
			while(!receivedInfoMsg.contains(id)) {
				lockSendInfo.wait();
			}
			//System.out.println("UNLOCKED");
			response = takeReceivedInfoMsg();
			//System.out.println(response);
			
			return response;
		}
	}
	
	private String generateId() {
		String formatedID = formatID();
		String idStr = "(ID: " + formatedID + ")";
		increaseMsgID();
		
		return idStr;
	}
	
	private String formatID() {
		int idLength = String.valueOf(msgID).length();
		int maxLength = String.valueOf(Integer.MAX_VALUE).length();
		String formatedID = "";
		
		for(int i = idLength; i < maxLength; i++) { 
			formatedID += "_";
		}
		
		formatedID += String.valueOf(msgID);
		return formatedID;
	}
	
	private void increaseMsgID(){
		msgID = (msgID + 1) % Integer.MAX_VALUE;
	}
	
	private String takeReceivedEnvMsg() {
		String msg = getReceivedEnvMsgContent();
		clearReceivedEnvMsgBuffer();
		checkedRecvdMsg.release();
		
		return msg;
	}
	
	private String getReceivedEnvMsgContent() {
		//System.out.println("SUBSTRING");
		if(hasID)
			return receivedEnvMsg.substring(EnvironmentTAG.length() + IDExampleTAG.length());
		else
			return receivedEnvMsg.substring(EnvironmentTAG.length());
	}
	
	private void clearReceivedEnvMsgBuffer() {
		receivedEnvMsg = "";
	}
	
	private String takeReceivedInfoMsg() {
		String msg = getReceivedInfoMsgContent();
		clearReceivedInfoMsgBuffer();
		checkedRecvdMsg.release();
		
		return msg;
	}

	private String getReceivedInfoMsgContent() {
		if(hasID)
			return receivedInfoMsg.substring(InformationTAG.length() + IDExampleTAG.length());
		else
			return receivedInfoMsg.substring(InformationTAG.length());
	}
	
	private void clearReceivedInfoMsgBuffer() {
		receivedInfoMsg = "";
	}
}
