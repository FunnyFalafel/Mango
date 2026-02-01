using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private bool rolling;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides");
        Debug.Log("Loaded dice sides: " + diceSides.Length);
    }

    private void Update()
    {
        if (rolling) return;
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var cam = Camera.main;
            if (cam == null) return;

            Vector2 world = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Collider2D hit = Physics2D.OverlapPoint(world);

            if (hit != null && hit.gameObject == gameObject)
                StartCoroutine(Roll());
        }
    }

    private IEnumerator Roll()
    {
        rolling = true;

        int randomSide = 0;
        for (int i = 0; i < 20; i++)
        {
            randomSide = Random.Range(0, 6); // 0..5
            rend.sprite = diceSides[randomSide];
            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log("Final side: " + (randomSide + 1));
        rolling = false;
    }
}
