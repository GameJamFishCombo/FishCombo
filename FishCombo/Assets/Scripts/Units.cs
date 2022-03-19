using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum UnitType {
    None, Basic, Spike, Lunger, Player, Boss 
}

public class Units : MonoBehaviour
{
    public int team;
    public int currX, currY;
    public UnitType type;
    private Vector3 desiredPos, desiredScale;
    public int maxHP = 100;
    public int currHP {get; private set;}
    public int dmg;
    bool invincible = false;
    float invcibilityDuration = 0.0005f;
    int comboPts;
    public int def;
    public HUDHealth HPBar;
    public AudioSource dmgSound;

    void Start() {
        currHP = maxHP;
        // HPBar = gameObject.Find("Healthbar UI").GetComponent<HUDHealth>();
    }

    public void TakeDmg(int dmg) {
        if(!invincible){
            dmg -= def;
            dmg = Mathf.Clamp(dmg, 0, int.MaxValue);
            currHP -= dmg;
            if(gameObject.tag != "Player")
                GameManager.comboManager.IncrementCombo();
            HPBar.SetHealth(currHP);

            if (currHP <= 0) {
                Die();
            }
            StartCoroutine(Invincible(invcibilityDuration));
        }
        
    }

    IEnumerator Invincible(float duration){
        invincible = true;
        while(duration > 0){
            duration -= Time.deltaTime;
            yield return null;
        }
        invincible = false;
    }
    
    public virtual void Die() {
        //Die in some way
        //This method is meant to be overwritten
        Debug.Log(transform.name + " died");
    }

    public virtual void Sound() {
        Debug.Log(transform.name + " audibly took damage");
    }

    public void heal() {
        currHP = maxHP;
        Debug.Log("Player healed. HP at " + currHP);
        // HPBar.SetHealth(currHP);
    }

    public void IncreaseATK() {
        dmg += 1;
    }

    public void IncreaseMaxHP() {
        currHP += 20;
        maxHP += 20;
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration) {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration) {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    public bool inBounds(Vector3 vec) {
        if(vec.x < 0 || vec.x > 7 || vec.z < 0  || vec.z > 3) {
            return true;
        }

        return false;
    }

    public bool pushTo(MovementInput direction, float pushDuration){
        Vector3 move;
        bool checkBounds = true;
        transform.position = new Vector3((float)Math.Round(transform.position.x), transform.position.y, (float)Math.Round(transform.position.z));
        switch(direction) {
            case MovementInput.Up:
                move = new Vector3(0, 0, 1f) + transform.position;

                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;

                break;
            case MovementInput.Left: //move left
                move = new Vector3(-1f, 0, 0) + transform.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;

            case MovementInput.Down: //move south
                move = new Vector3(0, 0, -1f) + transform.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;
                
            case MovementInput.Right: //move right
                move = new Vector3(1f, 0, 0) + transform.position;
                checkBounds = inBounds(move);

                if(!checkBounds) {
                    StartCoroutine(LerpPosition(move, pushDuration));
                    return true;
                }
                return false;
        }
        return false;
    }

}
