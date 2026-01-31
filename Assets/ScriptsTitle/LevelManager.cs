using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Transitions")]
    public GameObject transitionsContainer;
    private SceneTransition[] transitions;

    [Header("Pre-transition Character Move")]
    [Tooltip("Assign if you want. If empty, LevelManager will try to Find(\"Character\") at runtime.")]
    [SerializeField] private Transform character;

    [Tooltip("Units per second (world units).")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("How far to move to the right before transitioning.")]
    [SerializeField] private float moveDistance = 8f;

    [Tooltip("If the character has a SpriteRenderer, fade its alpha over this duration. Set 0 to skip fade.")]
    [SerializeField] private float spriteFadeTime = 0.25f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (!transitionsContainer)
        {
            Debug.LogError("LevelManager: transitionsContainer is not assigned.");
            return;
        }

        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>(true);
    }

    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        // Find transition safely
        if (transitions == null || transitions.Length == 0)
        {
            Debug.LogError("LevelManager: No transitions found. Did Start() run and container assigned?");
            yield break;
        }

        SceneTransition transition = transitions.FirstOrDefault(t => t.name == transitionName);
        if (transition == null)
        {
            Debug.LogError($"LevelManager: Transition '{transitionName}' not found under transitionsContainer.");
            yield break;
        }

        // Start loading next scene in the background (but don't activate yet)
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);
        sceneLoad.allowSceneActivation = false;

        // 1) Move/fade Character first
        yield return MoveCharacterOffscreen();

        // 2) Fade to black
        yield return transition.AnimateTransitionIn();

        // 3) Activate scene
        sceneLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => sceneLoad.isDone);

        // 4) Fade back out
        yield return transition.AnimateTransitionOut();
    }

    private IEnumerator MoveCharacterOffscreen()
    {
        if (character == null)
        {
            GameObject go = GameObject.Find("Character");
            if (go != null) character = go.transform;
        }

        if (character == null)
        {
            // No character in this scene; just continue
            yield break;
        }

        Vector3 startPos = character.position;
        Vector3 targetPos = startPos + Vector3.right * moveDistance;

        // Move right at constant speed
        while ((character.position - targetPos).sqrMagnitude > 0.0001f)
        {
            character.position = Vector3.MoveTowards(character.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Optional sprite fade, if this is a 2D sprite
        if (spriteFadeTime > 0f)
        {
            SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                float startA = c.a;
                float t = 0f;

                while (t < spriteFadeTime)
                {
                    t += Time.deltaTime;
                    float a = Mathf.Lerp(startA, 0f, t / spriteFadeTime);
                    sr.color = new Color(c.r, c.g, c.b, a);
                    yield return null;
                }

                sr.color = new Color(c.r, c.g, c.b, 0f);
            }
        }

        // Disable after it leaves / fades
        character.gameObject.SetActive(false);
    }
}
