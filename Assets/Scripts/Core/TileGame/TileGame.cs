public class TileGame
{
    public TileGameEventSystem Events = new();
    public GameState State;
    public ChainBuilder Chain;
    public UserInputHandler Input;

    public TileGame() {
        // GameState is the "Domain" of the game. It has the state but also can run logical events emitted by its children. Ultimately will change state as needed and emit events for a UI
        // we'll eventually be feeding customizations from "[Game Mode concept]" into the State object here:
        // such as: Encounter, Player, Tileselector, etc (these are just vanilla impls to start)
        State = new GameState(Events);

        Chain = new ChainBuilder(State, State);

        // UI INPUT Access Point
        Input = new UserInputHandler(Chain);
    }
}
