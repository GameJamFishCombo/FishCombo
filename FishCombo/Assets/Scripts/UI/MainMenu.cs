using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource onClickSound;
    public AudioSource onHoverSound;
    public float time = 2.3f;
    public float timer = 2.3f;
    public bool playing;
    public GameObject fadeOutObject;
    public AudioSource track;
    void Update(){
        
        if(playing)
            time-=Time.deltaTime;
        if(time<=0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Start(){
        Shop.numVisited = 0;
        AudioListener.pause = false;
        Cursor.visible = true;
        StartCoroutine(FadeMusic.StartFade(track, 4f, 1f));
    }

    public void PlayGame() {
        StartCoroutine(FadeMusic.StartFade(track, 1.7f, 0f));
        fadeOutObject.SetActive(true);
        playing = true;
        onClickSound.Play();
        
    }

    public void QuitGame() {
        Debug.Log("Application has quit");
        Application.Quit();
    }

    public void OnHover() {
        onHoverSound.Play();
    }
}
