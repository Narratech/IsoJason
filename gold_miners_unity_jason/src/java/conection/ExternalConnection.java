package conection;

import java.util.concurrent.Semaphore;

public class ExternalConnection extends Thread{
	private static ExternalConnection instance;
	
	private static Semaphore environmentSem = new Semaphore(0);
	private static Semaphore mentalActionSem = new Semaphore(0);
	private static Semaphore receiveSem = new Semaphore(0);
	
	private final static String envRespTag = "(EnvironmentTag)";
	private final static String mentActRespTag = "(MentalActionTag)";
	private static String environmentMsg;
	private static String mentalActionMsg;
	
	private static boolean ready = false;
	
	public static ExternalConnection getInstance() throws InterruptedException{
		if (instance == null)
			instance = new ExternalConnection();
		return instance;
	}
	
	@Override
	public void run(){
		try {
			receiveResponse();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public boolean isReady(){
		return ready;
	}
	
	private void receiveResponse() throws InterruptedException{
		while (true){
			String response = Connection.getInstance().receive();
			if(response.contains(envRespTag)){
				environmentMsg = response.substring(envRespTag.length(), response.length());
				// Message ready to be returned
				environmentSem.release();
				// receiveSem awaits until msg gets returned for clearing EnvironmentMsg
				receiveSem.acquire();
				// Cleans the environmentMsg after it's read
				environmentMsg = "";
			}
			else if (response.contains(mentActRespTag)){
				mentalActionMsg = response.substring(mentActRespTag.length(), response.length());
				// Message ready to be returned
				mentalActionSem.release();
				// receiveSem awaits until msg gets returned for clearing mentalActionMsg
				receiveSem.acquire();
				// Cleans the mentalActionMsg after it's read
				mentalActionMsg = "";
			}
		}
	}
	
	public String receiveMsgEnvironment() throws InterruptedException{
		environmentSem.acquire();
		
		// Saving msg before we notify it can be cleared
		String msg = environmentMsg;
		receiveSem.release();
		
		return msg;
	}
	
	public String receiveMsgMentalAction() throws InterruptedException{
		mentalActionSem.acquire();
		
		// Saving msg before we notify it can be cleared
		String msg = mentalActionMsg;
		receiveSem.release();
		
		return msg;
	}
	
}
