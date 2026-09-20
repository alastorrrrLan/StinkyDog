using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ButtonScale")]
    public float normalScale = 1.0f;
    public float hoverScale = 1.5f;
    public float animDuration = 0.1f;

    RectTransform rect;
    Coroutine scaleCoroutine;
    Vector3 targetScale;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        targetScale = Vector3.one * normalScale;
        rect.localScale = targetScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartScale(hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartScale(normalScale);
    }

    void StartScale(float scale)
    {
        targetScale = Vector3.one * scale;

        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleAnim());
    }

    IEnumerator ScaleAnim()
    {
        Vector3 startScale = rect.localScale;
        float time = 0f;

        while (time < animDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / animDuration;
            t = t * t * (3f - 2f * t);
            rect.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        rect.localScale = targetScale;
        scaleCoroutine = null;
    }
}