using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rigidbody;
    GameObject enemy;
    public GameObject shooter;
    Units shooterStat;
    public GameObject particles;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        shooterStat = shooter.GetComponent<Units>();
    }

    void Update() {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector3 direction, float force) {
        rigidbody.AddForce(direction * force);
    }

    void OnTriggerEnter(Collider other) {
        if(this.tag == "PlayerBullet") {
            if(other.tag == "Enemy") {
                Units enemyStat = other.gameObject.GetComponent<Units>();
                enemyStat.TakeDmg(shooterStat.dmg);
                //Debug.Log("Enemy HP: " + enemyStat.currHP);        
                Instantiate(particles,transform.position,Quaternion.identity);
                AudioManager.PlaySound("BulletCollide");
                Destroy(gameObject);
            }
        } else if(this.tag == "EnemyBullet"){
            if(other.tag == "Player") {
                Units playerStat = other.gameObject.GetComponent<Units>();
                Instantiate(particles,transform.position,Quaternion.identity);
                playerStat.TakeDmg(shooterStat.dmg);
                //Debug.Log("Player HP: " + playerStat.currHP);        
                Destroy(gameObject);
            }
        }
    }

    public bool inBounds(Vector3 vec) {
        if(vec.x < 0 || vec.x > 7 || vec.z < 0  || vec.z > 3) {
            return true;
        }

        return false;
    }
}
