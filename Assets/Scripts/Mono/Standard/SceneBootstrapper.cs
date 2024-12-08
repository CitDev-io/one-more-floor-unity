using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
    [SerializeField] GameObject DDOLobj;

    void Awake()
    {
        if (GameObject.FindObjectOfType<GameController_DDOL>() == null)
        {
            Instantiate(DDOLobj);
        }
    }
}
