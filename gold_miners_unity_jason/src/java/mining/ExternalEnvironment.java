package mining;

import jason.asSyntax.Literal;
import jason.asSyntax.Structure;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.concurrent.Semaphore;
import java.util.logging.Level;
import java.util.logging.Logger;

import conection.Connection;
import conection.ConnectionListener;
import conection.ExternalConnection;

/* Importante que el entorno que hagamos herede de Environment, la clase de
Jason pensada para estos menesteres */
public class ExternalEnvironment extends jason.environment.Environment {

	// Logger que informa por consola de lo que sea
    protected Logger logger = Logger.getLogger("jasonTeamSimLocal.mas2j." + ExternalEnvironment.class.getName());
    
	// El numero que indica qué escenario estamos usando
    protected int     simId    = 3; 
    protected int     nbWorlds = 3; // Este debe ser el máximo de escenarios que tenemos

	//Tiempo que se para la simulación entre movimientos, para verla más o menos rápido
    protected int     sleep    = 0; 
	
	// Para indicar si la simulación está en marcha o no
    protected boolean running  = true;
    
    //Variables que intervienen en la recepción de respuestas a ExecuteAction
    public boolean awaitedResponse = false;
    public boolean succesfulExecutedAction = false;
	private Semaphore actionResponseSem = new Semaphore(0);
    
	// Los segundos que durará la simulación
    public static final int SIM_TIME = 60;  

    
	private static ExternalEnvironment instance;
	//Thread (ConnectionListener)
	private Thread threadCL;
	public Thread threadEC;
	public boolean loadedEnvironment;
	
	public ExternalEnvironment(){
		instance = this;
		this.threadCL = null;
		this.threadEC = null;
		this.loadedEnvironment = false;
	}
	
	public static ExternalEnvironment getInstance() {
    	if (instance == null) 
    		instance = new ExternalEnvironment();
    	
		return instance;
	}
	
    public int getSimId() {
        return simId;
    }
    
    public void setSleep(int s) {
        sleep = s;
    }
	
	
    
    
	//--------INIT
	/**
	 * Método de inicialización en el que se espera que se introduzcan nuevas percepciones en el sistema multiagente (Jason)
	 */
    @Override
    public void init(String[] args) {
        sleep  = Integer.parseInt(args[0]);
		//Crea un ConnectionListener en thread para escuchar todas las acciones recibidas desde Unity
        
        this.threadSocketConnection(true);
        
		try {
			this.threadExternalConnection(true);
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
		
		System.out.println("Esperando a Unity...");
		while(!loadedEnvironment){
			try {
				Thread.sleep(1);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
		
		System.out.println("Jason BDI ready...");
    }
    
    /**
     * Método que crea un ConnectionListener como thread. El hilo principal queda a la espera de que el entorno se cargue, mientras que
     * el thread (ConnectionListener) queda a la espera de mensajes desde Unity.
     * @param run
     */
	private void threadSocketConnection(boolean run) {
		if (run) {
			this.threadCL = new Thread(new ConnectionListener(), "connectionListener" );
			//Empezará a escuchar la conexión
			this.threadCL.start();
		}
	}
	
	/**
	 * Runs a thread that awaits from model responses
	 * @param run
	 * @throws InterruptedException
	 */
	private void threadExternalConnection(boolean run) throws InterruptedException {
		if (run){
			this.threadEC = new Thread(ExternalConnection.getInstance(), "externalConnection" );
			//Empezará a escuchar la conexión
			this.threadEC.start();
		}
	}
	
	/**
	 * Enumerado que distingue los tipos de cambios que solicita Unity a través del socket.
	 * @author Sergio González Jiménez
	 *
	 */
	private enum ReceivableMessages {
		ADDPERCEPT, REMOVEPERCEPT, CLEARPERCEPTS, INFORMCHANGES, //Changes on MAS
		ENVIRONMENTDONE, ACTIONRESPONSE //Communication with the model
	}
	
	/**
	 * Interpreta y realiza el mensaje pasado por parametro, <b>data</b>.
	 * <li>(FROM Outside TO Jason)</li>
	 * @param data
	 * @return Devuelve el mismo string que recibe por entrada, data
	 * @throws JSONException
	 */
	public String performReceivedMessage(String data) throws JSONException{
		String result = data;
		JSONObject json = new JSONObject(data);
		String name = json.getString("name");
		ReceivableMessages msg = ReceivableMessages.valueOf(name.toUpperCase());
		
		JSONObject parameters = null;
		if(json.has("parameters")){
			parameters = json.getJSONObject("parameters");
		}
		
		String who = null;
		if (parameters.has("who")){
			who = parameters.getString("who");
		}
		
		Literal what = null;
		if (parameters.has("what")){
			what = Literal.parseLiteral(parameters.getString("what"));
		}
		
		String success = null;
		if (parameters.has("success")){
			success = parameters.getString("success");
		}

		switch(msg) {
			case ADDPERCEPT:
				if (who == null)
					addPercept(what);
				else
					addPercept(who, what);
				break;
			case REMOVEPERCEPT:
				if (who == null)
					removePercept(what);
				else
					removePercept(who, what);
				break;
			case CLEARPERCEPTS:
				if (who == null)
					clearPercepts();
				else
					clearPercepts(who);
				break;
				
			case INFORMCHANGES:
				informAgsEnvironmentChanged();
				break;
			case ENVIRONMENTDONE:
				loadedEnvironment = true;
				break;
			case ACTIONRESPONSE:
				if (awaitedResponse){
					actionResponseSem.release();
					awaitedResponse = false;
					if (success.contains("yes")){
						succesfulExecutedAction = true;
					}
				}
				break;
			default:
				break;
		}
		return result;
	}
	
	
	
    //--------EXECUTEACTION
    /**
     * Transforma una acción de AgentSpeak en una acción real sobre el modelo
     * Debe ser sobreescrita en una clase que extienda de UnityEnvironment
     * @param ag : Nombre del agente
	 * @param action : Acción a realizar
     */
    @Override
    public boolean executeAction(String ag, Structure action) {
    	try {
			if (sleep > 0) {
			    Thread.sleep(sleep); // Ralentiza acciones para poder "verlas"
			}
			String actionName = action.getFunctor();
			String parameters = action.getTerms().toString();
			sendActionToModel(ag, actionName, parameters);
			//Connection.getInstance().send("name:" + act +"|" + parameters);
			
			actionResponseSem.acquire();
			
			if (succesfulExecutedAction) {
				succesfulExecutedAction = false;
			    return true;
			}
        } catch (InterruptedException e) {
        } catch (Exception e) {
			logger.log(Level.SEVERE, "error executing " + action + " for " + ag, e);
        }
		
        return false; 
    }
    
	/**
	 * Sends an action to the model to perform it.
	 * @param ag : Who wants to execute the action
	 * @param actionName
	 * @param parameters : Needed parameters for the action execution in the model
	 */
	private void sendActionToModel(String ag, String actionName, String parameters) {
		String sentence = "{\"name\":\""+actionName+"\",\"parameters\":{\"who\":\""+ag+"\",\"otherParameters\":\""+parameters+"\"}}";
		System.out.println("HACER "+ parameters + "POR " + ag);
		awaitedResponse = true;
		Connection.getInstance().send(sentence);
	}
	
	
	
	
	//--------STOP
	/** 
	 * Called before the end of MAS execution.
	 * Closes the ConnectionListener and stops the simulation.
	 */
    @Override
    public void stop() {
        running = false;
		this.threadCL.interrupt();
		this.threadEC.interrupt();
        super.stop();
    }
    
}
