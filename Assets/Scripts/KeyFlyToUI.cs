using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeyFlyToUI : MonoBehaviour
{
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private float popUp = 0.6f;

    private PlayerInventory inv;
    private Image targetIcon;
    private Camera cam;

    public void Play(PlayerInventory inventory, Image uiIcon, Camera camera)
    {
        inv = inventory;
        targetIcon = uiIcon;
        cam = camera;

        if (inv == null || targetIcon == null || cam == null)
        {
            Debug.LogError("KeyFlyToUI missing refs");
            Destroy(gameObject);
            return;
        }

        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        Vector3 start = transform.position;
        Vector3 mid = start + Vector3.up * popUp;

        // UI icon screen position (Overlay safe)
        Vector2 iconScreenPos = RectTransformUtility.WorldToScreenPoint(cam, targetIcon.rectTransform.position);

        // Convert screen -> world at the same Z depth as the key
        float z = cam.WorldToScreenPoint(start).z;
        Vector3 target = cam.ScreenToWorldPoint(new Vector3(iconScreenPos.x, iconScreenPos.y, z));
        target.z = start.z;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // pop up then curve to target
            Vector3 a = Vector3.Lerp(start, mid, Mathf.SmoothStep(0, 1, Mathf.Clamp01(t * 1.3f)));
            Vector3 b = Vector3.Lerp(mid, target, Mathf.SmoothStep(0, 1, t));
            transform.position = Vector3.Lerp(a, b, t);

            yield return null;
        }

        inv.GiveKey();
        Debug.Log("Key reached UI, giving key");
        Destroy(gameObject);
    }
}


