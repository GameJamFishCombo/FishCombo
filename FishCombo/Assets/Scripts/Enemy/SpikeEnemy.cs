using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpikeEnemy : Units
{
    // Random rand = new Random();
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
    public float minShootSpd = .5f;
    public float maxShootSpd = 3f;
    public GameObject projectilePrefab;

    public GameObject playerObj;
    Player player;
    public Animator animator;


    public void Awake() {
        enemy = GetComponent<Transform>();
        timer1 = time1;
        timer2 = time2;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();
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
            time2 = UnityEngine.Random.Range(minShootSpd, maxShootSpd);
            timer2 = time2;

            StartCoroutine(Spikes());
            Spikes();
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
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;
            case 1: //move left
                move = new Vector3(-1f, 0, 0) + enemy.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;

            case 2: //move south
                move = new Vector3(0, 0, -1f) + enemy.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;
            case 3: //move right
                move = new Vector3(1f, 0, 0) + enemy.position;
                checkBounds = inBounds(move);

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

                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;

                break;
            case MovementInput.Left: //move left
                move = new Vector3(-1f, 0, 0) + enemy.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;

            case MovementInput.Down: //move south
                move = new Vector3(0, 0, -1f) + enemy.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;
                
            case MovementInput.Right: //move right
                move = new Vector3(1f, 0, 0) + enemy.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;
        }
        return false;
    }

    public bool inBounds(Vector3 vec) {
        return (vec.x < 4 || vec.x > 7 || vec.z < 0  || vec.z > 3);
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

    IEnumerator Spikes() {
        Debug.Log("Spawn spikes");
        animator.SetBool("Attack",true);
        GameObject projectileObject = Instantiate(projectilePrefab, player.getCurrPosition(), Quaternion.identity);
        // animator.SetBool("Attack",true);
        yield return null;
        animator.SetBool("Attack",false);
        //some animation that shows where obj gonna spawn
        SpikesProjectile projectile = projectileObject.GetComponent<SpikesProjectile>();
        // animator.SetBool("Attack",false);

    }
}
