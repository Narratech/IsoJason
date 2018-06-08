package mining;

import java.util.logging.Level;

import jason.asSyntax.Structure;

public class AlephMinersEnvironment extends ExternalEnvironment{
	public String[] agClasses;
	
	public AlephMinersEnvironment() {
	    super();
		agClasses = new String[5];
	    agClasses[0] = "soldier";
	    agClasses[1] = "explorer";
	    agClasses[2] = "explorer";
	    agClasses[3] = "collector";
	    agClasses[4] = "collector";
	}

	
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
			
    		int numAg = Integer.parseInt(ag.substring(5)) - 1;
			if(ag.contains("miner")) {
	    		if (agClasses[numAg].contains("collector")) {
				    Thread.sleep(500);
				}else if (agClasses[numAg].contains("soldier")) {
					if(action.getFunctor() != "kill")
						Thread.sleep(200); 
				}
			}
			else if (ag.contains("alien"))
				Thread.sleep(200);

        } catch (InterruptedException e) {
        } catch (Exception e) {
			logger.log(Level.SEVERE, "error executing " + action + " for " + ag, e);
        }

        return success; 
    }
}
