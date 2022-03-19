using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BasicEnemy : Units
{
    // Random rand = new Random();
    Transform enemy;
    bool canMove = false;
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
    public GameObject firePoint;
    public Animator animator;
    public int projectileSpeed ;

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
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }
        }

        timer2 -= Time.deltaTime;

        if(timer2 <= 0) {
            time2 = UnityEngine.Random.Range(minShootSpd, maxShootSpd);
            timer2 = time2;

            StartCoroutine(Launch());
        }

    }

    Vector3 SetDirection(float randomNum) {
        Vector3 move = new Vector3(0, 0, 0);

        switch(randomNum) {
            case 0: //move forward
                return new Vector3(1f,0,0) + enemy.position;
                break;
            case 1: //move backward
                return new Vector3(-1f,0,0) + enemy.position;

                break;
            case 2: //move up
                return new Vector3(0,0,1f) + enemy.position;

                break;
            case 3: //move down
                return new Vector3(0,0,-1f) + enemy.position;
                break;
            default:
                return new Vector3(0,0,0);
        }
    }
    
    public bool OccupiedTile(Ray ray) {
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) {
            string tag = hit.collider.tag;

            if(tag == "Enemy") {
                Debug.Log("Tile is occupied");
                return true;
            }
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

    public override void Die() {
        base.Die();
        Destroy(this.gameObject);
        Debug.Log(enemy + " dead.");
    }

    public override void Sound() {

    }

    IEnumerator Launch() {
        animator.SetBool("Attack",true);
        yield return new WaitForSeconds(0.5f);
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.transform.position, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 lookDirection = new Vector3(-1f, 0, 0);
        projectile.Launch(lookDirection, projectileSpeed);
        animator.SetBool("Attack", false);
        
        // PlaySound(throwSound);
    }
}
