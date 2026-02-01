using UnityEngine;
using System.Collections.Generic;

public class HatController : MonoBehaviour
{
    private SpriteRenderer spriteRendherher;
    public List<Sprite> sprites;
    private int spriteIndex;
    private int spriteCount;
    private PlayerMovementExperimental playah;
    private Vector3 left;
    private Vector3 right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRendherher = GetComponent<SpriteRenderer>();
        spriteCount = sprites.Count;
        left = new Vector3(-0.05f, 0.5f, 0f);
        right = new Vector3(0.05f, 0.5f, 0f);
        playah = transform.parent.gameObject.GetComponent<PlayerMovementExperimental>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            spriteIndex++;
            if (spriteIndex >= sprites.Count)
            {
                spriteIndex = 0;
            }
            spriteRendherher.sprite = sprites[spriteIndex];
        }
        if(playah.lastDir == -1)
        {
            transform.position = transform.parent.position + right;
        }
        else
        {
            transform.position = transform.parent.position + left;
        }
    }

    public void AddSprite(Sprite sprite)
    {
        sprites.Add(sprite);
        spriteCount++;
    }
}
