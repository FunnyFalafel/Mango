using UnityEngine;

public class TitleMovement : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = .3f;

    private Vector2 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //transform.position = Vector3.Smoothamp(transform.position, startPosition * (offset * offsetMultiplier), ref velocity, smoothTime);

    }
}
