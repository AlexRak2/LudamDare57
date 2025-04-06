using UnityEngine;
using System.Collections;

namespace LD57.UI
{
[RequireComponent(typeof(CanvasGroup))]
public class LD57CanvasGroup : MonoBehaviour
{
    public bool inProgress, cancelAction;
    [HideInInspector] public CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Fade out using a coroutine
    public void FadeOut(float duration = 0.5f, bool _interactable = false, bool _blocksRaycasts = false)
    {
        StartCoroutine(FadeOutCoroutine(duration, _interactable, _blocksRaycasts));
    }

    // Fade in using a coroutine
    public void FadeIn(float duration = 0.5f, bool _interactable = true, bool _blocksRaycasts = true)
    {
        StartCoroutine(FadeInCoroutine(duration, _interactable, _blocksRaycasts));
    }

    private IEnumerator FadeOutCoroutine(float duration, bool _interactable, bool _blocksRaycasts)
    {
        canvasGroup.interactable = _interactable;
        canvasGroup.blocksRaycasts = _blocksRaycasts;

        while (inProgress)
        {
            cancelAction = true;
            yield return null;
        }

        inProgress = true;
        cancelAction = false;

        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (canvasGroup.alpha > 0 && !cancelAction) {
            t += Time.unscaledDeltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        if (!cancelAction) {
            canvasGroup.alpha = 0f;
        }
        inProgress = false;
    }

    private IEnumerator FadeInCoroutine(float duration, bool _interactable, bool _blocksRaycasts)
    {
        canvasGroup.interactable = _interactable;
        canvasGroup.blocksRaycasts = _blocksRaycasts;

        while (inProgress) {
            cancelAction = true;
            yield return null;
        }

        inProgress = true;
        cancelAction = false;

        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (canvasGroup.alpha < 1 && !cancelAction)
        {
            t += Time.unscaledDeltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, t);
            yield return null;
        }

        if (!cancelAction) {
            canvasGroup.alpha = 1f;
        }
        inProgress = false;
    }
    public void CGEnable()
    {
        cancelAction = true;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void CGDisable()
    {
        cancelAction = true;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
}