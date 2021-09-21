public class BoardContext
{
    private readonly PlayerCharacter _pc;
    private readonly Stage _stage;

    public BoardContext(PlayerCharacter playerCharacter, Stage stage) {
        _pc = playerCharacter;
        _stage = stage;        }

    public PlayerCharacter PC {
        get { return _pc; }
    }

    public Stage Stage {
        get { return _stage; }
    }
}
