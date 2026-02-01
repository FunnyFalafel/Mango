using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeyFlyToUI : MonoBehaviour
{
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private float popUp = 0.6f;

    public void Play(PlayerInventory inv, Image targetIcon, Camera cam)
    {
        StartCoroutine(Fly(inv, targetIcon, cam));
    }

    private IEnumerator Fly(PlayerInventory inv, Image targetIcon, Camera cam)
    {
        Vector3 start = transform.position;
        Vector3 mid = start + Vector3.up * popUp;

        // convert UI icon position to world position (works for Screen Space Overlay)
        Vector3 targetScreen = RectTransformUtility.WorldToScreenPoint(null, targetIcon.rectTransform.position);
        Vector3 targetWorld = cam.ScreenToWorldPoint(new Vector3(targetScreen.x, targetScreen.y, cam.nearClipPlane + 1f));
        targetWorld.z = start.z;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // pop up then curve to target
            Vector3 a = Vector3.Lerp(start, mid, Mathf.SmoothStep(0, 1, Mathf.Clamp01(t * 1.3f)));
            Vector3 b = Vector3.Lerp(mid, targetWorld, Mathf.SmoothStep(0, 1, t));
            transform.position = Vector3.Lerp(a, b, t);

            float s = Mathf.Lerp(1f, 0.6f, t);
            transform.localScale = new Vector3(s, s, 1f);

            yield return null;
        }

        // once it reaches UI, actually give key
        inv.GiveKey();
        Destroy(gameObject);
    }
}

