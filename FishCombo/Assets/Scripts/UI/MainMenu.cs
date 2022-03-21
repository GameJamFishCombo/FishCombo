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
    
    void Update(){
        
        if(playing)
            time-=Time.deltaTime;
        if(time<=0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Start(){
        Cursor.visible = true;
    }

    public void PlayGame() {
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
