using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Units
{
    // Random rand = new Random();
    Transform enemy;
    bool canMove = true;    
    public float duration = 0.09f;
    public float time = 1, timer;
    public float speed = .2f;


    public void Awake() {
        enemy = GetComponent<Transform>();
        timer = time;
    }

    public void Update() {
        timer -= Time.deltaTime;

        if(timer <= 0) {
            time = Random.Range(duration, speed);
            timer = time;

            move();
        }

        
    }

    void move() {
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
}
