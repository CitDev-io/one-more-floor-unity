using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DOODAD_SelectionCount : MonoBehaviour
{
    [SerializeField] TextMeshPro txt;

    public void SetText(string newtxt) {
        txt.text = newtxt;
    }
}
