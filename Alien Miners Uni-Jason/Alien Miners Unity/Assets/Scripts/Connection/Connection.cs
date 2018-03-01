public abstract class Connection {

    private static Connection instance;

    public static Connection GetInstance() {
        if (instance == null)
            instance = new ConnectionImp();
        return instance;
    }

    public abstract void Initialize();
    public abstract void Send(bool send, object ev);
    public abstract string Receive();
}
