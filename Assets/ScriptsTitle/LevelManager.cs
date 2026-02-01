using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject transitionsContainer;
    private SceneTransition[] transitions;

    public Transform character;
    public float moveSpeed = 5f;
    public float moveDistance = 8f;
    public float spriteFadeTime = 0.25f;

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
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>(true);
    }

    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        SceneTransition transition = transitions.FirstOrDefault(t => t.name == transitionName);
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);
        sceneLoad.allowSceneActivation = false;

        yield return MoveCharacterOffscreen();
        yield return transition.AnimateTransitionIn();
        sceneLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => sceneLoad.isDone);
        yield return transition.AnimateTransitionOut();
    }

    private IEnumerator MoveCharacterOffscreen()
    {
        if (character == null)
        {
            GameObject go = GameObject.Find("Character");
            if (go != null) character = go.transform;

        }

        Vector3 startPos = character.position;
        Vector3 targetPos = startPos + Vector3.right * moveDistance;

        // Need to add animation here
        while ((character.position - targetPos).sqrMagnitude > 0.0001f)
        {
            character.position = Vector3.MoveTowards(character.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

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

        character.gameObject.SetActive(false);
    }
}