using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Cutscene : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator[] animators;
    public AudioSource paper;
    public int sceneNum = 0;
    public float timer = 3;
    public float time;

    public float exitTimer;
    public float exitTime;

    public GameObject transitionOut;
    public bool cutsceneOver;
    public string levelToLoad;
    void Start(){
    }


    void Update(){
        if(cutsceneOver)
            exitTime -= Time.deltaTime;

        time-=Time.deltaTime;
        if(time <=0){
            time = timer;
            sceneNum++;
            PlayScene();
        }
        if(exitTime <=0){
            SceneManager.LoadScene(levelToLoad);
        }

    }
    void PlayScene(){
        if(sceneNum < animators.Length)
            animators[sceneNum].Play("In");
            else
            transitionOut.SetActive(true);
            cutsceneOver = true;
    }

    void Skipscene(){
        time = 0;
    }

    
}
