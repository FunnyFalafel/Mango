using UnityEngine;

public class TitleMovement : MonoBehaviour
{
    public float offsetMultiplier = 1f;   // how far it can move (in world units)
    public float smoothTime = 0.3f;

    private Vector3 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //transform.position = Vector3.Smoothamp(transform.position, startPosition * (offset * offsetMultiplier), ref velocity, smoothTime);
        if (Camera.main == null) return; // ensure your camera is tagged "MainCamera"


        Vector3 viewport = Camera.main.ScreenToViewportPoint(Input.mousePosition); // x,y in [0,1]
        Vector3 centered = new Vector3(viewport.x - 0.5f, viewport.y - 0.5f, 0f);

        Vector3 target = startPosition + centered * offsetMultiplier;

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }
}

