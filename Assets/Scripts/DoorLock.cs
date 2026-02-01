using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [SerializeField] private Collider2D solidCollider; // non-trigger
    private bool opened;

    private void Awake()
    {
        if (solidCollider == null)
        {
            var cols = GetComponents<Collider2D>();
            foreach (var c in cols)
                if (!c.isTrigger) { solidCollider = c; break; }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        if (!inv.ConsumeKey()) return;

        opened = true;

        if (solidCollider != null) solidCollider.enabled = false;
        else Debug.LogError("DoorLock: no solidCollider assigned/found.");
    }
}


