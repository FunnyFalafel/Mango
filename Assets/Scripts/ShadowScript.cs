using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    public GameObject otherShadow;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            //particles?
            col.gameObject.GetComponent<PlayerMovementExperimental>().RemoveShadow();
            Destroy(otherShadow);
            Destroy(gameObject);
        }
    }    
}
