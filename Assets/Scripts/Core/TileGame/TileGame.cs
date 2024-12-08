public class TileGame
{
    public TileGameEventSystem Events = new();
    public GameState State;
    public ChainBuilder Chain;
    public UserInputHandler Input;

    public TileGame() {

        State = new GameState(Events);

        Chain = new ChainBuilder(State, State);

        // UI INPUT Access Point

        // thought process: wait why does it need chain?
            // at this point in the code, there aren't other events. just chain.
            // we COULD fire events and have the chain builder listen to them - decide later.

            // this is just a class where you can pass around
            // public methods that translates and calls ChainBuilder

            // we want this to ultimately be the UI access point. If it fires events in the future,
            // we'll be passing Events instead of Chain
        Input = new UserInputHandler(Chain);
    }
}
