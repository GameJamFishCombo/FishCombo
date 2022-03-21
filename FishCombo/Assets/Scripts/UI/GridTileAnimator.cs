using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileAnimator : MonoBehaviour
{

    public Animator animator;
    public Animator conBlockAnimator;
    public GameObject particles;
    // Start is called before the first frame update
    
    public List<Collider> triggerList = new List<Collider>();

    void Update(){
        if(triggerList.Count > 0){
            int count = 0;
            foreach(Collider c in triggerList){
                if(c != null && c.tag != "Player")
                    count++;
            }

            if(count <= 0){
                animator.Play("GrayHighlight");
                triggerList.Clear();
            }
        }
    }

    void OnTriggerEnter(Collider other){
        triggerList.Add(other);
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
    void OnTriggerStay(Collider other){
        if(other.tag == "EnemyBullet" || other.tag == "YellowTile"){    
            animator.Play("RedHighlight");
        }
    }
    void OnTriggerExit(Collider other){
        triggerList.Remove(other);
        animator.Play("GrayHighlight");
    }
    

    
}
