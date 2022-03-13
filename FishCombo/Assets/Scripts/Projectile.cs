using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rigidbody;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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

    void OnTriggerEnter(Collider other)
    {
        // EnemyController e = other.collider.GetComponent<EnemyController>();
        // if (e != null)
        // {
        //     e.Fix();
        // }

        if(other.gameObject.layer == 6 || other.gameObject.layer == 8) {
            return;
        }
        
        Destroy(gameObject);
    }
}
