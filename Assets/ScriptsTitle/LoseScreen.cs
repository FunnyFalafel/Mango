using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
