using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Units
{
    Transform player;
    Vector3 playerPos;
    public float duration = 0.09f;
    public GameObject projectilePrefab;
    Rigidbody rigidbody;
    bool canMove = true;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        playerPos = rigidbody.position;
    }

    public void Update() {

        if(canMove) {        

            if(Input.GetKeyDown(KeyCode.W)) {
                Vector3 move = new Vector3(0, 0, 1f) + player.position;

                if(!(move.x < 0 || move.z < 0 || move.x > 3 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

            }

            if(Input.GetKeyDown(KeyCode.A)) {
                Vector3 move = new Vector3(-1f, 0, 0) + player.position;

                if(!(move.x < 0 || move.z < 0 || move.x > 3 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

            }
            
            if(Input.GetKeyDown(KeyCode.S)) {
                Vector3 move = new Vector3(0, 0, -1f) + player.position;

                if(!(move.x < 0 || move.z < 0 || move.x > 3 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

            }

            if(Input.GetKeyDown(KeyCode.D)) {
                Vector3 move = new Vector3(1f, 0, 0) + player.position;

                if(!(move.x < 0 || move.z < 0 || move.x > 3 || move.z > 3)) {
                    StartCoroutine(LerpPosition(move, duration));
                }

            }

            if(Input.GetKeyDown(KeyCode.L)) //if between tiles, round up or down
            {
                Launch();
            }
        }

        if(Input.GetKeyDown(KeyCode.K)) {
            TakeDmg(5);
        }
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
        GameObject projectileObject = Instantiate(projectilePrefab, player.position, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 lookDirection = new Vector3(1f, 0, 0);
        projectile.Launch(lookDirection, 300);

        // animator.SetTrigger("Launch");
        
        // PlaySound(throwSound);
    }

    public override void Die() {
        base.Die();
        Destroy(this.gameObject);
        Debug.Log(player + " dead.");
    }
}
