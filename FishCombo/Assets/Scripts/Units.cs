using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {
    None, Basic, Fast, Player, Boss 
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
    int comboPts;
    public int def;

    void Start() {
        currHP = maxHP;
    }

    public void TakeDmg(int dmg) {
        dmg -= def;
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);
        currHP -= dmg;
        if(gameObject.tag != "Player")
            GameManager.comboManager.IncrementCombo();

        if (currHP <= 0) {
            Die();
        }
    }
    
    public virtual void Die() {
        //Die in some way
        //This method is meant to be overwritten
        Debug.Log(transform.name + " died");
    }

    public void heal() {
        currHP = 100;
        Debug.Log("Player healed. HP at " + currHP);
        // HPBar.SetHealth(currHP);
    }


}
