using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Slider slider;
    Animator animator;
    void Start(){
        animator = gameObject.GetComponent<Animator>();
    }
    public void SetMaxCombo(int maxCombo) {
        slider.maxValue = maxCombo;
        slider.value = 0;
    }

    public void SetCombo(int combos) {
        slider.value = combos;
    }

    public void PlayAnimation(string animation){
        animator.Play(animation);
    }
}
