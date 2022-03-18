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

    public void Update() {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;

        if(timer1 <= 0) {
            time1 = UnityEngine.Random.Range(minMoveWaitTime, maxMoveWaitTime);
            timer1 = time1;

            Move();
        }

        if(timer2 <= 0) {
            time2 = UnityEngine.Random.Range(minStabSpd, maxStabSpd);
            timer2 = time2;

            StartCoroutine(Lunge(transform.position + (new Vector3(-4f, 0, 0))));
        }

        
    }

    void Move() {
        int randomNum = (int)UnityEngine.Random.Range(0,4);
        Vector3 move = new Vector3(0, 0, 0);
        bool checkBounds = true;

        switch(randomNum) {
            case 0: //move up
                move = new Vector3(0, 0, 1f) + enemy.position;

                // max 7 max 3 min 4 min 0
                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;
            case 1: //move left
                move = new Vector3(-1f, 0, 0) + enemy.position;
                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;

            case 2: //move south
                move = new Vector3(0, 0, -1f) + enemy.position;
                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;
            case 3: //move right
                move = new Vector3(1f, 0, 0) + enemy.position;
                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;
        }
    }

    public bool pushTo(MovementInput direction, float pushDuration){
        Vector3 move;
        bool checkBounds = true;
        transform.position = new Vector3((float)Math.Round(transform.position.x), transform.position.y, (float)Math.Round(transform.position.z));
        switch(direction) {
            case MovementInput.Up:
                move = new Vector3(0, 0, 1f) + enemy.position;

                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;

                break;
            case MovementInput.Left: //move left
                move = new Vector3(-1f, 0, 0) + enemy.position;
                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;

            case MovementInput.Down: //move south
                move = new Vector3(0, 0, -1f) + enemy.position;
                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;
                
            case MovementInput.Right: //move right
                move = new Vector3(1f, 0, 0) + enemy.position;
                checkBounds = inBounds(move, "Enemy");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;
        }
        return false;
    }

    
    public bool inBounds(Vector3 vec, string tag) {
        if(tag == "Enemy") {
            if(vec.x < 4 || vec.x > 7 || vec.z < 0  || vec.z > 3) {
                return true;
            } else {
                return false;
            }
        }

        if(tag == "Projectile") {
            if(vec.x < 0 || vec.x > 7 || vec.z < 0  || vec.z > 3) {
                return true;
            } else {
                return false;
            }
        }

        return false;
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration) {
        canMove = false;
        float time = 0;
        Vector3 startPosition = enemy.position;

        while (time < duration) {
            enemy.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        enemy.position = targetPosition;
        canMove = true;
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
        LaunchMelee();

        time = 0;
        while (time < lungeDuration) {
            enemy.position = Vector3.Lerp(targetPosition, startPosition, time / lungeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        enemy.position = startPosition;
        canMove = true;
    }


    void LaunchMelee(){
        animator.SetBool("Attack",true);

        Vector3 spawnPosition = enemy.position + new Vector3(-1f, 0, 0); //tile behind
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = enemy.position + new Vector3(0, 0, 0); //current tile
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = enemy.position + new Vector3(1f, 0, 0); //tile infront
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        animator.SetBool("Attack", false);
    }

}
