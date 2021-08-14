using UnityEngine;
using TMPro;

public class UI_RoundProgress : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject check;

    RoundController _rc;

    private void Awake()
    {
        _rc = GameObject.FindObjectOfType<RoundController>();
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
        text.text = "x" + (_rc.KillRequirement - _rc.Kills);
    }
}