using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenuUI;
    public AudioSource onClickSound;
    public AudioSource onHoverSound;
    public AudioSource pauseSound;
    public AudioSource unpauseSound;
    public AudioSource pauseBackground;
    public AudioListener audiolistener;
    public GameObject clickBlocker;
    public bool loadMain = false;
    public float timer = 3;
    public float time = 3;
    public GameObject fadeOutObject;
    public GameObject gameOverObj;
    public bool cutscene;
    
    void Start(){
        if(!cutscene)
        Cursor.visible = false;
        onHoverSound.ignoreListenerPause = true;
        pauseSound.ignoreListenerPause = true;
        pauseBackground.ignoreListenerPause = true;
        onClickSound.ignoreListenerPause = true;
        time = timer;
    }

    public void PlayHover(){
        onHoverSound.Play();
    }

    void Update()
    {
        if(loadMain)
            time-=Time.deltaTime;

        if(time <= 2.4f){
            fadeOutObject.SetActive(true);
        }

        if(time <= 0){
            LoadMenu();
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(!loadMain){
                if(GamePaused) {
                    Resume();
                } else {
                    Pause();
                }
            }
        }
    }

    public void GameOver() {
        LoadMenu();
        /*gameOverObj.SetActive(true);

        time-=Time.deltaTime;

        Debug.Log(time);

        if(time <= 2.4f){
            fadeOutObject.SetActive(true);
        }

        if(time <= 0){
            LoadMenu();
        }*/
    }

    public void Resume() {
        if(!loadMain){
            unpauseSound.Play();
            AudioListener.pause = false;
            pauseBackground.Pause();
            onClickSound.Play();
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GamePaused = false;
        }
    }

    public void Pause() {
        AudioListener.pause = true;
        onClickSound.Play();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        pauseSound.Play();
        pauseBackground.Play();
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void ClickLoadMenu(){
        loadMain = true;
        Time.timeScale = 1f;
        onClickSound.Play();
        clickBlocker.SetActive(true);
    }

    public void QuitGame() {
        if(!loadMain){
            onClickSound.Play();
            Debug.Log("Quiting game. . .");
            Application.Quit();
        }
    }
}

