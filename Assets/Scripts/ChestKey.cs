using UnityEngine;
using UnityEngine.UI;

public class ChestKey : MonoBehaviour
{
    [SerializeField] private GameObject keyFlyPrefab;  // prefab with KeyFlyToUI
    [SerializeField] private Image keyIcon;            // KeyIcon in UI

    private bool opened;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        opened = true;

        // spawn fly key
        var cam = Camera.main;
        GameObject k = Instantiate(keyFlyPrefab, transform.position, Quaternion.identity);
        k.GetComponent<KeyFlyToUI>().Play(inv, keyIcon, cam);

        // optional: disable chest collider so it can't be re-opened
        GetComponent<Collider2D>().enabled = false;

        // optional: change sprite to "open chest" here
    }
}

