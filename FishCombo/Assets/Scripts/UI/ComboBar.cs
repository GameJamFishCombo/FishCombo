using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Slider slider;
    Animator animator;
    public float lerpSpd = 1;
    float time = 0;
    public float speed = 2;
    public float newCombo;
    public float oldCombo;
    public float timeStartedLerping;

    void Start(){
        animator = gameObject.GetComponent<Animator>();
    }
    public void SetMaxCombo(int maxCombo) {
        slider.maxValue = maxCombo;
        slider.value = 0;
    }

    public void SetCombo(float comboNew, float comboOld) {
        // slider.value = combos;
        time = 0;
        newCombo = comboNew;
        oldCombo = comboOld;
        timeStartedLerping = Time.time;
        //StartCoroutine(LerpCombo(comboNew, comboOld));
    }

    void Update(){
        LerpComboValue();
    }


    // IEnumerator LerpCombo(float newCombo, float oldCombo) {
    //     while(time < 1) {
    //         time += (lerpSpd * Time.deltaTime);
    //         slider.value = Mathf.Lerp(oldCombo, newCombo, time);
    //         if(newinputqueued){
    //             StartCoroutine(LerpCombo(newcombo,slider.value);
    //             StopCoroutine(LerpCombo());
    //             StopCoroutine(LerpCombo();
    //         }
    //         yield return null;
    //     }
    // }
    
    public void LerpComboValue(){
        
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / speed;
        slider.value = Mathf.Lerp(slider.value, newCombo, percentageComplete); //Time.deltaTime * speed
    }

    public void PlayAnimation(string animation){
        animator.Play(animation);
    }

}
