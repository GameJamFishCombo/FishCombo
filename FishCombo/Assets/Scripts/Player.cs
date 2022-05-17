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
    private int lungeCost = 4;
    private int pushCost = 12;
    private int areaCost = 20;
    public float duration = 0.09f;
    public float lungeDuration = 0.03f;
    public int numMeleeHits = 3;
    public float meleeHitDelay = 0.01f;
    public GameObject projectilePrefab;
    public GameObject pushPrefab;
    public GameObject areaProjectile;
    public GameObject meleeProjectile;
    public Transform firePoint;
    Rigidbody rigidbody;
    bool canMove = true, canAutoFire = true, canCast = true;
    public float projectileSpeed = 450;
    public GameObject recordAbility;
    public Animator animator;
    public float lungeAnimationCooldown = 0.3f;
    public float pushAnimationCooldown = 0.6f;
    public float areaAnimationCooldown = 0.6f;
    private Queue<MovementInput> buffer;
    private Grid grid;
    private ComboManager comboManager;
    public PauseMenu pauseMenuUI;
    public GameObject deathParticles;
    void Awake()
    {
        buffer = new Queue<MovementInput>();
        rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        playerPos = rigidbody.position;
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        comboManager = GameObject.Find("Combo Manager").GetComponent<ComboManager>();
        pauseMenuUI = GameObject.Find("Canvas").GetComponent<PauseMenu>();
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

        if((Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E)) && canMove && canAutoFire && canCast) 
        {
            StartCoroutine(PrimaryCooldown());
        }

        // if(Input.GetKey(KeyCode.R) && canAutoFire) //when holding down, shoot auto
        // {
        //     StartCoroutine(PrimaryCooldown());
        // }

        if(Input.GetKeyDown(KeyCode.W) && canMove && comboManager.comboLevel >= pushCost && canCast) //if between tiles, round up or down
        {
            // attackSound2.Play();
            AudioManager.PlaySound("Player Special Attack 2");
            comboManager.DecreaseCombo(pushCost);
            // StartCoroutine(AnimationTimer(pushAnimationCooldown));
            //LaunchPush(); moved into animatorwait
            StartCoroutine(PushAnimationWait("Attack2"));
        }

        if(Input.GetKeyDown(KeyCode.R) && canMove && comboManager.comboLevel >= areaCost && canCast) //if between tiles, round up or down
        {
            // attackSound1.Play();
            AudioManager.PlaySound("Player Special Attack 2");
            comboManager.DecreaseCombo(areaCost);
            StartCoroutine(AnimationTimer(areaAnimationCooldown));
            StartCoroutine(AreaAnimationWait("Attack3"));
        }

        if(Input.GetKeyDown(KeyCode.Q) && canMove && comboManager.comboLevel >= lungeCost && canCast) //if between tiles, round up or down
        {
            // attackSound2.Play();
            AudioManager.PlaySound("Player Special Attack 1");
            comboManager.DecreaseCombo(lungeCost);
            StartCoroutine(AnimationTimer(lungeAnimationCooldown));
            StartCoroutine(Lunge(transform.position + (new Vector3(4f, 0, 0))));
        }

        move();
    }

    IEnumerator PrimaryCooldown() {
        canAutoFire = false;
        AudioManager.PlaySound("Player Attack");
        Launch();
        yield return new WaitForSeconds(.15f);
        canAutoFire = true;
    }

    private void move(){
        if(canMove && buffer.Count > 0) {        
            MovementInput input = buffer.Dequeue();
            bool checkBounds = true;
            if(input == MovementInput.Up) {
                Vector3 move = new Vector3(0, 0, 1f) + player.position;
                checkBounds = inBounds(move, "Player");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }

            if(input == MovementInput.Left) {
                Vector3 move = new Vector3(-1f, 0, 0) + player.position;
                checkBounds = inBounds(move, "Player");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }
            
            if(input == MovementInput.Down) {
                Vector3 move = new Vector3(0, 0, -1f) + player.position;
                checkBounds = inBounds(move, "Player");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }

            if(input == MovementInput.Right) {
                Vector3 move = new Vector3(1f, 0, 0) + player.position;
                checkBounds = inBounds(move, "Player");

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, duration));
                }
            }
        }
    }

    public bool inBounds(Vector3 vec, string tag) {
        if(tag == "Player") {
            if(vec.x < 0 || vec.x > 3 || vec.z < 0  || vec.z > 3) {
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
        StartCoroutine(AnimationWait("Attack1"));
        StartCoroutine(Invincible(0.55f));
        canMove = false;
        float time = 0;
        Vector3 startPosition = player.position;

        while (time < lungeDuration) {
            player.position = Vector3.Lerp(startPosition, targetPosition, time / lungeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        player.position = targetPosition;
        AudioManager.PlaySound("LungeAbility");
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
        //StartCoroutine(AnimationWait("Attack3"));
        Vector3 spawnPosition = player.position + new Vector3(0, 0, -1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(0, 0, 0);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(0, 0, 1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(1f, 0, -1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(1f, 0, 0);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(1f, 0, 1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(meleeProjectile, spawnPosition, Quaternion.identity);
    }

    void Launch() {
        AudioManager.PlaySound("Player Attack");

        //animator.SetBool("Fire",true);
        animator.Play("Fire",-1,0.0f);
        GameObject projectileObject = Instantiate(projectilePrefab, roundFirePt(), Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 lookDirection = new Vector3(1f, 0, 0);
        projectile.Launch(lookDirection, projectileSpeed);

        //animator.SetBool("Fire",false);
        // animator.SetTrigger("Launch");
        
        // PlaySound(throwSound);
    }

    void LaunchPush(){
        
        Vector3 spawnPosition = getCurrPosition() + new Vector3(1f, 0, 1f);

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
        StartCoroutine(AnimationWait("Attack3"));
        Vector3 spawnPosition = getCurrPosition() + new Vector3(3f, 0, -1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(4f, 0, -1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(5f, 0, -1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(3f, 0, 0);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(4f, 0, 0);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(5f, 0, 0);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(3f, 0, 1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(4f, 0, 1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

        spawnPosition = player.position + new Vector3(5f, 0, 1);
        if(!inBounds(spawnPosition, "Projectile"))
            Instantiate(areaProjectile, spawnPosition, Quaternion.identity);

    }

    IEnumerator AnimationWait(string animation){
        animator.SetBool(animation,true);
        yield return null;
        animator.SetBool(animation,false);
    }

    IEnumerator PushAnimationWait(string animation){
        StartCoroutine(AnimationTimer(pushAnimationCooldown));
        animator.SetBool(animation,true);
        yield return null;
        animator.SetBool(animation,false);
        yield return new WaitForSeconds(0.5f);
        AudioManager.PlaySound("PushAbilityClap");
        LaunchPush();
    }

    IEnumerator AreaAnimationWait(string animation){
        Instantiate(recordAbility,getCurrPosition(),Quaternion.identity);
        animator.SetBool(animation,true);
        yield return null;
        animator.SetBool(animation,false);
        yield return new WaitForSeconds(0.5f);
        LaunchArea();
    }

    IEnumerator AnimationTimer(float timer){
        canCast = false;
        while(timer > 0){
            yield return null;
            timer -= Time.deltaTime;
        }
        canCast = true;
    }

    public override void Die() {
        // base.Die();
        //deathSound.Play();
        if(deathParticles != null)
            Instantiate(deathParticles,transform.position,Quaternion.identity);
        AudioManager.PlaySound("Player Death");
        // Destroy(this.gameObject);
        Debug.Log(player + " dead.");
        dmg = 2; //reset amps
        maxHP = 100;        
        pauseMenuUI.Death();
        Destroy(this.gameObject);
    }

    public override void Sound() {

    }

    public Vector3 getCurrPosition() {
        Vector3 playerPos = player.position;

        float x = Mathf.Round(playerPos.x);
        float z = Mathf.Round(playerPos.z);

        playerPos = new Vector3(x,.5f,z);

        return playerPos;
    }

    public Vector3 roundFirePt() {
        Vector3 fingerPos = firePoint.position;

        float x = Mathf.Round(fingerPos.x);
        float z = Mathf.Round(fingerPos.z);

        playerPos = new Vector3(x,.5f,z);

        return fingerPos;
    }

}
