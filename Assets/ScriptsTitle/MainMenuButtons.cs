using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void Play() {
        SceneManager.LoadScene("GameplayScene");
    }

    public void Quit() {
        Application.Quit();
    }
}
