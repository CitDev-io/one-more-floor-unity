using UnityEngine;
using TMPro;

public class UI_RoundProgress : MonoBehaviour
{
    //not in use
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject check;

    GameBridge _rc;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<GameBridge>();
    }

    private void Start()
    {
        _rc.OnRoundEnd += OnRoundEnd;
        text.gameObject.SetActive(true);
        check.SetActive(false);
    }

    private void OnDestroy()
    {
        _rc.OnRoundEnd -= OnRoundEnd;
    }

    void OnRoundEnd()
    {
        text.gameObject.SetActive(false);
        check.SetActive(true);
    }

    void OnGUI() {
        if (_rc.Board == null) return;
        text.text = "x??togo";
    }
}
