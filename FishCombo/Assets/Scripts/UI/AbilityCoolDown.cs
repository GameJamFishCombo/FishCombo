using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCoolDown : MonoBehaviour
{

    public Slider slider;
    public Animator animator;

    void Start(){
        animator = gameObject.GetComponent<Animator>();
    }


    public void SetCooldown(int cooldown) {
        slider.maxValue = cooldown;
        slider.value = cooldown;
    }

    public void SetCombo(int cooldown) {
        slider.value = cooldown;
    }

    public void Animate(string animation){
        animator.Play(animation);
    }
}
