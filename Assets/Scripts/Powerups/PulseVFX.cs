using UnityEngine;

public class PulseVFX : MonoBehaviour
{
    public float duration = 0.35f;
    public float startScale = 0.8f;
    public float endScale = 6.0f;
    public float yOffset = 0.05f;

    float t;
    Vector3 basePos;
    Material mat;
    Color startColor;

    void Awake()
    {
        basePos = transform.position;
        transform.position = basePos + Vector3.up * yOffset;

        // Grab material instance so we can fade alpha
        var r = GetComponentInChildren<Renderer>();
        if (r != null)
        {
            mat = r.material; // instanced
            startColor = mat.color;
        }

        transform.localScale = Vector3.one * startScale;
    }

    void Update()
    {
        t += Time.deltaTime;
        float u = duration <= 0 ? 1f : Mathf.Clamp01(t / duration);

        // Expand
        float s = Mathf.Lerp(startScale, endScale, u);
        transform.localScale = Vector3.one * s;

        // Fade out (if material supports color alpha)
        if (mat != null)
        {
            Color c = startColor;
            c.a = Mathf.Lerp(startColor.a, 0f, u);
            mat.color = c;
        }

        if (u >= 1f) Destroy(gameObject);
    }
}
