using UnityEngine;

public class ShadowSpawner : MonoBehaviour
{
    public GameObject otherSpawner;
    public Vector3 shadowPos;
    public GameObject shadows;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<PlayerMovementExperimental>().AddShadow(shadowPos)) 
            { 
                Instantiate(shadows, shadowPos, Quaternion.identity);

                Destroy(otherSpawner);
                Destroy(gameObject);
            }
        }
    }
}
