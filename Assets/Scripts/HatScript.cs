using UnityEngine;

public class HatScript : MonoBehaviour
{
    public Sprite sprite;
    public HatController hatController;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            hatController.AddSprite(sprite);
            Destroy(gameObject);
        }
    }
}
