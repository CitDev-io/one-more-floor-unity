using UnityEngine;
using TMPro;
// Floating Text
public partial class GameBridge
{
    [SerializeField] GameObject dmgPrefab;
    [SerializeField] GameObject hpPrefab;
    [SerializeField] GameObject apPrefab;
    [SerializeField] GameObject gpPrefab;
    [SerializeField] GameObject xpPrefab;
    [SerializeField] GameObject floaterParent;
    [SerializeField] Transform floaterPositionRef;

    void FloatExp(int xp) {
        var go = Instantiate(
            xpPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + xp + " XP";
    }
    void FloatDamage(int dmg, Vector2 spot)
    {
        var go = Instantiate(
            dmgPrefab,
            spot,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = dmg + " dmg";
    }
    void FloatHeal(int dmg)
    {
        var go = Instantiate(
            hpPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " HP";
    }
    void FloatArmor(int dmg)
    {
        var go = Instantiate(
            apPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " AP";
    }
    void FloatGold(int dmg)
    {
        var go = Instantiate(
            gpPrefab,
            floaterPositionRef.position,
            Quaternion.identity,
            floaterParent.transform
        );
        go.GetComponent<TextMeshProUGUI>().text = "+" + dmg + " GP";
    }
}