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

    void OnTriggerEnter(Collider other) {
        Debug.Log("Collide with " + other.name);
        if(this.tag == "EnemyBullet"){
            if(other.tag == "Player") {
                Units playerStat = other.gameObject.GetComponent<Units>();
                Debug.Log("shooterstat dmg" + shooterStat.dmg);
                playerStat.TakeDmg(shooterStat.dmg);
                //Debug.Log("Player HP: " + playerStat.currHP);        
                Destroy(this.gameObject, 3f);
            }
        }
    }
}
