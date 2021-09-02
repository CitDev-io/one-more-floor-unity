public class RogueFactory : PCFactory
{
    public RogueFactory() {
        
    }

    public override PlayerCharacter GetPC() {
        return new PC_Rogue();
    }
}
