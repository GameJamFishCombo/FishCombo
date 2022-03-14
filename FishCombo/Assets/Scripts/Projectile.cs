using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rigidbody;
    GameObject enemy;
    public GameObject shooter;
    Units shooterStat;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        shooterStat = shooter.GetComponent<Units>();
    }

    void Update()
    {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector3 direction, float force)
    {
        rigidbody.AddForce(direction * force);
    }

    void OnTriggerEnter(Collider other) {
        Units enemyStat = other.gameObject.GetComponent<Units>();

        if(other.gameObject.layer == 6 || other.gameObject.layer == 8) {
            return;
        }

        enemyStat.TakeDmg(shooterStat.dmg);
        Debug.Log("Enemy HP: " + enemyStat.currHP);        
        Destroy(gameObject);
    }
}
