using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCORE_ScoringSummary : MonoBehaviour
{
    GameController_DDOL _gc;
    [SerializeField] TextMeshProUGUI roundPoints;
    [SerializeField] TextMeshProUGUI roundBonus;
    [SerializeField] TextMeshProUGUI movesBonus;
    [SerializeField] TextMeshProUGUI movesCount;
    [SerializeField] TextMeshProUGUI roundScore;
    [SerializeField] TextMeshProUGUI totalScore;

    private void Start()
    {
        roundPoints.gameObject.SetActive(false);
        roundBonus.gameObject.SetActive(false);
        movesBonus.gameObject.SetActive(false);
        movesCount.gameObject.SetActive(false);
        roundScore.gameObject.SetActive(false);
        totalScore.gameObject.SetActive(false);

        _gc = GameObject.FindObjectOfType<GameController_DDOL>();
        StartCoroutine("DoScoreDisplay");
    }

    void SetValues()
    {
        roundPoints.text = "-";
        roundBonus.text = "-";
        movesBonus.text = "-";
        movesCount.text = _gc.PreviousRoundMoves + "";
        roundScore.text = "-";
        totalScore.text = "-";
    }

    IEnumerator DoScoreDisplay()
    {
        SetValues();
        yield return new WaitForSeconds(0.3f);
        roundPoints.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        roundBonus.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        movesBonus.gameObject.SetActive(true);
        movesCount.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        roundScore.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        totalScore.gameObject.SetActive(true);
    }
}
