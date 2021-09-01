using UnityEngine;
using TMPro;

namespace citdev {
public class UI_ArmorTicker : MonoBehaviour
{
    GridGameManager _rc;
    TextMeshProUGUI _txt;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GridGameManager>();
        _txt = GetComponent<TextMeshProUGUI>();
    }
    private void OnGUI() {
        _txt.text = _rc.Armor + " / 10";
    }
}
}
