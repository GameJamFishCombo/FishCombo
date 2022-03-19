using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossEnemy : Units
{
    // Random rand = new Random();
    Transform enemy;
    bool canMove = false;
    [Tooltip("Duration it takes to LERP between tiles.")]    
    public float duration = 0.09f; //time for lerp
    public float time1 = 1, time2 = 1, time3 = 1, time4 = 1,
    timer1, timer2, timer3, timer4;

    [Header("Action Timings")]
    [Tooltip("Minumum move wait time. MUST be =>duration.")]
    public float minMoveWaitTime = 0.09f; //minimum move wait time, must be >=duration
    public float maxMoveWaitTime = 2f; //max move wait time
    [Tooltip("Minumum fire wait time. MUST be =>duration.")]
    public float minShootSpd = .5f;
    public float maxShootSpd = 3f;
    
    public GameObject projectilePrefab;
    public GameObject spikesPrefab;

    public GameObject playerObj;
    Player player;
    public Animator animator;
    public Transform firePoint;

    public void Awake() {
        enemy = GetComponent<Transform>();
        timer1 = time1;
        timer2 = time2;
        timer3 = time3;
        timer4 = time4;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();
    }

    public void FixedUpdate() {
        float randomNum = Mathf.Floor((int)UnityEngine.Random.Range(0,4));
        Vector3 move = new Vector3(0, 0, 0);
        bool checkBounds = true, occupied = false;
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        timer3 -= Time.deltaTime;
        timer4 -= Time.deltaTime;
        Ray ray = new Ray(transform.position, -transform.right);

        if(timer1 <= 0) {
            time1 = UnityEngine.Random.Range(minMoveWaitTime, maxMoveWaitTime);
            timer1 = time1;

            move = SetDirection(randomNum); //gets next direction it will move

            if(randomNum == 0) { //forward
                ray = new Ray(transform.position, transform.right);
                occupied = OccupiedTile(ray);
            } else if(randomNum == 1) { //back
                ray = new Ray(transform.position, -transform.right);
                occupied = OccupiedTile(ray);
            } else if(randomNum == 2) { //up
                ray = new Ray(transform.position, transform.forward);
                occupied = OccupiedTile(ray);
            } else if(randomNum == 3) { //down
                ray = new Ray(transform.position, -transform.forward);
                occupied = OccupiedTile(ray);
            }

            if(!occupied) {
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }
        }

        if(timer2 <= 0) {
            time2 = UnityEngine.Random.Range(minShootSpd, maxShootSpd);
            timer2 = time2;

            StartCoroutine(Launch());
        }

        if(timer3 <= 0) {
            time3 = UnityEngine.Random.Range(minShootSpd, maxShootSpd);
            timer3 = time3;

            StartCoroutine(Spikes());
        }

        if(timer4 <= 0) {
            time4 = UnityEngine.Random.Range(minShootSpd, maxShootSpd);
            timer4 = time4;

            // StartCoroutine(Launch());
        }

    }

    public bool inBounds(Vector3 vec) {
        return (vec.x < 4 || vec.x > 7 || vec.z < 0  || vec.z > 3);
    }

    public override void Die() {
        base.Die();
        Destroy(this.gameObject);
        Debug.Log(enemy + " dead.");
    }

    public override void Sound() {

    }

    IEnumerator Launch() {
        animator.SetBool("Attack2",true);
        yield return new WaitForSeconds(0.5f);
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 lookDirection = new Vector3(-1f, 0, 0);
        projectile.Launch(lookDirection, 300);
        animator.SetBool("Attack2", false);
        
        // PlaySound(throwSound);
    }

    IEnumerator Spikes() {
        Debug.Log("Spawn spikes");
        animator.SetBool("Attack1",true);
        Vector3 playerPos = player.getCurrPosition();
        GameObject projectileObject = Instantiate(spikesPrefab, playerPos, Quaternion.identity);
        yield return null;
        animator.SetBool("Attack1",false);
        //some animation that shows where obj gonna spawn
        SpikesProjectile projectile = projectileObject.GetComponent<SpikesProjectile>();
    }
}
