using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PushProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rigidbody;
    public int damageMulitplier = 2;
    public float projectileSpeed = 450;
    public float pushDuration = 0.07f;
    public GameObject shooter;
    Units shooterStat;
    Grid grid;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        shooterStat = shooter.GetComponent<Units>();
        grid = GameManager.grid;
    }

    void Update() {

        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }

        if(grid.inBounds(transform.position, "Projectile")){
            Destroy(gameObject);
        }
    }

    public void Launch(Vector3 direction) {
        rigidbody.AddForce(direction * projectileSpeed);
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy") {
            //BasicEnemy enemyBehavior = other.gameObject.GetComponent<BasicEnemy>();
            //enemyBehavior.pushTo(MovementInput.Right, pushDuration);
            Units enemyStat = other.gameObject.GetComponent<Units>();
            enemyStat.TakeDmg(shooterStat.dmg * damageMulitplier);
            enemyStat.pushTo(MovementInput.Right, pushDuration);
            //Debug.Log("Enemy HP: " + enemyStat.currHP);        
            //Destroy(gameObject);
        }
    }

    public bool inBounds(Vector3 vec) {
        if(vec.x < 0 || vec.x > 7 || vec.z < 0  || vec.z > 3) {
            return true;
        }

        return false;
    }
}
