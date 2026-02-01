/* using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] bool respawnOnHit = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var hp = other.GetComponent<PlayerHealth>();
        if (hp != null) hp.TakeDamage(damage);

        if (respawnOnHit)
        {
            var respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null) respawn.Respawn();
        }
    }
}
*/
