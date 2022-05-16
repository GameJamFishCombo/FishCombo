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
            if(resetTimer <= 0){
                // Debug.Log("Reset Combo");
                float oldComboLv = comboLevel;
                comboLevel = 0;
                Gray();
                comboBar.SetCombo(comboLevel, oldComboLv);
            }
            else{
                resetTimer -= Time.deltaTime;
            }
        }
        
        if(maxed && maxCombo > comboLevel){
            maxed = false;
            comboBarObj.GetComponent<Animator>().SetTrigger("Static");
        }
       
    }

    public void IncrementCombo(){
        float oldComboLv = comboLevel;
        if(comboLevel < maxCombo)
        comboLevel += 1;

        if(maxCombo > comboLevel)
        comboBar.PlayAnimation("ComboBar");

        if(maxCombo == comboLevel){
            maxed = true;
            comboBarObj.GetComponent<Animator>().SetTrigger("Shake");
        }
        
        resetTimer = resetTime;
        //Debug.Log("Combo set to "+ comboLevel);
        comboBar.SetCombo(comboLevel, oldComboLv);
        if(comboLevel == 4){
            abilityCooldown1.GetComponent<Animator>().Play("AbilityIcon");
        }
        if(comboLevel == 12){
            abilityCooldown2.GetComponent<Animator>().Play("AbilityIcon");
        }
        if(comboLevel == 20){
            abilityCooldown3.GetComponent<Animator>().Play("AbilityIcon");
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
        }

        if(comboLevel < 12) {
            abilityCooldown2.SetCombo(1);
        }

        if(comboLevel < 5) {
            abilityCooldown1.SetCombo(1);
        }
        
       
    }

    public void Gray(){
        abilityCooldown1.SetCombo(1);
        abilityCooldown2.SetCombo(1);
        abilityCooldown3.SetCombo(1);
    }
}
