using UnityEngine;

public class MouseParticle : MonoBehaviour
{
    public Sprite spriteA;
    public Sprite spriteB;

    private SpriteRenderer spriteRenderer;
    private float lifespan;
    private float spawnTime;
    private Vector2 dir;
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(Random.value>0.5f) spriteRenderer.sprite = spriteA;
        else spriteRenderer.sprite = spriteB;
        spawnTime = Time.fixedTime;
        lifespan = 0.1f + Random.value * 0.4f;
        dir = Random.insideUnitCircle*2.5f;
        velocity = new Vector3(dir.x, dir.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedTime - spawnTime > lifespan) Destroy(gameObject);
        transform.position += velocity * Time.deltaTime;
    }
}
