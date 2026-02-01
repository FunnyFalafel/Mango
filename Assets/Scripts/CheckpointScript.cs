using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;
        try
        {
            Debug.Log("player entered checkpoint.");
            other.gameObject.GetComponent<PlayerMovementExperimental>().SetCheckpoint(transform.position);
            GetComponent<Collider2D>().enabled = false;
        }
        catch { }
    }
}
