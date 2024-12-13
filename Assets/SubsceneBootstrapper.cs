using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsceneBootstrapper : MonoBehaviour
{
    [SerializeField] GameObject subsceneStuffPrefab;

    #if UNITY_EDITOR
    void Awake()
    {
        Instantiate(subsceneStuffPrefab);
    }
    #endif
}
