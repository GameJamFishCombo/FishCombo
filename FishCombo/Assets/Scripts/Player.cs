using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementInput
{
    Up = 0,
    Left = 1,
    Down = 2,
    Right = 3,
}

public class Player : Units
{
    Transform player;
    Vector3 playerPos;
    public float duration = 0.09f;
    public float lungeDuration = 0.03f;
    public int numMeleeHits = 3;
    public float meleeHitDelay = 0.01f;
    public GameObject projectilePrefab;
    public GameObject pushPrefab;
    public GameObject areaProjectile;
    public GameObject meleeProjectile;

    Rigidbody rigidbody;
    bool canMove = true;
    public float projectileSpeed = 450;

    public Animator animator;

    private Queue<MovementInput> buffer;
    private Grid grid;

    void Awake()
    {
        buffer = new Queue<MovementInput>();
        rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        playerPos = rigidbody.position;
        grid = GameManager.grid;
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            if(buffer.Count < 2)
                buffer.Enqueue(MovementInput.Up);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(buffer.Count < 2)
                buffer.Enqueue(MovementInput.Left);
        }
            
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            if(buffer.Count < 2)
                buffer.Enqueue(MovementInput.Down);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            if(buffer.Count < 2)
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

        if(Input.GetKeyDown(KeyCode.C) && canMove) //if between tiles, round up or down
        {
            LaunchArea();
        }

        if(Input.GetKeyDown(KeyCode.V) && canMove) //if between tiles, round up or down
        {
            StartCoroutine(Lunge(transform.position + (new Vector3(4f, 0, 0))));
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

    IEnumerator Lunge(Vector3 targetPosition){ // KILL ME
        canMove = false;
        float time = 0;
        Vector3 startPosition = player.position;

        while (time < lungeDuration) {
            player.position = Vector3.Lerp(startPosition, targetPosition, time / lungeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        player.position = targetPosition;
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
            player.position = Vector3.Lerp(targetPosition, startPosition, time / lungeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        player.position = startPosition;
        canMove = true;
    }

    void LaunchMelee(){
        Vector3 spawnPosition = player.position + new Vector3(0, 0, -1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(0, 0, 0);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(0, 0, 1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(1f, 0, -1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(1f, 0, 0);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(1f, 0, 1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);
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

    void LaunchArea(){
        Vector3 spawnPosition = player.position + new Vector3(3f, 0, -1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(4f, 0, -1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(5f, 0, -1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(3f, 0, 0);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(4f, 0, 0);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(5f, 0, 0);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(3f, 0, 1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(4f, 0, 1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(5f, 0, 1);
        if(!grid.inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

    }

    public override void Die() {
        base.Die();
        Destroy(this.gameObject);
        Debug.Log(player + " dead.");
    }
}
