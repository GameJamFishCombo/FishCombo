using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileAnimator : MonoBehaviour
{

    public Animator animator;
    
    // Start is called before the first frame update
    
    void OnTriggerEnter(Collider other){
        if(other.tag == "PlayerBullet" || other.tag == "PushBullet" || other.tag == "StillBullet"){
            animator.Play("BlueHighlight");
        }
        if(other.tag == "EnemyBullet"){
            animator.Play("RedHighlight");
        }
        
    }

    void OnTriggerExit(Collider other){
        animator.Play("GrayHighlight");
    }
}
