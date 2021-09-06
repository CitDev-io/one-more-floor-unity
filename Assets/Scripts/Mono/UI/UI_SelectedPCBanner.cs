using UnityEngine;
using TMPro;

public class UI_SelectedPCBanner : MonoBehaviour
{
    GameController_DDOL _gc;
    [SerializeField] TextMeshProUGUI _nameTxt;
    [SerializeField] TextMeshProUGUI _lvlTxt;

    private void Start()
    {
        _gc = GameObject.FindObjectOfType<GameController_DDOL>();
    }
    private void OnGUI()
    {
        _nameTxt.text = _gc.CurrentCharacter.Name;
        _lvlTxt.text = "level " + _gc.CurrentCharacter.Level();
    }
}
