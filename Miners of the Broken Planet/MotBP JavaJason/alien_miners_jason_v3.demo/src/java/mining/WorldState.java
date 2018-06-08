package mining;

import org.json.JSONException;
import org.json.JSONObject;

import connection.NewExternalConnector;

public class WorldState {
	
	private static WorldState instance;

    public final int CLEAN = 0;
    public final int AGENT = 2;
    public final int OBSTACLE = 4;

	private static int height = 0;
	private static int width = 0;
	private static int[][] state;
	
	private static int updateCounter = 0;
	
	public static WorldState get() throws InterruptedException, JSONException{
		if (instance == null)
			instance = new WorldState();
		// Update the state each 10 get() requests (with a minimum impact on inteligence and increasing speed)
		//if(updateCounter == 0)
		updateState();
			
		//increaseUpdateCounter();
		
		return instance;
	}
	
	private static void increaseUpdateCounter() {
		updateCounter = (updateCounter + 1) % 2;
	}
	
	private static void updateState() throws InterruptedException, JSONException{
		//Connection.getInstance().send("{\"name\":\"requestState\",\"parameters\":{}}");
		String sentence = "{\"name\":\"stateRequest\",\"parameters\":{}}";
		NewExternalConnector extConnInstance = NewExternalConnector.getInstance();
		
		String responseData = extConnInstance.sendInformationRequest(sentence);
		
		JSONObject js = new JSONObject(responseData);
		JSONObject param = js.getJSONObject("parameters");
		String state = param.getString("state");
		updateStateMatrix(state);
		//printState();
	}
	
	private static void updateStateMatrix(String data){
		char c;
		String w = "";
		String h = "";
		c = data.charAt(1);
		
		int iter = 1;
		while(c != ')'){
			w += c;
			iter++;
			c = data.charAt(iter);
		}
		iter += 2;
		c = data.charAt(iter);
		while(c != ')'){
			h += c;
			iter++;
			c = data.charAt(iter);
		}
		width = Integer.parseInt(w);
		height = Integer.parseInt(h);

		data = data.substring(iter+2, data.length()-1);
		String[] dataSplit = data.split(",");
		
		if(state == null)
			state = new int [width][height];
		
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				iter = (i*width)+j;
				state[i][j] = Integer.parseInt(dataSplit[iter]);
			}
		}
	}
	
	public static void printState(){
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
				System.out.print(state[i][j]+",");
			}
			System.out.print("\n");
		}
	}
	
	public int getHeight(){
		return height;
	}
	
	public int getWidth(){
		return width;
	}
	
	public boolean inGrid(int x, int y){
		return y >= 0 && y < height && x >= 0 && x < width;
	}
	
	public boolean isFreeOfObstacle(int x, int y){
		return isFree(OBSTACLE, x, y);
	}

    public boolean isFree(int obj, int x, int y) {
        return inGrid(x, y) && (state[x][y] & obj) == 0;
    }
    
    public boolean isFree(int x, int y) {
        return inGrid(x, y) && (state[x][y] & OBSTACLE) == 0 && (state[x][y] & AGENT) == 0;
    }

}
