using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Image keyIcon; // drag KeyIcon here
    private bool hasKey;

    private void Awake()
    {
        RefreshUI();
    }

    public bool HasKey() => hasKey;

    public void GiveKey()
    {
        hasKey = true;
        RefreshUI();
    }

    public bool ConsumeKey()
    {
        if (!hasKey) return false;
        hasKey = false;
        RefreshUI();
        return true;
    }

    private void RefreshUI()
    {
        if (keyIcon == null) return;
        // either show/hide:
        keyIcon.gameObject.SetActive(hasKey);
        // OR if you prefer alpha:
        // var c = keyIcon.color; c.a = hasKey ? 1f : 0f; keyIcon.color = c;
    }
}
