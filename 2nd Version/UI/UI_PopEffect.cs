using UnityEngine;
using System.Collections;

public class UI_PopEffect : MonoBehaviour
{
    public float scaleUp = 1.2f;
    public float speed = 8f;
    public float stayTime = 0.1f;

    private Vector3 originalScale;
    private Coroutine currentRoutine;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void PlayPopUpEffect()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PopAnimation());
    }

    IEnumerator PopAnimation()
    {
        Vector3 targetScale = originalScale * scaleUp;

        float t = 0f;

        // 🔼 POP UP (fast + punchy)
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float eased = EaseOutBack(t);
            transform.localScale = Vector3.LerpUnclamped(originalScale, targetScale, eased);
            yield return null;
        }

        transform.localScale = targetScale;

        // ⏸ stay
        yield return new WaitForSeconds(stayTime);

        t = 0f;

        // 🔽 POP DOWN (smooth)
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float eased = EaseInOut(t);
            transform.localScale = Vector3.Lerp(targetScale, originalScale, eased);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    float EaseOutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }

    float EaseInOut(float x)
    {
        return x < 0.5f
            ? 2 * x * x
            : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }
}