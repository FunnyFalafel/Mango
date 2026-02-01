using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [SerializeField] private Collider2D solidCollider; // the blocking collider (non-trigger)
    [SerializeField] private Animator anim;            // optional door anim
    [SerializeField] private string openTrigger = "Open";

    private bool opened;

    private void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        if (!inv.ConsumeKey()) return; // no key -> nothing happens

        opened = true;

        if (anim != null) anim.SetTrigger(openTrigger);
        if (solidCollider != null) solidCollider.enabled = false;

        // If you don't have a separate solid collider:
        // GetComponent<Collider2D>().enabled = false;
    }
}

