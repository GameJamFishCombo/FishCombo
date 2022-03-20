using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public float resetTime = 2; // Seconds
    public int comboLevel = 0;

    private float resetTimer;
    public GameObject comboBarObj;
    public ComboBar comboBar;
    GameObject abilityCooldownObj1;
    GameObject abilityCooldownObj2;
    GameObject abilityCooldownObj3;
    AbilityCoolDown abilityCooldown1;
    AbilityCoolDown abilityCooldown2;
    AbilityCoolDown abilityCooldown3;

    void Awake() {
        comboBarObj = GameObject.Find("ComboBar");
        comboBar = comboBarObj.GetComponent<ComboBar>();

        abilityCooldownObj1 = GameObject.Find("Ability1");
        abilityCooldownObj2 = GameObject.Find("Ability2");
        abilityCooldownObj3 = GameObject.Find("Ability3");

        abilityCooldown1 = abilityCooldownObj1.GetComponent<AbilityCoolDown>();
        abilityCooldown2 = abilityCooldownObj2.GetComponent<AbilityCoolDown>();
        abilityCooldown3 = abilityCooldownObj3.GetComponent<AbilityCoolDown>();
    }

    // Start is called before the first frame update
    void Start()
    {
        comboBar.SetMaxCombo(36);
    }

    // Update is called once per frame
    void Update()
    {
        if(comboLevel > 0){
            if(resetTimer <= 0){
                Debug.Log("Reset Combo");
                comboLevel = 0;
                comboBar.SetCombo(comboLevel);
            }
            else{
                resetTimer -= Time.deltaTime;
            }
        }
    }

    public void IncrementCombo(){
        comboLevel += 1;
        comboBar.PlayAnimation("ComboBar");
        resetTimer = resetTime;
        //Debug.Log("Combo set to "+ comboLevel);
        comboBar.SetCombo(comboLevel);
        if(comboLevel == 5){
            abilityCooldown1.GetComponent<Animator>().Play("AbilityIcon");
            abilityCooldown1.SetCombo(0);
        }
        if(comboLevel == 15){
            abilityCooldown2.GetComponent<Animator>().Play("AbilityIcon");
            abilityCooldown2.SetCombo(0);
        }
        if(comboLevel == 30){
            abilityCooldown3.GetComponent<Animator>().Play("AbilityIcon");
            abilityCooldown3.SetCombo(0);
        }
    }

    public void DecreaseCombo(int decrement){
        comboLevel -= decrement;
        comboBar.SetCombo(comboLevel);
        
        if(comboLevel < 5) {
            abilityCooldown1.SetCombo(1);
        } else if(comboLevel < 15) {
            abilityCooldown2.SetCombo(1);
        } else if(comboLevel < 30) {
            abilityCooldown3.SetCombo(1);
        }
    }
}
