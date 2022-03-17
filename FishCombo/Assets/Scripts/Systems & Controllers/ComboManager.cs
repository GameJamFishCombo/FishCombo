using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public float resetTime = 2; // Seconds
    public int comboLevel = 0;

    private float resetTimer;

    public ComboBar comboBar;

    // Start is called before the first frame update
    void Start()
    {
        comboBar.SetMaxCombo(100);
    }

    // Update is called once per frame
    void Update()
    {
        if(comboLevel > 0){
            if(resetTimer <= 0){
                Debug.Log("Reset Combo");
                comboLevel = 0;
                comboBar.SetCombo(comboLevel);
            }else{
                resetTimer -= Time.deltaTime;
            }
        }
    }

    public void IncrementCombo(){
        comboLevel += 1;
        resetTimer = resetTime;
        Debug.Log("Combo set to "+ comboLevel);
        comboBar.SetCombo(comboLevel);
    }

    public void DecreaseCombo(int decrement){
        comboLevel -= decrement;
        comboBar.SetCombo(comboLevel);
    }
}
