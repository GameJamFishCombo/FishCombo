using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StillProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rigidbody;
    public int damageMulitplier = 5;
    public float lifeSpan = 0.2f; 
    public GameObject shooter;
    float lifeTimer = 0f;
    Units shooterStat;
    Grid grid;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        shooterStat = shooter.GetComponent<Units>();
        grid = GameManager.grid;
    }

    void Update() {
        if(lifeTimer >= lifeSpan){
            Destroy(gameObject);
        }
        lifeTimer += Time.deltaTime;

        if(grid.inBounds(transform.position, "Projectile")){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(!(other.tag == "Player" && shooter.tag == "Player") & !(other.tag == "Enemy" && gameObject.tag == "Enemy") & (other.tag == "Enemy" || other.tag == "Player")) {
            Units enemyStat = other.gameObject.GetComponent<Units>();
            Debug.Log("Player AOE did: " + (shooterStat.dmg + damageMulitplier));
            enemyStat.TakeDmg(shooterStat.dmg * damageMulitplier);
            //Debug.Log("Enemy HP: " + enemyStat.currHP);        
            Destroy(gameObject);
        }
    }
}

