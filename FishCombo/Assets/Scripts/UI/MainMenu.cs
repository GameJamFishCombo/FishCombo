using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource onClickSound;
    public AudioSource onHoverSound;

    public void PlayGame() {
        onClickSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("Application has quit");
        Application.Quit();
    }

    public void OnHover() {
        onHoverSound.Play();
    }
}
