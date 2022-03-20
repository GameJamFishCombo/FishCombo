using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesProjectile : MonoBehaviour
{
    public GameObject shooter;
    Units shooterStat;

    void Awake() {
        shooterStat = shooter.GetComponent<Units>();
    }

    void Start() {
        //StartCoroutine(Sound());
        AudioManager.PlaySound("Spikes");
        Destroy(this.gameObject, 1f);
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Collide with " + other.name);
        if(this.tag == "EnemyBullet"){
            if(other.tag == "Player") {
                Units playerStat = other.gameObject.GetComponent<Units>();
                Debug.Log("shooterstat dmg" + shooterStat.dmg);
                playerStat.TakeDmg(shooterStat.dmg);
                //Debug.Log("Player HP: " + playerStat.currHP);        
            }
        }
    }
    
    // IEnumerator Sound(){
    //     yield return new WaitForSeconds(0.3f);
    //     AudioManager.PlaySound("Spikes");
    // }
}
