using UnityEngine;
using UnityEngine.UI;

public class UI_SpecialIcon : MonoBehaviour
{
    GridGameManager _rc;
    bool isSet = false;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
    }
    private void Update()
    {
        if (isSet) return;

        if (_rc.Board.CurrentCharacterType() == CharacterType.Rogue) {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Tiles/poison1");
        }
        isSet = true;
    }
}
