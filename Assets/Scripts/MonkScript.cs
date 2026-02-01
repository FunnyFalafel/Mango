using UnityEngine;

public class MonkScript : MonoBehaviour
{
    public GameObject canvas;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("collided with monk");
            canvas.SetActive(true);
            col.gameObject.GetComponent<PlayerMovementExperimental>().StartCutscene(canvas);
        }
    }
}
