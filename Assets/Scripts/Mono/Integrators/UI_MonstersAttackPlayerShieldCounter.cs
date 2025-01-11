using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_MonstersAttackPlayerShieldCounter : MonoBehaviour
{
    int shields = 0;
    TextMeshProUGUI _txt;
    Vector2 starterPos;
    Image anotherRefNeededForShieldOnThereForNow;
    public Vector2 BonkIntercept = new Vector2(466f, -621f);
    public float speed = 3f;
    RectTransform rectTransform;
    void Awake()
    {
        // use vector2 position on canvas
        starterPos = GetComponent<RectTransform>().anchoredPosition;
        rectTransform = GetComponent<RectTransform>();
        _txt = transform.Find("Text Box").GetComponent<TextMeshProUGUI>();
        anotherRefNeededForShieldOnThereForNow = transform.Find("Shield").GetComponent<Image>();
        Hide();
    }
    public void Reset() {
        shields = 0;
        GetComponent<RectTransform>().anchoredPosition = starterPos;
        UpdateText();
    }

    public void AdjustShields(int newVal) {
        shields = newVal;
        UpdateText();
    }

    void UpdateText() {
        _txt.text = "x " + shields.ToString();
    }

    public void Show() {
        _txt.enabled = true;
        GetComponent<Image>().enabled = true;
        anotherRefNeededForShieldOnThereForNow.enabled = true;
    }

    public void Hide() {
        _txt.enabled = false;
        GetComponent<Image>().enabled = false;
        anotherRefNeededForShieldOnThereForNow.enabled = false;
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
