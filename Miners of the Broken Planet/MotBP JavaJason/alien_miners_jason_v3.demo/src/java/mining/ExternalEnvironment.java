package mining;

import jason.asSyntax.Literal;
import jason.asSyntax.Structure;
import jason.asSyntax.parser.ParseException;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.concurrent.Semaphore;
import java.util.logging.Level;
import java.util.logging.Logger;

import connection.Connection;
import connection.ConnectionListener;
import connection.NewExternalConnector;

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
	protected Semaphore actionResponseSem = new Semaphore(1, true);
    
	// Los segundos que durará la simulación
    public static final int SIM_TIME = 60;  

    
	protected static ExternalEnvironment instance;
	//Threads (ConnectionListener and External Connector)
	protected Thread threadCL;
	protected Thread threadEC;
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
     * Instances a new ConnectionListener and ExternalConnector as threads. The main thread awaits outside environment to be loaded,
     * meanwhile both threads start receiving messages.
     * @param run
     */
    @Override
    public void init(String[] args) {
        sleep  = Integer.parseInt(args[0]);
		
        //Crea un ConnectionListener en thread para escuchar todas las acciones recibidas desde Unity
        
        this.threadExternalConnection(true);
        
        this.threadSocketConnection(true);
		
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
	 * Runs a thread that awaits for model responses about environment
	 * @param run
	 * @throws InterruptedException
	 */
	private void threadSocketConnection(boolean run) {
		if (run) {
			this.threadCL = new Thread(new ConnectionListener(), "connectionListener" );
			//Empezará a escuchar la conexión
			this.threadCL.start();
		}
	}
	
	/**
	 * Runs a thread that awaits model messages
	 * @param run
	 * @throws InterruptedException
	 */
	private void threadExternalConnection(boolean run) {
		if (run){
			
			try {
				this.threadEC = new Thread(NewExternalConnector.getInstance(), "externalConnection" );
			} catch (InterruptedException e) {
				
			}
			//Empezará a escuchar la conexión
			this.threadEC.start();
		}
	}
	
	/**
	 * Enum that differs the kind of changes may be solicited from outside the environment, through the socket.
	 * @author Sergio González Jiménez
	 *
	 */
	private enum ReceivableMessages {
		ADDPERCEPT, REMOVEPERCEPT, REMOVEPERCEPTSBYUNIF, CLEARPERCEPTS, INFORMCHANGES, //Changes on MAS
		ENVIRONMENTDONE, ACTIONRESPONSE //Communication with the model
	}
	
	/**
	 * Interprets and performs the action within the message <b>data</b>.
	 * <li>(FROM Outside TO Jason)</li>
	 * @param data
	 * @return Returns if an action request was successful (only to use from ExecuteAction() )
	 * @throws JSONException
	 * @throws ParseException 
	 * @throws InterruptedException 
	 */
	public boolean performReceivedMessage(String data) throws JSONException, ParseException, InterruptedException{
		boolean executed = false;
		JSONObject json = new JSONObject(data);
		String name = json.getString("name");
		ReceivableMessages header = ReceivableMessages.valueOf(name.toUpperCase());
		
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
		
		switch(header) {
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
			case REMOVEPERCEPTSBYUNIF:
				if (who == null)
					removePerceptsByUnif(what);
				else {
					int r = removePerceptsByUnif(who, what);
					System.out.println("REMOVED: " + r);
					System.out.println("REMOVE BY UNIFICATION " + what + " " + parameters.getString("what"));
				}
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
				if (success.contains("yes"))
					executed = true;
				break;
			default:
				break;
		}
		return executed;
	}
	
	
	
    //--------EXECUTEACTION
    /**
     * Transforms an AgentSpeak action to a real one on the model.
     * <li>(FROM Jason TO Outside)</li>
     * @param ag : Agent's name
	 * @param action : Action to be made
     */
    @Override
    public boolean executeAction(String ag, Structure action) {
		String actionName="";
		String parameters="";
		
		boolean success = false;
		
    	try {
			if (sleep > 0) {
			    Thread.sleep(sleep); // Slows down the actions for doing them able to be seen
			}
			actionName = action.getFunctor();
			parameters = action.getTerms().toString();
			
			String response = sendActionRequestToModel(ag, actionName, parameters);
			success = performReceivedMessage(response);

        } catch (InterruptedException e) {
        } catch (Exception e) {
			logger.log(Level.SEVERE, "error executing " + action + " for " + ag, e);
        }

        return success; 
    }
    
	/**
	 * Sends an action to the model to perform it.
	 * @param ag : Who wants to execute the action
	 * @param actionName
	 * @param parameters : Needed parameters for the action execution in the model
	 * @throws InterruptedException 
	 * @throws ParseException 
	 * @throws JSONException 
	 */
	protected String sendActionRequestToModel(String ag, String actionName, String parameters) throws InterruptedException, JSONException, ParseException {
		String request = "{\"name\":\""+actionName+"\",\"parameters\":{\"who\":\""+ag+"\",\"otherParameters\":\""+parameters+"\"}}";
		return NewExternalConnector.getInstance().sendEnvironmentRequest(request);
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
		Connection.getInstance().stopConnection();
        super.stop();
    }
    
}
