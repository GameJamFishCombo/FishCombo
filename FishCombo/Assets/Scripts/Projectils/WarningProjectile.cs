using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningProjectile : MonoBehaviour
{
    public float lifeSpan = 1f;
    public GameObject spawnObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeSpan <= 0){
            if(spawnObject != null)
                Instantiate(spawnObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        lifeSpan -= Time.deltaTime;
    }
}
