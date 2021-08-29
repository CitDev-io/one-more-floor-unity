using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace citdev {
    public class UI_SelectedPCBanner : MonoBehaviour
    {
        GameController_DDOL _gc;
        [SerializeField]
        TextMeshProUGUI _txt;

        private void Start()
        {
            _gc = GameObject.FindObjectOfType<GameController_DDOL>();
        }
        private void OnGUI()
        {
            _txt.text = _gc.CurrentCharacter.Name;
        }
    }
}
