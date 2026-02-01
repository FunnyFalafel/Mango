using UnityEngine;
using System.Collections.Generic;

public class HatController : MonoBehaviour
{
    private SpriteRenderer spriteRendherher;
    public List<Sprite> sprites;
    private int spriteIndex;
    private int spriteCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRendherher = GetComponent<SpriteRenderer>();
        spriteCount = sprites.Count;
        
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
    }
}
