using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileAnimator : MonoBehaviour
{

    public Animator animator;
    public Animator conBlockAnimator;
    public GameObject particles;
    // Start is called before the first frame update
    
    void OnTriggerEnter(Collider other){
        if(other.tag == "PlayerBullet" || other.tag == "PushBullet" || other.tag == "StillBullet"){
            animator.Play("BlueHighlight");
        }

        if(other.tag == "PushBullet"){
            conBlockAnimator.Play("Slam");
            Instantiate(particles,transform.position+(Vector3.up/2),Quaternion.identity);
        }
        if(other.tag == "EnemyBullet" || other.tag == "YellowTile"){
            animator.Play("RedHighlight");
        }
        
    }

    void OnTriggerExit(Collider other){
        animator.Play("GrayHighlight");
    }
    

    
}
