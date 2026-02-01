using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void Play() {
        LevelManager.Instance.LoadScene("SampleScene_Brendan", "CrossFade");
    }

    public void Quit() {
        Application.Quit();
    }
}
