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
        _nameTxt.text = "warrior";
        _lvlTxt.text = "level 1";
    }
}
