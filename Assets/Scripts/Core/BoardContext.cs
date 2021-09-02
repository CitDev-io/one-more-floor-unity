public class BoardContext
{
    private readonly PlayerCharacter _pc;
    private readonly int _stage;

    public BoardContext(PlayerCharacter playerCharacter, int stage) {
        _pc = playerCharacter;
        _stage = stage;        }

    public PlayerCharacter PC {
        get { return _pc; }
    }

    public int Stage {
        get { return _stage; }
    }
}
