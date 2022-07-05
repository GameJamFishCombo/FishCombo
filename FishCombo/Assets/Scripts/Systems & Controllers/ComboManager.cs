using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public float resetTime = 2; // Seconds
    public float comboLevel = 0;

    private float resetTimer;
    public GameObject comboBarObj;
    public ComboBar comboBar;
    GameObject abilityCooldownObj1;
    GameObject abilityCooldownObj2;
    GameObject abilityCooldownObj3;
    AbilityCoolDown abilityCooldown1;
    AbilityCoolDown abilityCooldown2;
    AbilityCoolDown abilityCooldown3;
    private int maxCombo = 25;

    public bool comboing = true;

    public bool maxed = false;
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
        comboBar.SetMaxCombo(maxCombo);
    }

    // Update is called once per frame
    void Update()
    {
        if(comboLevel > 0){
            if(resetTimer <= 0 && comboing){
                comboing = false;
                // Debug.Log("Reset Combo");
                 maxed = false;
                float oldComboLv = comboLevel;
                comboLevel = 0;
                Gray();
                comboBar.SetCombo(comboLevel, oldComboLv);
                comboBarObj.GetComponent<Animator>().Play("Static");
                comboBarObj.GetComponent<Animator>().ResetTrigger("Static");

            }
            else{
                resetTimer -= Time.deltaTime;
            }
        }
        
        if(maxed && maxCombo > comboLevel){
            maxed = false;
            comboBarObj.GetComponent<Animator>().Play("Static");
            comboBarObj.GetComponent<Animator>().ResetTrigger("Static");
        }
       
    }

    public void IncrementCombo(){
        comboing = true;
        float oldComboLv = comboLevel;
        if(comboLevel < maxCombo)
        comboLevel += 1;

        if(maxCombo > comboLevel)
        comboBar.PlayAnimation("ComboBar");

        if(maxCombo == comboLevel){
            maxed = true;
            comboBarObj.GetComponent<Animator>().Play("ComboBarShake");
        }
        else maxed = false;
        
        resetTimer = resetTime;
        //Debug.Log("Combo set to "+ comboLevel);
        comboBar.SetCombo(comboLevel, oldComboLv);
        if(comboLevel == 4){
            abilityCooldown1.GetComponent<Animator>().Play("AbilityIcon");
            Debug.Log("Set Cooldown3 abailable cuz = 4");
        }
        if(comboLevel == 12){
            abilityCooldown2.GetComponent<Animator>().Play("AbilityIcon");
            Debug.Log("Set Cooldown3 abailable cuz = 12");
        }
        if(comboLevel == 20){
            abilityCooldown3.GetComponent<Animator>().Play("AbilityIcon");
            Debug.Log("Set Cooldown3 abailable cuz = 20");
        }
        if(comboLevel >= 4){
            abilityCooldown1.SetCombo(0);
        }
        if(comboLevel >= 12){
            abilityCooldown2.SetCombo(0);
        }
        if(comboLevel >= 20){
            abilityCooldown3.SetCombo(0);
        }
    }

    public void DecreaseCombo(int decrement){
        
        float oldComboLv = comboLevel;
        comboLevel -= decrement;
        comboBar.SetCombo(comboLevel, oldComboLv);
        if(comboLevel < 20) {
            abilityCooldown3.SetCombo(1);
            //abilityCooldown3.GetComponent<Animator>().SetTrigger("Static");
            //abilityCooldown3.GetComponent<Animator>().ResetTrigger("Static");
            abilityCooldown3.GetComponent<Animator>().Play("Static");
            Debug.Log("Set Cooldown3 static");
        }

        if(comboLevel < 12) {
            abilityCooldown2.SetCombo(1);
            //abilityCooldown2.GetComponent<Animator>().SetTrigger("Static");
            //abilityCooldown2.GetComponent<Animator>().ResetTrigger("Static");
            abilityCooldown2.GetComponent<Animator>().Play("Static");
            Debug.Log("Set Cooldown2 static");
        }

        if(comboLevel < 4) {
            abilityCooldown1.SetCombo(1);
           // abilityCooldown1.GetComponent<Animator>().SetTrigger("Static");
            //abilityCooldown1.GetComponent<Animator>().ResetTrigger("Static");
            abilityCooldown1.GetComponent<Animator>().Play("Static");
            Debug.Log("Set Cooldown1 static");
        }
        
       
    }

    public void Gray(){
         Debug.Log("Set all Cooldowns static");
        abilityCooldown1.SetCombo(1);
        //abilityCooldown1.GetComponent<Animator>().SetTrigger("Static");
        abilityCooldown1.GetComponent<Animator>().Play("Static");
        abilityCooldown2.SetCombo(1);
        //abilityCooldown2.GetComponent<Animator>().SetTrigger("Static");
        abilityCooldown2.GetComponent<Animator>().Play("Static");
        abilityCooldown3.SetCombo(1);
        //abilityCooldown3.GetComponent<Animator>().SetTrigger("Static");
        abilityCooldown3.GetComponent<Animator>().Play("Static");
    }
}
