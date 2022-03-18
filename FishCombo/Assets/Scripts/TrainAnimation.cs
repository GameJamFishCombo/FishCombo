using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public float timer = 6;
    public float time;

    public Animator animator;
    
    void Start()
    {
        time = timer;   
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0){
            PlayAnimation();
            SetTimer();
        }
    }

    void PlayAnimation(){
        animator.Play("Train");
    }

    void SetTimer(){
            timer = Random.Range(12,23);
            time = timer;
    }
}
