using UnityEngine;
using System.Collections.Generic;

public class MouseTrackerScript : MonoBehaviour
{
    private LinkedList<float> angleChanges;
    private float cumulativeAngle;
    public float angleRate = 0.1f;
    private float lastAngleUpdate;
    private Vector2 center;
    private Vector3 mousePos3D;
    private Vector2 mousePos;
    private Vector2 lastMousePos;
    private Vector2 lastAngleVector;
    private Vector2 thisAngleVector;
    private float speed;
    public GameObject particle;
    public GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        cumulativeAngle = 0f;
        mousePos3D = Input.mousePosition;
        lastMousePos = new Vector2(mousePos3D.x, mousePos3D.y);
        lastAngleVector = new Vector2(mousePos3D.x, mousePos3D.y)-center;
        angleChanges = new LinkedList<float>();
        lastAngleUpdate = Time.fixedTime;

    }

    // Update is called once per frame
    void Update()
    {
        mousePos3D = Input.mousePosition;
        mousePos = new Vector2(mousePos3D.x, mousePos3D.y);
        speed = Mathf.Sqrt(Mathf.Pow((lastMousePos.x-mousePos.x)*16/Screen.width, 2)+ Mathf.Pow((lastMousePos.y - mousePos.y) * 9 / Screen.height, 2)) / Time.deltaTime;
        if (speed > 200f)
        {
            //Debug.Log("Mouse Speed: " + speed);
            Instantiate(particle, Camera.main.ScreenToWorldPoint(mousePos3D)+new Vector3(0f,0f,10f), Quaternion.identity);
        }
        lastMousePos = mousePos;

        if(Time.fixedTime-lastAngleUpdate > angleRate)
        {
            thisAngleVector = mousePos - center;
            float dAngle = Vector2.SignedAngle(lastAngleVector, thisAngleVector);
            if(angleChanges.Count == 5)
            {
                cumulativeAngle -= angleChanges.Last.Value;
                angleChanges.RemoveLast();
            }
            cumulativeAngle += dAngle;
            angleChanges.AddFirst(dAngle);
            lastAngleUpdate += angleRate;

            if (Mathf.Abs(cumulativeAngle) >= 300f)
            {
                cumulativeAngle = 0f;
                angleChanges = new LinkedList<float>();
                player.GetComponent<PlayerMovementExperimental>().Warp();
            }
            lastAngleVector = thisAngleVector;
        }

    }
}
