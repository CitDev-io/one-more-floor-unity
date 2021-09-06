using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    GameController_DDOL _gc;

    private void Start()
    {
        _gc = GameObject.FindObjectOfType<GameController_DDOL>();
    }

    public void SelectCharacterClass(string name) {
        _gc.CurrentCharacter = _gc.characterManager.GetCharacterByClassName(name);
    }
}
