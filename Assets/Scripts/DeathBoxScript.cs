using UnityEngine;

public class DeathBoxScript : MonoBehaviour
{
    void Start()
    {
        //Debug.Log("I wanna live!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entry!");
        if (other.gameObject.tag != "Player") return;
        try
        {
            Debug.Log("player died.");
            other.gameObject.GetComponent<PlayerMovementExperimental>().Die();
        }
        catch { }
    }
}
