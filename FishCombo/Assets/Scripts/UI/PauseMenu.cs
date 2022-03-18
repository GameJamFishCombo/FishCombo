using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenuUI;
    public AudioSource onClickSound;
    public AudioSource pauseSound;
    public AudioSource pauseBackground;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(GamePaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        onClickSound.Play();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause() {
        onClickSound.Play();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        pauseSound.Play();
        pauseBackground.Play();
    }

    public void LoadMenu() {
        onClickSound.Play();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame() {
        onClickSound.Play();
        Debug.Log("Quiting game. . .");
        Application.Quit();
    }
}

