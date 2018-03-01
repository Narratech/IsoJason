package jia;

import jason.asSemantics.DefaultInternalAction;
import jason.asSemantics.TransitionSystem;
import jason.asSemantics.Unifier;
import jason.asSyntax.Atom;
import jason.asSyntax.Term;

import org.json.JSONObject;

import connection.ExternalConnector;

public class action_await extends DefaultInternalAction {
	public Object execute(TransitionSystem ts, Unifier un, Term[] terms) throws Exception {
		try{
			String msg = terms[0].toString();
			
			String request, response;
			request = "{\"name\":\"actionAwait\",\"parameters\":{\"message\":\""+ msg +"\"}}";
			ExternalConnector.getInstance().sendInformationMsg(request);
			
			response = ExternalConnector.getInstance().receiveInformationMsg();
			JSONObject json = new JSONObject(response);
			JSONObject params = json.getJSONObject("parameters");
			String sentence = params.getString("sentence");
			
			return un.unifies(terms[1], new Atom(sentence));
		}catch (Throwable e) {
			e.printStackTrace();
			return false;
		}
	}
	
}
    