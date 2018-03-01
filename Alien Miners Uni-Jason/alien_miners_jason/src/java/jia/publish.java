package jia;

import jason.asSemantics.DefaultInternalAction;
import jason.asSemantics.TransitionSystem;
import jason.asSemantics.Unifier;
import jason.asSyntax.Term;

import connection.ExternalConnector;

public class publish extends DefaultInternalAction{
	public Object execute(TransitionSystem ts, Unifier un, Term[] terms) throws Exception {
		try{
			String msg = terms[0].toString();
			String direction = terms[1].toString().toUpperCase();
			String jsonMsg;
			if (direction.equals("RECEIVED")) {
				jsonMsg = "{\"name\":\"publishReceived\",\"parameters\":{\"message\":\""+ msg +"\"}}";
				ExternalConnector.getInstance().sendInformationMsg(jsonMsg);
			}
				
			if (direction.equals("SENT")) {
				jsonMsg = "{\"name\":\"publishSent\",\"parameters\":{\"message\":\""+ msg +"\"}}";
				ExternalConnector.getInstance().sendInformationMsg(jsonMsg);
			}
			return un.unifies(terms[0], terms[0]);
		}catch (Throwable e) {
			e.printStackTrace();
			return false;
		}
	}

}
