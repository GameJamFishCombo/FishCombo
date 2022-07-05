using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using EZCameraShake;

public class BossEnemy : Units
{
    // Random rand = new Random();
    Transform enemy;
    bool canMove = false;
    [Tooltip("Duration it takes to LERP between tiles.")]    
    public float duration = 0.09f; //time for lerp
    public float movementTime = 1, abilityTime = 2,
    movementTimer, abilityTimer;

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
    public float spikeWaitTime;
    public float projectileWaitTime;
    public void Awake() {
        enemy = GetComponent<Transform>();
        movementTimer = movementTime;
        abilityTimer = abilityTime;
        player = FindObjectOfType<Player>();
       //player = playerObj.GetComponent<Player>();
    }

    public void FixedUpdate() {
        float randomNum = Mathf.Floor((int)UnityEngine.Random.Range(0,4));
        Vector3 move = new Vector3(0, 0, 0);
        bool checkBounds = true, occupied = false;
        movementTimer -= Time.deltaTime;
        abilityTimer -= Time.deltaTime;
        Ray ray = new Ray(transform.position, -transform.right);

        if(movementTimer <= 0) {
            movementTime = UnityEngine.Random.Range(minMoveWaitTime, maxMoveWaitTime);
            movementTimer = movementTime;

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

        if(abilityTimer <= 0) {
            abilityTime = UnityEngine.Random.Range(minShootSpd, maxShootSpd);
            //abilityTimer = abilityTime;
            float abilityNum = Mathf.Floor((int)UnityEngine.Random.Range(0,2));
            switch(abilityNum){
                case(0f):
                    StartCoroutine(SpikeAnimationWait());
                    abilityTimer = 1;
                    break;
                case(1f):
                    StartCoroutine(ProjectileAnimationWait());
                    abilityTimer = 2;
                    break;
            }
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
    IEnumerator SpikeAnimationWait(){
        animator.SetBool("Attack1",true);
        yield return null;
        animator.SetBool("Attack1",false);
        yield return new WaitForSeconds(spikeWaitTime);
        StartCoroutine(Spikes());
    }

    IEnumerator ProjectileAnimationWait(){
         
        animator.SetBool("Attack2",true);
        yield return null;
        animator.SetBool("Attack2",false);
        yield return new WaitForSeconds(0.1f);
        AudioManager.PlaySound("FunkyFire");
        yield return new WaitForSeconds(projectileWaitTime);
        StartCoroutine(Launch());
        
    }

    IEnumerator Launch() {
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 lookDirection = new Vector3(-1f, 0, 0);
        projectile.Launch(lookDirection, 300);
        CameraShaker.Instance.ShakeOnce(1.5f, 1f, 0.1f, 0.3f);
        yield return null;
        // PlaySound(throwSound);
    }

    IEnumerator Spikes() {
        Debug.Log("Spawn spikes");
        Vector3 playerPos = player.getCurrPosition();
        yield return new WaitForSeconds(0.2f);
        AudioManager.PlaySound("GroundPound");
        CameraShaker.Instance.ShakeOnce(1.5f, 2f, 0.1f, 0.4f);
        GameObject projectileObject = Instantiate(spikesPrefab, playerPos, Quaternion.identity);
        //some animation that shows where obj gonna spawn
        SpikesProjectile projectile = projectileObject.GetComponent<SpikesProjectile>();
        yield return null;
    }
}
