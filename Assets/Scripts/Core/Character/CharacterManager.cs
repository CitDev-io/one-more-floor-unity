public class CharacterManager {
    PC_Warrior war = new PC_Warrior();
    PC_Rogue rog = new PC_Rogue();

    public PlayerCharacter GetCharacterByClassName(string name) {
        if (name == "rogue") return rog;

        return war;
    }
}
