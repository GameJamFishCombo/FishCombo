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

        StartCoroutine(LerpCombo(comboNew, comboOld));
    }

    IEnumerator LerpCombo(float newCombo, float oldCombo) {
        while(time < 1) {
            time += (lerpSpd * Time.deltaTime);
            slider.value = Mathf.Lerp(oldCombo, newCombo, time);
            yield return null;
        }
    }   

    public void PlayAnimation(string animation){
        animator.Play(animation);
    }

}
