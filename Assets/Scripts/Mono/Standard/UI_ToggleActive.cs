using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToggleActive : MonoBehaviour
{
    public void ToggleActive() {
        if (GetComponent<CanvasGroup>()) {
            if (GetComponent<CanvasGroup>().alpha == 0) {
                GetComponent<CanvasGroup>().alpha = 1;
                GetComponent<CanvasGroup>().interactable = true;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            } else {
                GetComponent<CanvasGroup>().alpha = 0;
                GetComponent<CanvasGroup>().interactable = false;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            return;
        }
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
