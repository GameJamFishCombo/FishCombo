using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Units
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


    public void Awake() {
        enemy = GetComponent<Transform>();
        timer1 = time1;
        timer2 = time2;
    }

    public void Update() {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;

        if(timer1 <= 0) {
            time1 = Random.Range(minMoveWaitTime, maxMoveWaitTime);
            timer1 = time1;

            Move();
        }

        if(timer2 <= 0) {
            time2 = Random.Range(minShootSpd, maxShootSpd);
            timer2 = time2;

            Launch();
        }

        
    }

    void Move() {
        int randomNum = (int)Random.Range(0,4);
        Vector3 move = new Vector3(0, 0, 0);

        switch(randomNum) {
            case 0: //move up
                move = new Vector3(0, 0, 1f) + enemy.position;

                // max 7 max 3 min 4 min 0

                if(!(move.x < 4 || move.z < 0 || move.x > 7 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }


                break;
            case 1: //move left
                move = new Vector3(-1f, 0, 0) + enemy.position;

                if(!(move.x < 4 || move.z < 0 || move.x > 7 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;

            case 2: //move south
                move = new Vector3(0, 0, -1f) + enemy.position;
                
                if(!(move.x < 4 || move.z < 0 || move.x > 7 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;
            case 3: //move right
                move = new Vector3(1f, 0, 0) + enemy.position;

                if(!(move.x < 4 || move.z < 0 || move.x > 7 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

                break;
        }
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

    void Launch() {
        GameObject projectileObject = Instantiate(projectilePrefab, enemy.position, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 lookDirection = new Vector3(-1f, 0, 0);
        projectile.Launch(lookDirection, 300);

        // animator.SetTrigger("Launch");
        
        // PlaySound(throwSound);
    }
}
