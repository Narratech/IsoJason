package connection;

import java.util.concurrent.Semaphore;

import mining.ExternalEnvironment;

public class ExternalConnector extends Thread{
	private static ExternalConnector instance;
	
	private static Semaphore environmentSem = new Semaphore(0);
	private static Semaphore informationSem = new Semaphore(0);
	private static Semaphore receiveSem = new Semaphore(0);
	
	private final static String EnvironmentTAG = "(Environment)";
	private final static String InformationTAG = "(Information)";
	private final static String ResponseTAG = "(Response)";
	private final static String StopTAG = "(STOP)";
	
	private static String environmentMsg;
	private static String informationMsg;
	
	private static boolean ready = false;
	private static boolean running = false;
	
	public static ExternalConnector getInstance() throws InterruptedException{
		if (instance == null)
			instance = new ExternalConnector();
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
	
	public boolean isReady(){
		return ready;
	}
	/**
	 * Handles socket responses, distinguishing between "Environment" or "Information" messages
	 * @throws InterruptedException
	 */
	private void receiveResponse() throws InterruptedException{
		while (running){
			String response = Connection.getInstance().receive();

			if(response.contains(EnvironmentTAG)){
				environmentMsg = response.substring(EnvironmentTAG.length(), response.length());
				// Message ready to be returned
				environmentSem.release();
				// receiveSem awaits until msg gets returned for clearing EnvironmentMsg
				receiveSem.acquire();
				// Cleans the environmentMsg after it's read
				environmentMsg = "";
			}
			else if (response.contains(InformationTAG)){
				informationMsg = response.substring(InformationTAG.length(), response.length());
				// Message ready to be returned
				informationSem.release();
				// receiveSem awaits until msg gets returned for clearing mentalActionMsg
				receiveSem.acquire();
				// Cleans the mentalActionMsg after it's read
				informationMsg = "";
			}
			else if (response.contains(StopTAG)) {
				System.out.println(response.substring(0,StopTAG.length()));
				running = false;
			}
		}
	}
	
	/**
	 * Awaits until an "Environment" message is received trough the socket
	 * @return <b>msg</b>: Environment Message
	 * @throws InterruptedException
	 */
	public String receiveEnvironmentMsg() throws InterruptedException{
		environmentSem.acquire();
		
		// Saving msg before we notify it can be cleared
		String msg = environmentMsg;
		receiveSem.release();

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
		String msg = informationMsg;
		receiveSem.release();

		return msg;
	}
	
	/**
	 * Adds the Environment Tag (a header) to the message, and sends it through Connection class
	 * @param body 
	 * @return
	 */
	public void sendEnvironmentMsg(String body){
		Connection.getInstance().send(EnvironmentTAG + body);
	}
	
	/**
	 * Adds the Information Tag (a header) to the message, and sends it through Connection class
	 * @param body 
	 * @return
	 */
	public void sendInformationMsg(String body){
		Connection.getInstance().send(InformationTAG + body);
	}
}
