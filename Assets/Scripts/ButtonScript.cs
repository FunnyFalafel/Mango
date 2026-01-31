using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    public Button resumeButton;
    public Button exitButton;
    public Slider volumeSlider;
    public Canvas canvas;
    public float volumeScalar = 1f;

    private bool paused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resumeButton.onClick.AddListener(ClickResume);
        exitButton.onClick.AddListener(ClickExit);
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            paused = !paused;
            canvas.enabled = paused;
        }
    }

    void ClickResume()
    {
        paused = false;
        canvas.enabled = false;
    }

    void ClickExit()
    {
        Debug.Log("Game quitted. (Doesn't work in editor)");
        Application.Quit();
    }

    void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value * volumeScalar;
        Debug.Log("Volume set to: " + (volumeSlider.value * volumeScalar));
    }
}
