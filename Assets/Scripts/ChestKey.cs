using UnityEngine;

public class ChestKey : MonoBehaviour
{
    [SerializeField] private GameObject keyFlyPrefab;
    private bool opened;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        opened = true;

        if (keyFlyPrefab == null)
        {
            Debug.LogError("ChestKey: keyFlyPrefab not assigned.");
            return;
        }

        var cam = Camera.main;
        var k = Instantiate(keyFlyPrefab, transform.position, Quaternion.identity);

        var fly = k.GetComponent<KeyFlyToUI>();
        if (fly == null)
        {
            Debug.LogError("ChestKey: keyFlyPrefab is missing KeyFlyToUI.");
            Destroy(k);
            return;
        }

        fly.Play(inv, inv.KeyIcon, cam);

        Destroy(gameObject); // ok since you want it to disappear
    }
}




