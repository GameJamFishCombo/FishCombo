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

    public float exitTimer = 3f;
    public float exitTime = 3f;

    public GameObject transitionOut;
    public bool cutsceneOver;
    public string levelToLoad;

    void Update(){
        if(cutsceneOver)
            exitTime -= Time.deltaTime;

            
        

        if(exitTime <=0){
            //go to credits or end application
            SceneManager.LoadScene(levelToLoad);
        }
        

    }

    void PlayScene(){
        if(sceneNum < animators.Length) {
            animators[sceneNum].Play("In");
        } else{
                transitionOut.SetActive(true);
                cutsceneOver = true;
        }
    }

    public void Skipscene(){
        AudioManager.PlaySound("PageTurn");
        sceneNum++;
        PlayScene();
    }
}
