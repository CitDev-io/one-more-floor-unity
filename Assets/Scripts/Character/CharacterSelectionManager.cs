using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace citdev {
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
                    factory = new WarriorFactory(0);
                    break;
                case "rogue":
                    factory = new RogueFactory(0);
                    break;
                default:
                    break;
            }

            PlayerCharacter pc = factory.GetPC();
            _gc.CurrentCharacter = pc;
        }
    }
}
