public class WarriorFactory : PCFactory
{
    public WarriorFactory() {
    }

    public override PlayerCharacter GetPC() {
        return new PC_Warrior();
    }
}
