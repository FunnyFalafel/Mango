using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public Vector3 nextCheckPoint;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Entry!");
        if (other.gameObject.tag != "Player") return;
        try
        {
            Debug.Log("next checkpoint.");
            other.gameObject.GetComponent<PlayerMovementExperimental>().SetCheckpoint(nextCheckPoint);
        }
        catch { }
    }
}
