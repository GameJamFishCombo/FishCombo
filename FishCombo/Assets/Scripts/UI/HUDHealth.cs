using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealth : MonoBehaviour
{
    public Slider slider;
    public float lerpSpd = 2;
    bool lerpHP = false;
    float time = 0;
    public int health;
    public Animator animator;
    void Start() {
        SetMaxHealth(health);
    }

    public void SetMaxHealth(int HP) {
        slider.maxValue = HP;
        slider.value = HP;
    }

    public void SetHealth(float health, float originalHP) {
        time = 0;
        if(animator !=null){
            //if(gameObject.tag == "Player")
            animator.Play("HealthWiggle",-1,0.0f);
            // if(gameObject.tag == "Enemy"){
            //     animator.Play("HealthGrow",-1,0.0f);  
            // }
        }
        if(!lerpHP)
            StartCoroutine(LerpHP(health, originalHP));

        // slider.value = health;
    }

    IEnumerator LerpHP(float health, float originalHP) {
        lerpHP = true;

        while(time < 1) {
            time += (lerpSpd * Time.deltaTime);
            slider.value = Mathf.Lerp(originalHP, health, time);
            yield return null;
        }

        lerpHP = false;
    }

}
