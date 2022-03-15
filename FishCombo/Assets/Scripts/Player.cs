using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Units
{
    Transform player;
    Vector3 playerPos;
    public float duration = 0.09f;
    public GameObject projectilePrefab;
    public GameObject pushPrefab;
    Rigidbody rigidbody;
    bool canMove = true;
    public float projectileSpeed = 450;
<<<<<<< Updated upstream
=======

    public Animator animator;

>>>>>>> Stashed changes
    private Queue<MovementInput> buffer;

    void Awake()
    {
        buffer = new Queue<MovementInput>();
        rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        playerPos = rigidbody.position;
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            if(buffer.Count < 3)
                buffer.Enqueue(MovementInput.Up);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(buffer.Count < 3)
                buffer.Enqueue(MovementInput.Left);
        }
            
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            if(buffer.Count < 3)
                buffer.Enqueue(MovementInput.Down);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            if(buffer.Count < 3)
                buffer.Enqueue(MovementInput.Right);
        }

        if(Input.GetKeyDown(KeyCode.Z) && canMove) //if between tiles, round up or down
        {
            Launch();
        }

        if(Input.GetKeyDown(KeyCode.X) && canMove) //if between tiles, round up or down
        {
            LaunchPush();
        }

        move();
    }

    private void move(){
        if(canMove && buffer.Count > 0) {        
            MovementInput input = buffer.Dequeue();
            bool checkBounds = true;
            if(input == MovementInput.Up) {
                Vector3 move = new Vector3(0, 0, 1f) + player.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }

            if(input == MovementInput.Left) {
                Vector3 move = new Vector3(-1f, 0, 0) + player.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }
            
            if(input == MovementInput.Down) {
                Vector3 move = new Vector3(0, 0, -1f) + player.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }

            if(input == MovementInput.Right) {
                Vector3 move = new Vector3(1f, 0, 0) + player.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }
        }
    }

    public bool inBounds(Vector3 vec) {
        if(vec.x < 0 || vec.x > 3 || vec.z < 0  || vec.z > 3) {
            return true;
        }

        return false;
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration) {
        canMove = false;
        float time = 0;
        Vector3 startPosition = player.position;

        while (time < duration) {
            player.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        player.position = targetPosition;
        canMove = true;
    }

    void Launch() {
        //animator.SetBool("Fire",true);
        animator.Play("Fire",-1,0.0f);
        GameObject projectileObject = Instantiate(projectilePrefab, player.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 lookDirection = new Vector3(1f, 0, 0);
        projectile.Launch(lookDirection, projectileSpeed);

        //animator.SetBool("Fire",false);
        // animator.SetTrigger("Launch");
        
        // PlaySound(throwSound);
    }

    void LaunchPush(){
        Vector3 spawnPosition = player.position + new Vector3(1f, 0, 1f);

        GameObject projectile = Instantiate(pushPrefab, spawnPosition, Quaternion.identity);
        PushProjectile pushProjectile = projectile.GetComponent<PushProjectile>();
        pushProjectile.Launch(new Vector3(1f, 0, 0));

        spawnPosition += new Vector3(0f, 0f, -1f);
        projectile = Instantiate(pushPrefab, spawnPosition, Quaternion.identity);
        pushProjectile = projectile.GetComponent<PushProjectile>();
        pushProjectile.Launch(new Vector3(1f, 0, 0));

        spawnPosition += new Vector3(0f, 0f, -1f);
        projectile = Instantiate(pushPrefab, spawnPosition, Quaternion.identity);
        pushProjectile = projectile.GetComponent<PushProjectile>();
        pushProjectile.Launch(new Vector3(1f, 0, 0));
    }

    public override void Die() {
        base.Die();
        Destroy(this.gameObject);
        Debug.Log(player + " dead.");
    }
}

public enum MovementInput
{
    Up = 0,
    Left = 1,
    Down = 2,
    Right = 3,
}