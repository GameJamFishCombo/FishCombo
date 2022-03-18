using System.Collections;
using System.Collections.Generic;
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
    // public HUDHealth HPBar;
    public AudioSource dmgSound;

    void Start() {
        currHP = maxHP;
    }

    public void TakeDmg(int dmg) {
        if(!invincible){
            dmg -= def;
            dmg = Mathf.Clamp(dmg, 0, int.MaxValue);
            currHP -= dmg;
            if(gameObject.tag != "Player")
                GameManager.comboManager.IncrementCombo();
            // HPBar.SetHealth(currHP);

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


}
