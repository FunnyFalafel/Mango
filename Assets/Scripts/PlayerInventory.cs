using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Image keyIcon;
    private bool hasKey;

    public Image KeyIcon => keyIcon;

    private void Awake() => RefreshUI();

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
        Debug.Log("Has key: " + hasKey);
    if (keyIcon)
    {
        keyIcon.gameObject.SetActive(true); // FORCE SHOW
        keyIcon.color = Color.white;
    }

    }
}

