using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FileTouch : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip sfx;
    public LayerMask groundLayer;

    private readonly HashSet<(Tilemap tm, Vector3Int cell)> touched = new();

    private void OnCollisionEnter2D(Collision2D col) => Handle(col);
    private void OnCollisionStay2D(Collision2D col)  => Handle(col);

    private void Handle(Collision2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) == 0)
            return;

        Tilemap tm = col.collider.GetComponentInParent<Tilemap>();
        if (tm == null) return;

        foreach (var contact in col.contacts)
        {
            Vector2 sample = contact.point - contact.normal * 0.02f;
            Vector3Int cell = tm.WorldToCell(sample);

            if (!tm.HasTile(cell)) continue;

            var key = (tm, cell);
            if (touched.Contains(key)) continue;

            touched.Add(key);

            if (sfxSource && sfx)
                sfxSource.PlayOneShot(sfx);

            break;
        }
    }

    public void ResetTouched() => touched.Clear();
}
