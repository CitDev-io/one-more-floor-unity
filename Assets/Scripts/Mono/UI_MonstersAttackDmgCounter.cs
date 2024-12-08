using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_MonstersAttackDmgCounter : MonoBehaviour
{
    int damage = 0;
    TextMeshProUGUI _txt;
    Vector2 starterPos;
    Vector2 BonkIntercept = new Vector2(128f, -758f);
    RectTransform rectTransform;

public float speed = 3f;
    void Awake()
    {
        // use vector2 position on canvas
        starterPos = GetComponent<RectTransform>().anchoredPosition;
        rectTransform = GetComponent<RectTransform>();
        _txt = transform.Find("Text Box").GetComponent<TextMeshProUGUI>();
        Hide();
    }
    public void Reset() {
        damage = 0;
        GetComponent<RectTransform>().anchoredPosition = starterPos;
        UpdateText();
    }

    public void SetDamage(int dmg) {
        damage = dmg;
        UpdateText();
    }
    public void AddDamage(int dmg) {
        damage += dmg;
        UpdateText();
    }

    void UpdateText() {
        _txt.text = damage.ToString() + "dmg";
    }

    public void Show() {
        _txt.enabled = true;
        GetComponent<Image>().enabled = true;
    }

    public void Hide() {
        _txt.enabled = false;
        GetComponent<Image>().enabled = false;
    }

    public float BONK_DURATION = 1.25f;
    public float BONK_ARCH = 100f;
    public Vector2 BONK_ARCH_BEND = new Vector2(1, 1);
    public Coroutine MoveToBonkIntercept() {
        return StartCoroutine(
            MoveRectTransformAlongArch(
                starterPos,
                BonkIntercept,
                BONK_DURATION,
                BONK_ARCH,
                BONK_ARCH_BEND
            )
        );
    }

    public float BOUNCE_DURATION = 1.25f;
    public float BOUNCE_ARCH = 100f;
    public Vector2 BOUNCE_ARCH_BEND = new Vector2(1, 1);
    public Vector2 BOUNCE_SPOT = new Vector2(88f, -658f);
    public Coroutine BounceBack() {
        return StartCoroutine(
            MoveRectTransformAlongArch(
                BonkIntercept,
                BOUNCE_SPOT,
                BOUNCE_DURATION,
                BOUNCE_ARCH,
                BOUNCE_ARCH_BEND
            )
        );
    }

    public float ATTACK_DURATION = 1.25f;
    public float ATTACK_ARCH = 100f;
    public Vector2 ATTACK_ARCH_BEND = new Vector2(1, 1);
    public Vector2 ATTACK_SPOT = new Vector2(371f, -1277f);
    public Coroutine AttackRun() {
        return StartCoroutine(
            MoveRectTransformAlongArch(
                rectTransform.anchoredPosition,
                ATTACK_SPOT,
                ATTACK_DURATION,
                ATTACK_ARCH,
                ATTACK_ARCH_BEND
            )
        );
    }

    public float TOPROPE_DURATION = 1.25f;
    public float TOPROPE_ARCH = 100f;
    public Vector2 TOPROPE_ARCH_BEND = new Vector2(1, 1);
    public Vector2 TOPROPE_SPOT = new Vector2(371f, -1277f);
    public Coroutine OffTheTopRope() {
        return StartCoroutine(
            MoveRectTransformAlongArch(
                rectTransform.anchoredPosition,
                TOPROPE_SPOT,
                TOPROPE_DURATION,
                TOPROPE_ARCH,
                TOPROPE_ARCH_BEND
            )
        );
    }

    public Coroutine Drift(Vector2 Direction, float duration) {
        return StartCoroutine(
            MoveRectTransformAlongArch(
                rectTransform.anchoredPosition,
                rectTransform.anchoredPosition + Direction,
                duration,
                0,
                Vector2.zero
            )
        );
    }

    IEnumerator MoveRectTransformAlongArch(
        Vector2 start,
        Vector3 end,
        float duration,
        float archHeight,
        Vector2 ArchDirection
    ) {
        float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        // Normalized time (0 to 1)
        float t = elapsedTime / duration;

        // Linear interpolation between start and end positions
        Vector3 position = Vector3.Lerp(start, end, t);

        // Add an offset for the arch (horizontal, along the X-axis)
        float archOffset = archHeight * (1 - Mathf.Pow(2 * t - 1, 2)); // Parabolic curve

        // Apply the arch offset to the X-axis
        position.x += archOffset * ArchDirection.x;
        position.y += archOffset * ArchDirection.y;

        // Move the RectTransform
        rectTransform.anchoredPosition = position;

        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Yield control back to Unity until the next frame
        yield return null;
    }

    // Ensure the final position is exactly the end position
    rectTransform.anchoredPosition = end;
    }

}
