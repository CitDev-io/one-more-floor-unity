using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DLTME_UISceneRiff : MonoBehaviour
{
    public void Button1() {
        SceneManager.LoadScene("Menu-Inventory", LoadSceneMode.Additive);
    }
    public void Button2() {
        Debug.Log("Button 2");
    }
}
