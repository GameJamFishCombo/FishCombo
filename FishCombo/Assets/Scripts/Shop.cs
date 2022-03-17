using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public int itemNum = 1;

    [Header("Health Object Spawn")]
    public GameObject medsPrefab;
    public GameObject meds;
    public Transform medsSpawn;
    public Animator medsAnim;
    
    [Header("Damage Object Spawn")]
    public GameObject syringePrefab;
    public GameObject syringe;
    public Transform syringeSpawn;
    public Animator syringeAnim;

    void Start()
    {
        itemNum = 1;
        meds = Instantiate(medsPrefab,medsSpawn);
        medsAnim = meds.GetComponent<Animator>();
        syringe = Instantiate(syringePrefab,syringeSpawn);
        syringeAnim = syringe.GetComponent<Animator>();
        medsAnim.SetBool("Selected",true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
            if(itemNum == 1){
                HoverItemTwo();
                return;
            }
            if(itemNum == 2){
                HoverItemOne();
                return;
            }
        }
    }

    void HoverItemOne(){
        itemNum = 1;
        syringeAnim.SetBool("Selected",false);
        medsAnim.SetBool("Selected",true);
    }
    
    void HoverItemTwo(){
        itemNum = 2;
        syringeAnim.SetBool("Selected",true);
        medsAnim.SetBool("Selected",false);
    }
}
