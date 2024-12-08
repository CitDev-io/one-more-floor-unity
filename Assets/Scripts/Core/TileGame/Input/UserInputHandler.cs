public class UserInputHandler {
    IChainBuilder chainBuilder;
    public UserInputHandler(ChainBuilder _chainBuilder) {
        chainBuilder = _chainBuilder;
    }

    public void UserStartSelection(Tile tile)
    {
        chainBuilder.StartChain(tile);
    }

    public void UserEndSelection()
    {
        chainBuilder.EndChain();
    }

    public void UserIndicatingTile(Tile tile)
    {
        chainBuilder.IndicateTile(tile);
    }
}