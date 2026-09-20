using UnityEngine;
using UnityEngine.UI;

public class WarningBorder : MonoBehaviour
{
    [Header("Borders")]
    [SerializeField] private RawImage top;
    [SerializeField] private RawImage bottom;
    [SerializeField] private RawImage left;
    [SerializeField] private RawImage right;

    [Header("Pulse Settings")]
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 0.65f;
    [SerializeField] private float pulseSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        SetAlpha(alpha);
    }

    private void SetAlpha(float alpha)
    {
        SetImageAlpha(top, alpha);
        SetImageAlpha(bottom, alpha);
        SetImageAlpha(left, alpha);
        SetImageAlpha(right, alpha);
    }

    private void SetImageAlpha(RawImage image, float alpha)
    {
        if (image == null) return;
        Color color = image.color;

        color.a = alpha;

        image.color = color;
    }
}
