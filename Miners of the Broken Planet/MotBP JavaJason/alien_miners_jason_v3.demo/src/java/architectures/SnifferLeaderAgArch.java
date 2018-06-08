package architectures;

import connection.NewExternalConnector;
import jason.architecture.AgArch;
import jason.asSemantics.Message;
import jason.asSyntax.Atom;
import jason.infra.centralised.CentralisedAgArch;
import jason.infra.centralised.MsgListener;

public class SnifferLeaderAgArch extends AgArch implements MsgListener{

	 @Override
    public void init() {
        if (getArchInfraTier() instanceof CentralisedAgArch)
            CentralisedAgArch.addMsgListener(this);
    }

    // method called-back when some message is exchanged
    public void msgSent(Message m) {
    	if(m.getReceiver().contains("leader")) {
    		if(m.isTell()) {
	    		String a = "msg_sent(" + new Atom(m.getSender())+"," + m.getPropCont().toString() + ")";
		        try {
		        	String jsonMsg = "{\"name\":\"msgReceived\",\"parameters\":{\"message\":\""+ a +"\"}}";
		        	//System.out.println(jsonMsg);
					NewExternalConnector.getInstance().sendInformationMsg(jsonMsg);
	
				} catch (InterruptedException e1) {
		            e1.printStackTrace();
		        }
    		}
    	}
    }
}
