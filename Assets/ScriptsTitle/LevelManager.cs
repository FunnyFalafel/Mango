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

        if (character != null)
        {
            Animator anim = character.GetComponent<Animator>();
            if (anim != null) anim.SetBool("isWalk", true);
            float moved = 0f;
            while (moved < moveDistance)
            {
                float step = moveSpeed * Time.deltaTime;
                character.Translate(Vector3.right * step, Space.World);
                moved += step;
                yield return null;
            }

            character.gameObject.SetActive("false");
        }

        if (transition != null)
            yield return transition.AnimateTransitionIn();

        sceneLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => sceneLoad.isDone);

        if (transition != null)
            yield return transition.AnimateTransitionOut();
    }

}