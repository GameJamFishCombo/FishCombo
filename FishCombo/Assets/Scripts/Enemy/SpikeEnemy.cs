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
        player = FindObjectOfType<Player>();
        //player = playerObj.GetComponent<Player>();
    }

    public void FixedUpdate() {
        float randomNum = Mathf.Floor((int)UnityEngine.Random.Range(0,4));
        Vector3 move = new Vector3(0, 0, 0);
        bool checkBounds = true, occupied = false;
        timer1 -= Time.deltaTime;
        Ray ray = new Ray(transform.position, -transform.right);
        timer2 -= Time.deltaTime;

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

            StartCoroutine(Spikes());
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

    // public override void Sound() {

    // }

    IEnumerator Spikes() {
        // Debug.Log("Spawn spikes");
        animator.SetBool("Attack",true);
        StartCoroutine(Sound());
        Vector3 playerPos = player.getCurrPosition();
        Instantiate(projectilePrefab, playerPos, Quaternion.identity);
        yield return null;
        animator.SetBool("Attack",false);
        //some animation that shows where obj gonna spawn
        //SpikesProjectile projectile = projectileObject.GetComponent<SpikesProjectile>();
    }

    IEnumerator Sound(){
        yield return new WaitForSeconds(0.3f);
        AudioManager.PlaySound("GroundPound");
    }
}
