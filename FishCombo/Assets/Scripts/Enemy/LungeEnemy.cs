using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LungeEnemy : Units
{
    Transform enemy;
    bool canMove = true;
    [Tooltip("Duration it takes to LERP between tiles.")]    
    public float duration = 0.09f; //time for lerp
    public float time1 = 1, time2 = 1, timer1, timer2;

    [Header("Action Timings")]
    [Tooltip("Minumum move wait time. MUST be =>duration.")]
    public float minMoveWaitTime = 0.09f; //minimum move wait time, must be >=duration
    public float maxMoveWaitTime = 2f; //max move wait time
    [Tooltip("Minumum fire wait time. MUST be =>duration.")]
    public float minStabSpd = .5f;
    public float maxStabSpd = 3f;

    Transform player;
    public float lungeDuration = 0.03f;
    public int numMeleeHits = 3;
    public float meleeHitDelay = 0.01f;
    public GameObject meleeProjectile;

    public Animator animator;

    public void Awake() {
        enemy = GetComponent<Transform>();
        timer1 = time1;
        timer2 = time2;
    }

    public void FixedUpdate() {
        float randomNum = Mathf.Floor((int)UnityEngine.Random.Range(0,4));
        Vector3 move = new Vector3(0, 0, 0);
        bool checkBounds = true, occupied = false;
        timer1 -= Time.deltaTime;
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
                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }
        }

        timer2 -= Time.deltaTime;

        if(timer2 <= 0) {
            time2 = UnityEngine.Random.Range(minStabSpd, maxStabSpd);
            timer2 = time2;

            StartCoroutine(Lunge(transform.position + (new Vector3(-4f, 0, 0))));
        }

    }
    
    public bool inBounds(Vector3 vec, string tag) {
        if(tag == "Enemy") 
            return (vec.x < 4 || vec.x > 7 || vec.z < 0  || vec.z > 3);
            
        

        if(tag == "Projectile") 
            return (vec.x < 0 || vec.x > 7 || vec.z < 0  || vec.z > 3);
            
        return false;
    }

    public override void Die() {
        base.Die();
        Destroy(this.gameObject);
        Debug.Log(enemy + " dead.");
    }

    IEnumerator Lunge(Vector3 targetPosition){ // KILL ME
        canMove = false;
        float time = 0;
        Vector3 startPosition = enemy.position;

        while (time < lungeDuration) {
            enemy.position = Vector3.Lerp(startPosition, targetPosition, time / lungeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        
        enemy.position = targetPosition;
        time = 0;

        for(int i = 0; i < (numMeleeHits-1); i++){
            LaunchMelee();
            while (time < meleeHitDelay) {
                time += Time.deltaTime;
                yield return null;
            }
            time = 0;
        }
        StartCoroutine(AttackAnimation());

        time = 0;
        while (time < lungeDuration) {
            enemy.position = Vector3.Lerp(targetPosition, startPosition, time / lungeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        enemy.position = startPosition;
        canMove = true;
    }


    IEnumerator AttackAnimation(){
        animator.SetBool("Attack",true);
        yield return null;
        animator.SetBool("Attack", false);
    }

    void LaunchMelee(){
        Vector3 spawnPosition = enemy.position + new Vector3(-1f, 0, 0); //tile behind
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = enemy.position + new Vector3(0, 0, 0); //current tile
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = enemy.position + new Vector3(1f, 0, 0); //tile infront
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);
    }

}
