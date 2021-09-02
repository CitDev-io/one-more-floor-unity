using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    GameController_DDOL _gc;

    private void Start()
    {
        _gc = GameObject.FindObjectOfType<GameController_DDOL>();
    }

    public void SelectCharacterClass(string name) {
        PCFactory factory = null;
        switch (name.ToLower()) {
            case "warrior":
                factory = new WarriorFactory();
                break;
            case "rogue":
                factory = new RogueFactory();
                break;
            default:
                break;
        }

        PlayerCharacter pc = factory.GetPC();
        _gc.CurrentCharacter = pc;
    }
}
