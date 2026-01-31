using System.Collections;
using UnityEngine;

[System.Serializable]
public class CrossFade : SceneTransition
{
    public CanvasGroup crossFade;

    public override IEnumerator AnimateTransitionIn()  => FadeTo(1f, 1f);
    public override IEnumerator AnimateTransitionOut() => FadeTo(0f, 1f);

    private IEnumerator FadeTo(float target, float duration)
    {
        float start = crossFade.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            crossFade.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }

        crossFade.alpha = target;
    }
}
