using UnityEngine;

public class DeathBoxScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;
        try
        {
            Debug.Log("player died.");
            other.gameObject.GetComponent<PlayerMovementExperimental>().Die();
        }
        catch { }
    }
}
